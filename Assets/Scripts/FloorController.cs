using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infinite_story
{
    public class FloorController
    {
        public GameObject SpawnedRoad;

        // события для сохраниеня 
        public static Action<List<GameObject>> SetRoads;
        public static Action<List<BonusesSpawner>> SetBonuses;
        public static Action Save;

        public int ScrollSpeed;

        private List<GameObject> _roadList;
        private Spawner _spawner;
        //private int SpawnCounter = 0;
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

        private string _roadTag, _bonusesTag;

        
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
            PlayerController.SetBonusesTag += ReadBonusesTag;
            PlayerController.SetRoadTag += ReadRoadTag;
            LoadGame.OnSaveFileReaded += OnGameLoaded;
        }

        public void OnDestroy()
        {
            ColliderWatchdog.SpawnColliderHit -= SpawnRoad;
            UIController.SaveFileEvent -= SaveFile;
            _bonusesSpawner?.OnDestroy();
            PlayerController.SetBonusesTag -= ReadBonusesTag;
            PlayerController.SetRoadTag -= ReadRoadTag;
            LoadGame.OnSaveFileReaded -= OnGameLoaded;
        }

        private void ReadRoadTag(string RoadTag)
        {
            _roadTag = RoadTag;
        }

        private void ReadBonusesTag(string BonusesTag)
        {
            _bonusesTag = BonusesTag;
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

        private void SeekAndDestroy(string Tag)
        {
            GameObject[] FoundedObj = GameObject.FindGameObjectsWithTag(Tag);
            if(FoundedObj.Length > 0)
            {
                foreach(GameObject obj in FoundedObj)
                {
                    GameObject.Destroy(obj);
                }
            }
        }
        /// <summary>
        /// Get preloaded objects, spawning it and place to define list
        /// </summary>
        /// <param name="SourceList"></param>
        /// <param name="DestList"></param>
        private void SpawnLoadedObject(List<GameObject> SourceList, List<GameObject> DestList)
        {
            foreach (GameObject LoadedObj in SourceList)
            {
                if(LoadedObj.transform.childCount == 0)
                {
                    GameObject SpawnedLoadedRoad = _spawner.SpawnLoadedObject(LoadedObj);
                    if (SpawnedLoadedRoad != null)
                    {
                        DestList.Add(SpawnedLoadedRoad);
                        GameObject.Destroy(LoadedObj);
                    }
                    else
                    {
                        Debug.LogWarning($"DestList is empty!");
                    }
                }
                else
                {
                    GameObject NewParent = new GameObject();
                    NewParent.transform.position = LoadedObj.transform.position;
                    NewParent.tag = LoadedObj.tag;
                    // тут костыль
                    for(int i=0; i < LoadedObj.transform.childCount; i++)
                    {
                        Transform LoadedObjChildTranform = LoadedObj.transform.GetChild(i);
                        GameObject SpawnedLoadedBonus = _spawner.SpawnLoadedObject(LoadedObjChildTranform.gameObject, NewParent);
                        //GameObject.Destroy(LoadedObjChildTranform.gameObject);
                        //SpawnedLoadedBonus.transform.SetParent(NewParent.transform, true);
                        SpawnedLoadedBonus.transform.localEulerAngles = LoadedObjChildTranform.transform.localEulerAngles;
                        
                    }
                    GameObject.Destroy(LoadedObj);
                    DestList.Add(NewParent);
                }
                

            }
        }
        /// <summary>
        /// передаёт предзагруженные объекты и спауних их в реальные
        /// </summary>
        /// <param name="LoadedList"></param>
        public void OnGameLoaded(List<GameObject> LoadedList)
        {
            List<GameObject> LoadedRoadsList = new List<GameObject>();
            List<GameObject> LoadedBonusesList = new List<GameObject>();

            foreach(GameObject LoadedObj in LoadedList)
            {
                if (LoadedObj.CompareTag(_roadTag))
                {
                    LoadedRoadsList.Add(LoadedObj);
                    LoadedObj.SetActive(false);
                }
                else if (LoadedObj.CompareTag(_bonusesTag))
                {
                    LoadedBonusesList.Add(LoadedObj);
                    LoadedObj.SetActive(false);
                }
            }
            SeekAndDestroy(_roadTag);
            SeekAndDestroy(_bonusesTag);

            _roadList.Clear();
            _bsList.Clear();

            List<GameObject> SpawnedLoadedBonuses = new List<GameObject>();
            SpawnLoadedObject(LoadedRoadsList, _roadList);
            SpawnLoadedObject(LoadedBonusesList, SpawnedLoadedBonuses);
            foreach(GameObject SpawnedBonus in SpawnedLoadedBonuses)
            {
                _bonusesSpawner = new BonusesSpawner(SpawnedBonus);
                _bsList.Add(_bonusesSpawner);
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