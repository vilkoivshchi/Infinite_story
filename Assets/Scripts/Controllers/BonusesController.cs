using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Infinite_story
{
    public class BonusesController : IInit, IScriptUpdate, IClear, IController
    {
        private List<GameObject> _roadsList;
        private List<GameObject> _rootBonusObjects;
        private BonusesData _bonusesData;
        private RoadController _roadController;
        private int _scrollSpeed;
        private int _nextRoadIndex;
        private Dictionary<int, GameObject> _bonusesPoolsList;
        private GameObject _goodBonusesPool;
        private GameObject _badBonusesPool;
        private GameObject _playerModificatorsPool;
        private GameObject _trapsPool;

        private float NormalTimeScale = 0;

        public BonusesController(RoadController roadctl, BonusesData bonusesData)
        {
            _bonusesData = bonusesData;
            _roadController = roadctl;
        }

        public void Init()
        {
            _roadsList = _roadController.RoadsList;
            _rootBonusObjects = new List<GameObject>();
            for (int i = 0; i < _roadsList.Count; i++)
            {
                _roadsList[i].GetComponentInChildren<SpawnColliderObserver>().OnTriggerColliderEnter += SpawnBonuses;
                GameObject RootBonusObject = new GameObject($"Root Bonuses Object");
                RootBonusObject.transform.position = _roadsList[i].transform.position;
                _rootBonusObjects.Add(RootBonusObject);
                
            }
            _scrollSpeed = _roadController.RoadsData.ScrollSpeed;

            _roadController.SetRoadIndex += OnSetRoadIndex;
            
            _bonusesPoolsList = new Dictionary<int, GameObject>();
            _goodBonusesPool = new GameObject("GoodBonuses Pool");
            _badBonusesPool = new GameObject("BadBonuses Pool");
            _playerModificatorsPool = new GameObject("Player Modificators pool");
            _trapsPool = new GameObject("Traps pool");
            _bonusesPoolsList.Add(_goodBonusesPool.name.GetHashCode(), _goodBonusesPool);
            _bonusesPoolsList.Add(_badBonusesPool.name.GetHashCode(), _badBonusesPool);
            _bonusesPoolsList.Add(_playerModificatorsPool.name.GetHashCode(), _playerModificatorsPool);
            _bonusesPoolsList.Add(_trapsPool.name.GetHashCode(), _trapsPool);
        
            BonusFactory BonusesFactory = new BonusFactory();
            InitBonuses(_bonusesData.GoodBonuses, _bonusesData.GoodBonusesQuantityMax * _roadsList.Capacity , Vector3.right, 90f, _goodBonusesPool, BonusesFactory);
            InitBonuses(_bonusesData.BadBonuses, _bonusesData.BadBonusesQuantityMax * _roadsList.Capacity, Vector3.right, -90f, _badBonusesPool, BonusesFactory);
            InitBonuses(_bonusesData.PlayerModificators, _bonusesData.PlayerModificatorsQuantityMax * _roadsList.Capacity, Vector3.right, 0, _playerModificatorsPool, BonusesFactory);
            InitBonuses(_bonusesData.Traps, _bonusesData.TrapsQuantityMax * _roadsList.Capacity, Vector3.right, -90, _trapsPool, BonusesFactory);
            NormalTimeScale = Time.timeScale;
        }

        private void InitBonuses(List<GameObject> prefabList, int maxQuamtity, Vector3 rotationAxis, float angle, GameObject parent, BonusFactory factory, bool randomRotation = false)
        {
            for (int i = 0; i < maxQuamtity; i++)
            {
                for (int j = 0; j < prefabList.Count; j++)
                {
                    GameObject bonus;
                    bonus = factory.CreateBonus(prefabList[j], Vector3.zero, Quaternion.AngleAxis(angle, rotationAxis), parent);
                    if (randomRotation)
                    {
                        bonus.transform.rotation = Quaternion.AngleAxis(Random.Range(0, angle), Vector3.up);
                    }
                    Ident bonusIdent = bonus.AddComponent<Ident>();
                    bonusIdent.BonusIdent = parent.name.GetHashCode();
                    BonusAction bonusAction;
                    bonus.TryGetComponent(out bonusAction);
                    if(bonusAction != null)
                    {
                        bonusAction.SetCaller += OnBonusGet;
                    }
                    bonus.SetActive(false);
                }

            }
        }

        private void OnSetRoadIndex(int index)
        {
            _nextRoadIndex = index;
        }

        private void SpawnBonuses(Vector3 pos)
        {
            BoxCollider roadbounds = _roadsList[_nextRoadIndex].GetComponent<BoxCollider>();

            Vector3 _spawnCoordsBeginPoint = new Vector3(
            roadbounds.bounds.center.x - roadbounds.bounds.extents.x,
            roadbounds.bounds.center.y + roadbounds.bounds.extents.y,
            roadbounds.bounds.center.z + roadbounds.bounds.extents.z
            );

            float pointsX = roadbounds.bounds.size.x / _bonusesData.GridSizeX;
            float pointsZ = roadbounds.bounds.size.z / _bonusesData.GridSizeZ;
            List<Vector3> _spawnCoords = new List<Vector3>();

            // разбиваем площадку на сетку, в которой будут респаунится бонусы
            for (int i = 0; i < _bonusesData.GridSizeX; i++)
            {
                for (int j = 0; j < _bonusesData.GridSizeZ; j++)
                {
                    // самая левая кромка поля нас не интересует
                    if (i > 0)
                    {
                        Vector3 currentPoint = new Vector3(
                            _spawnCoordsBeginPoint.x + pointsX * i,
                            _spawnCoordsBeginPoint.y,
                            _spawnCoordsBeginPoint.z + pointsZ * j);
                        _spawnCoords.Add(currentPoint);
                        //Debug.Log(currentPoint);
                    }
                }
            }

            int GoodBonusesQuantity = Random.Range(_bonusesData.GoodBonusesQuantityMin, _bonusesData.GoodBonusesQuantityMax);
            int BadBonusesQuantity = Random.Range(_bonusesData.BadBonusesQuantityMin, _bonusesData.BadBonusesQuantityMax);
            int ModificatorsQuantity = Random.Range(_bonusesData.PlayerModificatorsQuantityMin, _bonusesData.PlayerModificatorsQuantityMax);
            int TrapsQuantity = Random.Range(_bonusesData.TrapsQuantityMin, _bonusesData.TrapsQuantityMax);

            ReturnBonusesToPool(_rootBonusObjects[_nextRoadIndex]);
            Time.timeScale = 0;
            PlaceBonusesAtMap(_goodBonusesPool, GoodBonusesQuantity, _spawnCoords, _rootBonusObjects[_nextRoadIndex]);
            PlaceBonusesAtMap(_badBonusesPool, BadBonusesQuantity, _spawnCoords, _rootBonusObjects[_nextRoadIndex]);
            PlaceBonusesAtMap(_playerModificatorsPool, ModificatorsQuantity, _spawnCoords, _rootBonusObjects[_nextRoadIndex]);
            PlaceBonusesAtMap(_trapsPool, TrapsQuantity, _spawnCoords, _rootBonusObjects[_nextRoadIndex]);

        }

        private void OnBonusGet(GameObject sender)
        {
            foreach (KeyValuePair<int, GameObject> kvp in _bonusesPoolsList)
            {
                if (kvp.Key == sender.GetComponent<Ident>().BonusIdent)
                {
                    sender.transform.SetParent(kvp.Value.transform);
                }                
            }
            sender.SetActive(false);
        }

        private void PlaceBonusesAtMap(GameObject bonusesPool, int bonusesQantity, List<Vector3> spawnCoords, GameObject rootObject)
        {
            for (int i = 0; i < bonusesQantity; i++)
            {
                if (bonusesPool.transform.childCount > 0)
                {
                    GameObject newBonus = bonusesPool.transform.GetChild(Random.Range(0, bonusesPool.transform.childCount)).gameObject;
                    int newBonusPositionIndex = Random.Range(0, spawnCoords.Count);
                    Renderer renderer = newBonus.GetComponent<Renderer>();
                    Vector3 newBonusPosition = new Vector3(
                        spawnCoords[newBonusPositionIndex].x,
                        spawnCoords[newBonusPositionIndex].y + renderer.bounds.extents.y,
                        spawnCoords[newBonusPositionIndex].z);
                    spawnCoords.Remove(spawnCoords[newBonusPositionIndex]);
                    newBonus.transform.position = newBonusPosition;
                    newBonus.transform.SetParent(rootObject.transform, true);
                    newBonus.SetActive(true);
                }
                else Debug.LogWarning($"{bonusesPool.name} is empty!");
            }
        }

        private void ReturnBonusesToPool(GameObject parent)
        {
            int returnedBonuses = 0;
            Debug.Log($"Childs in parent: {parent.transform.childCount}");
                for(int i = 0; i < parent.transform.childCount; i++)
                {
                    GameObject bonus = parent.transform.GetChild(i).gameObject;
                    
                    bonus.transform.position = Vector3.zero;
                    int bonusId = bonus.GetComponent<Ident>().BonusIdent;
                    foreach(KeyValuePair<int, GameObject> kvp in _bonusesPoolsList)
                    {
                        
                        if (bonusId == kvp.Key)
                        {
                            bonus.transform.SetParent(kvp.Value.transform);
                        //Debug.Log($"{bonus.name} was returned to {kvp.Value.name}");
                        returnedBonuses++;
                        }
                    }
                if (bonus.transform.parent == parent) Debug.Log($"parent not changed");
                    bonus.SetActive(false);
                }
            Debug.Log($"Returned: {returnedBonuses}");
        }

        public void ScriptUpdate()
        {
            for(int i = 0; i < _rootBonusObjects.Count; i++)
            {
                _rootBonusObjects[i].transform.Translate(-Vector3.forward * _scrollSpeed * Time.deltaTime);
            }
            if (Input.GetKeyDown(KeyCode.P)) Time.timeScale = NormalTimeScale;
        }

        public void Clear()
        {
            for (int i = 0; i < _roadsList.Count; i++)
            {
                if (_roadsList[i])
                {
                    _roadsList[i].GetComponentInChildren<SpawnColliderObserver>().OnTriggerColliderEnter -= SpawnBonuses;
                }
            }
            _roadController.SetRoadIndex -= OnSetRoadIndex;


            foreach (KeyValuePair<int, GameObject> kvp in _bonusesPoolsList)
            {
                if (kvp.Value)
                {
                    for (int i = 0; i < kvp.Value.transform.childCount; i++)
                    {
                        BonusAction action;
                        kvp.Value.transform.GetChild(i).TryGetComponent(out action);
                        if (action != null)
                        {
                            action.SetCaller -= OnBonusGet;
                        }
                    }
                }
            }
        }
    }
}