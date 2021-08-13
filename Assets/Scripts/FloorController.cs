using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infinite_story
{
    public class FloorController
    {
        public GameObject SpawnedRoad;
        private List<GameObject> _roadList;
        private Spawner _spawner;
        private int SpawnCounter = 0;
        private Vector3 _startPos;

        private int _gridSizeX;
        private int _gridSizeZ;

        private int _goodBonusesNumber;
        private int _badBonusesNumber;
        private int _trapNumber;
        private int _modsNumber;

        private List<GameObject> _badBonuses;
        private List<GameObject> _goodBonuses;
        private List<GameObject> _modificators;
        private List<GameObject> _traps;

        private BonusesSpawner _bonusesSpawner;
        private List<BonusesSpawner> _bsList;
        
        // события для сохраниеня 
        public static Action<List<GameObject>> SetRoads;
        public static Action<List<BonusesSpawner>> SetBonuses;
        public static Action Save;

        public int ScrollSpeed;
     
        // Спаун без бонусов
        public FloorController(Vector3 StartPos)
        {
            _startPos = StartPos;
        }


        
        public FloorController(
            Vector3 StartPos, 
            List<GameObject> GoodBonuses, 
            int GoodBonusesNum, 
            List<GameObject> BadBonuses, 
            int BadBonusesNum, 
            List<GameObject> Modificators, 
            int ModificatorsNum,
            List<GameObject> Traps,
            int TrapsNum,
            int GridSizeX,
            int GridSizeZ
            )
        {
            _startPos = StartPos;
            _goodBonuses = GoodBonuses;
            _goodBonusesNumber = GoodBonusesNum;
            _badBonuses = BadBonuses;
            _badBonusesNumber = BadBonusesNum;
            _modificators = Modificators;
            _modsNumber = ModificatorsNum;
            _traps = Traps;
            _trapNumber = TrapsNum;
            _gridSizeX = GridSizeX;
            _gridSizeZ = GridSizeZ;

        }




        private void SpawnRoad()
        {
            _spawner = new Spawner();
            _spawner.Start();
            GameObject LastRoad = _roadList[_roadList.Count - 1];
            GameObject Road = _spawner.SpawnNewRoad(new Vector3(LastRoad.transform.position.x,LastRoad.transform.position.y, LastRoad.transform.position.z));
            //Road.name = $"Road{SpawnCounter++}";
            // Spawn some bonuses
            _bonusesSpawner = new BonusesSpawner(
                _goodBonuses, 
                _goodBonusesNumber,
                _badBonuses,
                _badBonusesNumber,
                _modificators, 
                _modsNumber,
                _traps,
                _trapNumber, 
                _gridSizeX, 
                _gridSizeZ,    
                Road
                );
            _bsList.Add(_bonusesSpawner);
            //_bonusesSpawner.Awake();
            _bonusesSpawner.SpawnBonuses();
            _roadList.Add(Road);
            if(_roadList.Count > 2)
            {
                GameObject.Destroy(_roadList[0]);
                _roadList.Remove(_roadList[0]);
            }
            
            if(_bsList.Count > 2)
            {
                GameObject.Destroy(_bsList[0].RootBonusesObjects);
                _bsList.Remove(_bsList[0]);
            }
            

        }


        public void Awake()
        {
            ColliderWatchdog.SpawnColliderHit += SpawnRoad;
            UIController.SaveFileEvent += SaveFile;
            
        }

        public void OnDestroy()
        {
            ColliderWatchdog.SpawnColliderHit -= SpawnRoad;
            UIController.SaveFileEvent -= SaveFile;
            _bonusesSpawner?.OnDestroy();
        }


        public void Start()
        {
            _roadList = new List<GameObject>();
            _spawner = new Spawner();
            _spawner.Start();
            GameObject Road = _spawner.SpawnNewRoad(_startPos);

            SpawnedRoad = Road;
            //Road.name = $"Road{SpawnCounter++}";
            _roadList.Add(Road);
            _bsList = new List<BonusesSpawner>();
        }
        /// <summary>
        /// передаёт предзагруженные объекты и спауних их в реальные
        /// </summary>
        /// <param name="LoadedList"></param>
        public void OnGameLoad(List<GameObject> LoadedList)
        {
            _roadList.Clear();
            _bsList.Clear();
            foreach(GameObject LoadedRoad in LoadedList)
            {
                GameObject SpawnedLoadedRoad = _spawner.SpawnLoadedObject(LoadedRoad);
                if(SpawnedLoadedRoad != null)
                {
                    _roadList.Add(SpawnedLoadedRoad);
                    GameObject.Destroy(LoadedRoad);
                }
                else
                {
                    Debug.LogWarning("_roadList is empty!");
                }
            }
        }

        private void SaveFile()
        {
            SetBonuses?.Invoke(_bsList);
            SetRoads?.Invoke(_roadList);
            Save?.Invoke();
        }

        public void Update(int ScrollSpeed)
        {
            foreach(GameObject Road in _roadList)
            {
                Road.transform.Translate(-Vector3.forward * ScrollSpeed * Time.deltaTime); 
            }
            if(_bsList.Count > 0)
            {
                foreach (BonusesSpawner bs in _bsList)
                {
                    bs.Update(ScrollSpeed);
                }

            }

        }
    }
}