using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infinite_story
{
    public class BonusesSpawner
    
    {
        /// <summary>
        /// Spawn bonuses to map
        /// </summary>
        /// <param name="gb">Good bonuses quantity</param>
        /// <param name="bb">Bad bonuses quantity</param>
        /// <param name="trapN">Traps quantity</param>
        /// <param name="gridX">Cells for bonuses along X axis</param>
        /// <param name="gridZ">Cells for bonuses along Z axis</param>
        /// <param name="goodbonuses">List of GameObject as Good bonuses</param>
        /// <param name="badbonuses">List of GameObject as Bad bonuses</param>
        /// <param name="mods">List of GameObject as Player's Modificator</param>
        /// <param name="traps">List of GameObject as Traps</param>
        /// <param name="RoadObject">Object where represent sizes of road</param>
        public BonusesSpawner(
            int gb, 
            int bb, 
            int trapN, 
            int gridX, 
            int gridZ,
            List<GameObject> badbonuses,
            List<GameObject> goodbonuses,
            List<GameObject> mods,
            List<GameObject> traps,
            GameObject RoadObject
            ) 
        {
            GoodBonusesNumber = gb;
            BadBonusesNumber = bb;
            TrapNumber = trapN;
            GridSizeX = gridX;
            GridSizeZ = gridZ;
            BadBonuses = badbonuses;
            GoodBonuses = goodbonuses;
            Modificators = mods;
            Traps = traps;
            RoadObj = RoadObject;
        }
       
        public int GridSizeX;
        public int GridSizeZ;

        public int GoodBonusesNumber;
        public int BadBonusesNumber;
        public int ModsNumber;
        public int TrapNumber;

        public List<GameObject> BadBonuses;
        public List<GameObject> GoodBonuses;
        public List<GameObject> Modificators;
        public List<GameObject> Traps;
        public GameObject RoadObj;
        //private float _scrollSpeed = 0;

        //public static Action<List<GameObject>> SaveFileDelegate;
        public static System.Action OnSpawnBonuses;

        private List<Vector3> _spawnCoords;
        private Vector3 _colliderBeginPoint;

        private Collider _bounds;

        // Объект, к которому привязаны бонусы
        public GameObject RootBonusesObjects;

        public void Awake()
        {
            //RootObjectsList = new List<GameObject>();
            // Событие сохранения игры
            //UIController.SaveFileEvent += OnSaveFile;
            //PlayerController.NewSpeed += OnSpeedChanged;
            // удаляем бонусы при спауне игрока
            //PlayerController.RemoveBonusesAtRespawn += RmBonuses;
        }

        public void OnDestroy()
        {
            //UIController.SaveFileEvent -= OnSaveFile;
        }

        

        public void SpawnBonuses()
        {
            // создаём новый GameObject, чтобы привязать к нему бонусы этого спавна
            RootBonusesObjects = new GameObject();
            // Поищем в списках тэгиобъектов, чтобы потом назодить и удалять при загрузки объекты со сцены
            if(BadBonuses.Count > 0)
            {
                RootBonusesObjects.tag = BadBonuses[0].gameObject.tag;
            }
            else if (GoodBonuses.Count > 0)
            {
                RootBonusesObjects.tag = GoodBonuses[0].gameObject.tag;
            }
            else if (Modificators.Count > 0)
            {
                RootBonusesObjects.tag = Modificators[0].gameObject.tag;
            }
            else if (Traps.Count > 0)
            {
                RootBonusesObjects.tag = Traps[0].gameObject.tag;
            }
            //_bounds = FloorScroller.Bounds;
            _bounds = RoadObj.GetComponent<BoxCollider>();

            // начало координат BoxCollider у пола
            _colliderBeginPoint = new Vector3(_bounds.bounds.center.x - _bounds.bounds.extents.x,
            _bounds.bounds.center.y + _bounds.bounds.extents.y,
            _bounds.bounds.center.z - _bounds.bounds.extents.z);


            // Делим площадку на сетку.
            float pointsX = _bounds.bounds.size.x / GridSizeX;
            float pointsZ = _bounds.bounds.size.z / GridSizeZ;
            _spawnCoords = new List<Vector3>();

            // разбиваем площадку на сетку, в которой будут респаунится бонусы
            for (int i = 0; i < GridSizeX; i++)
            {
                for (int j = 0; j < GridSizeZ; j++)
                {
                    // самая левая кромка поля нас не интересует
                    if (i > 0)
                    {
                        Vector3 currentPoint = new Vector3(_colliderBeginPoint.x + pointsX * i, _colliderBeginPoint.y, _colliderBeginPoint.z + pointsZ * j);
                        _spawnCoords.Add(currentPoint);
                        //Debug.Log(currentPoint);
                    }
                }
            }

            
            RootBonusesObjects.transform.position = Vector3.zero;

            BonusesSpawn(BadBonuses, BadBonusesNumber, RootBonusesObjects);
            BonusesSpawn(GoodBonuses, GoodBonusesNumber, RootBonusesObjects);
            BonusesSpawn(Traps, TrapNumber, RootBonusesObjects);
            BonusesSpawn(Modificators, ModsNumber, RootBonusesObjects);
            OnSpawnBonuses?.Invoke();
        }
                       
        public void BonusesSpawn(List<GameObject> bonusesList, int bonusesNum, GameObject parent)
        {
            if (bonusesList.Count > 0 && _spawnCoords.Count > 0)
            {
                try
                {
                    for (int i = 0; i < bonusesNum; i++)
                    {
                        // Количество типов бонусов в списке (задаётся в редакторе)
                        int BonusesTypeNum = Random.Range(0, bonusesList.Count);
                        // случайно выбираем координаты
                        int BonusInstanceCoords = Random.Range(0, _spawnCoords.Count);
                        Vector3 spawnInstanceCoords = _spawnCoords[BonusInstanceCoords];
                        // спавним префаб над полом
                        GameObject bonusInstance = GameObject.Instantiate(bonusesList[BonusesTypeNum], parent.transform, true);
                        Renderer instanceRend = bonusInstance.GetComponent<Renderer>();
                        bonusInstance.transform.position = spawnInstanceCoords;
                        bonusInstance.transform.position = new Vector3(bonusInstance.transform.position.x,
                        bonusInstance.transform.position.y + instanceRend.bounds.extents.y,
                        bonusInstance.transform.position.z);
                        // удаляем координаты из списка после спавна, чтобы на этом месте ещё что-то не появилось
                        _spawnCoords.Remove(spawnInstanceCoords);
                    }
                    
                }
                catch (ArgumentOutOfRangeException)
                {
                    Debug.LogError($"Бонусов больше, чем ячеек");
                }
            }
        }

        public void Update(int ScrollSpeed)
        {
            RootBonusesObjects.transform.Translate(-Vector3.forward * ScrollSpeed * Time.deltaTime);
            
        }
    }
}