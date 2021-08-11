using System.Collections.Generic;
using UnityEngine;
using System;

namespace Infinite_story
{
    public class FloorScroller : MonoBehaviour
    {
        public int scrollSpeed = 10;
        
        // статические переменные, понадобятся чтобы получать доступ к их содержимому отовсюду
        [HideInInspector] public static Collider Bounds;
        public static Action<int> CorrespondScrollSpeed;
        public static System.Action OnRoadSpawned;
        public static System.Action OnRoadDestroyed;
        public static Action<BonusesSpawner> OnBonusesSpawned;

        BonusesSpawner Bonuses;

        // размер сетки, на которую поделится площадка. В каждом узле сетки сможет появлятся бонус

        public int GridSizeX = 6;
        public int GridSizeZ = 20;

        public int GoodBonusesNumber = 20;
        public int BadBonusesNumber = 10;
        public int TrapNumber = 4;

        public List<GameObject> BadBonuses;
        public List<GameObject> GoodBonuses;
        public List<GameObject> Modificators;
        public List<GameObject> Traps;

        private Transform _floor;

        void Awake()
        {
            Bounds = GetComponent<BoxCollider>();
        }

        void Start()
        {
            _floor = GetComponent<Transform>();
            // Создаём событие, которое сообщает скорость движения платформы
            CorrespondScrollSpeed?.Invoke(scrollSpeed);
            // Событие, которое сообщает, что очередной кусок дороги респаунился
            OnRoadSpawned?.Invoke();
            GameObject RootObj = new GameObject();
            Bonuses = new BonusesSpawner(
                    GoodBonusesNumber,
                    BadBonusesNumber,
                    TrapNumber,
                    GridSizeX,
                    GridSizeZ,
                    BadBonuses,
                    GoodBonuses,
                    Modificators,
                    Traps,
                    RootObj
                    );
            Bonuses.Awake();
            Bonuses.SpawnBonuses();
            //Bonuses.OnSpeedChanged(scrollSpeed);

            //OnBonusesSpawned?.Invoke(Bonuses);
        }

        private void OnSpeedChange(int NewSpeed)
        {
            scrollSpeed = NewSpeed;
 //           CorrespondScrollSpeed?.Invoke(scrollSpeed);
        }

        private void Update()
        {
            //Bonuses.Update();
            // заставляем пол двигаться
            _floor.transform.Translate(-Vector3.forward * scrollSpeed * Time.deltaTime); 
        }
        
    }
}
