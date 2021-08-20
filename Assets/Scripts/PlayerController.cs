using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infinite_story
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player setings")]
        // сила прыжка
        public float jumpForce = 5.0f;
        
        [Range(1f, 10f)] public float sensivity = 1f;
        
        public static Action<int> currentScore { get; set; }
        public static Action<int> NewSpeed;

        public GameObject Player;
        public static Action<GameObject> SetPlayer;

        public Camera Cam;
        public static Action<Camera> SetCamera;
        // здесь храним счёт
        [Header("Floor accel by time")]
        [Tooltip("How much roads must be spawned before speed grow")]public int FloorAccelOver = 0;
        [Tooltip("How much should the scrolling speed of the road increase")]public int FloorAccelStep = 0;
        [Tooltip("Scroll speed limit")]public int FloorAccelLimit = 0;

        [Header("Floor Settings")]
        public int ScrollSpeed = 18;
        [Tooltip("First road piece coords")]
        public Vector3 FirstFloorPos = new Vector3(0, 0, -122);

        // будем передовать в конструктор FloorController информацию о бонусах
        [Header("Bonuses settings")]
        // размер сетки, на которую поделится площадка. В каждом узле сетки сможет появлятся бонус

        public int GridSizeX = 6;
        public int GridSizeZ = 20;

        public int GoodBonusesNumber = 20;
        public int BadBonusesNumber = 10;
        public int ModNumber = 0;
        public int TrapNumber = 4;

        public List<GameObject> BadBonuses;
        public List<GameObject> GoodBonuses;
        public List<GameObject> Modificators;
        public List<GameObject> Traps;

        [Header("Tags settings")]
        [Tooltip("Tag for road. Need for save/load game")] public string RoadTag = "Road";
        [Tooltip("Tag for bonuses. Need for save/load game")] public string BonusesTag = "Bonuses";
        [Header("Prefabs settings")]
        [Tooltip("Prefab path")] public string PrefabPath = "Prefabs";

        public static Action<string> SetRoadTag;
        public static Action<string> SetBonusesTag;

        private PreloadPrefabs PrefabsLoader;

        // очки не могут быть меньше 0
        [HideInInspector]public int Score
            
        { 
            get { return _score; } 
            set 
            {
                _score = value;
                if (_score < 0)
                {
                    _score = 0;
                };
            } 
        }
        private int _score;
        private Rigidbody _rb;
        private int _oldScrollSpeed;

        private FloorController _floorCtl;
        // переменная для управления игроком
        private float _deltaX = 0;
        
        
        // FPS для дебага
        private int _fps = 0;
        
        private SphereCollider _playerCollider;
        private int _BonusesSpawnCounter = 0;

        private CamConroller CamCtl;
        // событие будет сообщать, если игрок за пределами экрана
        public static Action<bool> IsPlayerVisible;

        private bool _playerMustBePushed = true;

        private List<GameObject> BonusesObjList;
        Dictionary<int, GameObject> PreloadedPrefabsList;

        private void Awake()
        {
            // подписываемся на события
            BonusAction.BonusesAction += OnScoreChanged;
            ColliderWatchdog.SpawnColliderHit += OnRoadSpawn;
            LoadGame.SetScore += SetScoreAtLoad;
            LoadGame.SetScrollSpeed += OnScrollSpeedChange;
            LoadGame.SetPlayerPos += SetPlayerPos;
            DisableForceAtStack.DisableForce += DisableForce;
            DisableForceAtStack.EnableForce += EnableForce;

        }

        private void OnDestroy()
        {
            // отписываемся от событий при выгрузке сцены

            BonusAction.BonusesAction -= OnScoreChanged;
            _floorCtl.OnDestroy();
            ColliderWatchdog.SpawnColliderHit -= OnRoadSpawn;
            LoadGame.SetScore -= SetScoreAtLoad;
            LoadGame.SetScrollSpeed -= OnScrollSpeedChange;
            LoadGame.SetPlayerPos -= SetPlayerPos;
            DisableForceAtStack.DisableForce -= DisableForce;
            DisableForceAtStack.EnableForce -= EnableForce;
        }

        private void DisableForce()
        {
            _playerMustBePushed = false;
        }

        private void EnableForce()
        {
            _playerMustBePushed = true;
        }
      
        /// <summary>
        /// Grow speed after some road respawns
        /// </summary>
        void OnRoadSpawn()
        {
            _BonusesSpawnCounter++;
            if (FloorAccelOver > 0 && _BonusesSpawnCounter % FloorAccelOver == 0 && ScrollSpeed < FloorAccelLimit)
            {
                /*
                if(FloorAccelLimit < ScrollSpeed)
                {
                    ScrollSpeed = FloorAccelLimit;
                }
                */
                ScrollSpeed += FloorAccelStep;
                
            }
            NewSpeed?.Invoke(ScrollSpeed);
        }

        void OnScrollSpeedChange(int NewScroolSpeed)
        {
            ScrollSpeed = NewScroolSpeed;
        }
        
        void OnScoreChanged(int newScore)
        {
            Score += newScore;
            currentScore?.Invoke(Score);
        }

        void SetScoreAtLoad(int newScore)
        {
            Score = newScore;
            currentScore?.Invoke(Score);
        }

        void SetPlayerPos(Vector3 pos)
        {
            Player.transform.position = pos;
        }

        
        void Start()
        {
            Score = 0;
#if UNITY_EDITOR
            sensivity /= 3;
#endif
            _oldScrollSpeed = ScrollSpeed;
            _rb = GetComponent<Rigidbody>();
            _playerCollider = Player.GetComponent<SphereCollider>();
            currentScore?.Invoke(Score);
            //Спавним полы
            
            _floorCtl = new FloorController(
                FirstFloorPos,
                GoodBonuses,
                GoodBonusesNumber,
                BadBonuses,
                BadBonusesNumber,
                Modificators,
                ModNumber,
                Traps,
                TrapNumber,
                GridSizeX,
                GridSizeZ);

            _floorCtl.Awake();
            _floorCtl.Start();

            BonusesObjList = new List<GameObject>();
            
            if (GoodBonuses.Count > 0) BonusesObjList.AddRange(GoodBonuses);
            if (BadBonuses.Count > 0) BonusesObjList.AddRange(BadBonuses);
            if (Modificators.Count > 0) BonusesObjList.AddRange(Modificators);
            if (Traps.Count > 0) BonusesObjList.AddRange(Traps);

            PrefabsLoader = new PreloadPrefabs(PrefabPath);

            PreloadedPrefabsList = PrefabsLoader.LoadPrefab(BonusesObjList);
            if (PreloadedPrefabsList != null)
            {
                foreach(KeyValuePair <int, GameObject> kvp in PreloadedPrefabsList)
                {
                    Debug.Log($"hash: {kvp.Key}, name: {kvp.Value.name}");
                }
            }
            GameObject SpawnedRoad = _floorCtl.SpawnedRoad;
            NewSpeed?.Invoke(ScrollSpeed);
            // Спавним камеру
            CamCtl = new CamConroller(transform, Cam, SpawnedRoad);
            //CamCtl.Awake();
            CamCtl.Start();
            SetPlayer?.Invoke(Player);
            SetCamera?.Invoke(Cam);
            SetRoadTag?.Invoke(RoadTag);
            SetBonusesTag?.Invoke(BonusesTag);
        }

        private void FixedUpdate()
        {
            if (_playerMustBePushed)
            {
                _rb.AddForce(Vector3.forward * Mathf.Pow(ScrollSpeed, 2));
            }
            //_rb.AddTorque(Vector3.right * _scrollSpeed);
            _floorCtl.Update(ScrollSpeed);
        }
        private void Update()
        {

            // определяем, приземлён ли игрок
            bool hitGround = false;
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
            {
                float check = _playerCollider.radius + 0.1f;
                hitGround = hit.distance <= check;  // to be sure check slightly beyond bottom of capsule
            }
            // контроль игрока
            _deltaX = Input.GetAxis("Horizontal");
            if (_deltaX != 0)
            {
                _rb.AddForce(new Vector3(_deltaX * sensivity, 0, 0));
            }
            if (Input.GetButtonDown("Jump") && hitGround)
            {
                _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            }
            // Сообщаем новую скорость, если она изменилась
            if(_oldScrollSpeed != ScrollSpeed)
            {
                NewSpeed?.Invoke(ScrollSpeed);
                _oldScrollSpeed = ScrollSpeed;
            }

            //_floorCtl.Update(ScrollSpeed);
            CamCtl.Update();

#if UNITY_EDITOR
            // определяем FPS
            _fps = (int)(1f / Time.unscaledDeltaTime);
#endif
            


            // debug
            /*
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                score++;
            }

            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                score--;
            }
            */
        }


        
#if UNITY_EDITOR
        // рисуем fps
        private void OnGUI()
        {
            GUI.Label(new Rect(Screen.width - 200, 200, 100, 100), $"FPS: {_fps} in {this.GetType()}, _score: {Score}\n" +
                $"ScrollSpeed: {ScrollSpeed}" );

        }

#endif
        void OnBecameInvisible()
        {
            IsPlayerVisible?.Invoke(false);
        }

        void OnBecameVisible()
        {
            IsPlayerVisible?.Invoke(true);
        }
        
    }
}