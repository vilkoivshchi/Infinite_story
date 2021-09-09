using System.Collections.Generic;
using UnityEngine;

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
                GameObject RootBonusObject = new GameObject("Root Bonuses Object");
                RootBonusObject.transform.position = _roadsList[i].transform.position;
                _rootBonusObjects.Add(RootBonusObject);
                
            }
            _scrollSpeed = _roadController.RoadsData.ScrollSpeed;
            _roadController.SetRoadIndex += OnSetRoadIndex;
            BonusFactory BonusesFactory = new BonusFactory();
            for (int i = 0; i < _bonusesData.GoodBonusesQuantityMax; i++)
            {
                Quaternion bonusRotatation = Quaternion.AngleAxis(90, Vector3.right);
                for(int j = 0; j < _bonusesData.GoodBonuses.Count; j++)
                {
                    BonusesFactory.CreateBonus(_bonusesData.GoodBonuses[j], Vector3.zero, bonusRotatation,
                 _rootBonusObjects[_nextRoadIndex]);
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
            // это потом вынести в ScriptableObject. Или нет.
            
            int BadBonusesQuantity = Random.Range(_bonusesData.BadBonusesQuantityMin, _bonusesData.BadBonusesQuantityMax);
            int ModificatorsQuantity = Random.Range(_bonusesData.PlayerModificatorsQuantityMin, _bonusesData.PlayerModificatorsQuantityMax);
            int TrapsQuantity = Random.Range(_bonusesData.TrapsQuantityMin, _bonusesData.TrapsQuantityMax);

            
            int firstBonus = Random.Range(0, _bonusesData.GridSizeX);
            ClearBonuses(_rootBonusObjects[_nextRoadIndex]);
        
            /*
            for (int j = 0; j < GoodBonusesQuantity; j++)
            {
                    int spawnIndex = Random.Range(0, _spawnCoords.Count);
                    
                    Vector3 bonusPosition = _spawnCoords[spawnIndex];
                    Quaternion bonusRotatation = Quaternion.AngleAxis(90, Vector3.right);
                    goodBonusFactory.CreateBonus(_bonusesData.GoodBonuses[Random.Range(0, _bonusesData.GoodBonuses.Count)],
                        bonusPosition,
                        bonusRotatation,
                        _rootBonusObjects[_nextRoadIndex]);
                    _spawnCoords.Remove(_spawnCoords[spawnIndex]);
            }
            */

        }

        public void ClearBonuses(GameObject parent)
        {
            if(parent.transform.childCount > 0)
            {
                for(int i = 0; i < parent.transform.childCount; i++)
                {
                    parent.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        public void ScriptUpdate()
        {
            for(int i = 0; i < _rootBonusObjects.Count; i++)
            {
                _rootBonusObjects[i].transform.Translate(-Vector3.forward * _scrollSpeed * Time.deltaTime);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _roadsList.Count; i++)
            {
                _roadsList[i].GetComponentInChildren<SpawnColliderObserver>().OnTriggerColliderEnter -= SpawnBonuses;
            }
            _roadController.SetRoadIndex -= OnSetRoadIndex;
        }
    }
}