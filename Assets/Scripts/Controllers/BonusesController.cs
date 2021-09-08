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
        private int _currentRoadIndex;
        private int _coordArrayPointer;

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

        }

        private void OnSetRoadIndex(int index)
        {
            _currentRoadIndex = index;
        }

        private void SpawnBonuses(Vector3 pos)
        {
            BoxCollider roadbounds = _roadsList[_currentRoadIndex].GetComponent<BoxCollider>();
           
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

            BonusFactory goodBonusFactory = new BonusFactory();

            for (int j = GoodBonusesQuantity; j > 0; j--)
            {
                int GoodBonusesSequenceLenght = Random.Range(2, 6);
                _coordArrayPointer = firstBonus;
                for (int i = 0; i < GoodBonusesSequenceLenght; i++)
                {
                    //int spawnIndex = Random.Range(0, _spawnCoords.Count);
                    
                    Vector3 bonusPosition = _spawnCoords[_coordArrayPointer];
                    Quaternion bonusRotatation = Quaternion.AngleAxis(90, Vector3.right);
                    goodBonusFactory.CreateBonus(_bonusesData.GoodBonuses[Random.Range(0, _bonusesData.GoodBonuses.Count)],
                        bonusPosition,
                        bonusRotatation,
                        _rootBonusObjects[_currentRoadIndex]);
                    _spawnCoords.Remove(_spawnCoords[_coordArrayPointer]);
                    _coordArrayPointer += _bonusesData.GridSizeX;
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