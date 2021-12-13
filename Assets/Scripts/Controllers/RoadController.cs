using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Infinite_story
{
    public class RoadController : IInit, IScriptUpdate, IClear, IController
    {
        public List<GameObject> RoadsList
        {
            get
            {
                return _roadsList;
            }
        }

        public RoadData RoadsData
        {
            get
            {
                return _roadData;
            }
        }
        public event Action<int> SetRoadIndex = delegate (int s) { };
        private RoadData _roadData;
        private List<GameObject> _roadsList;
        private int _scrollSpeed;
        private int _roadCounter = 0;

        public RoadController()
        {

        }

        public RoadController(RoadData roaddata)
        {
            _roadData = roaddata;
        }

        
        public void Init()
        {
            _roadsList = new List<GameObject>();
            if(_roadData == null)
            {
                throw new FileNotFoundException($"Road Data not found");
            }
            
            for(int i = 0; i < _roadData.PoolSize; i++)
            {
                GameObject Road = GameObject.Instantiate(_roadData.RoadPrefab, _roadData.SpawnPosition, Quaternion.identity);
                Road.name = $"Road{i}";
                _roadsList.Add(Road);
                Road.GetComponentInChildren<SpawnColliderObserver>().OnTriggerColliderEnter += SpawnNewRoad;
                Road.SetActive(false);
            }
            _roadsList[_roadCounter].SetActive(true);
            _roadCounter++;
            SetRoadIndex?.Invoke(_roadCounter);
            _scrollSpeed = _roadData.ScrollSpeed;
        }

        private void SpawnNewRoad(Vector3 pos)
        {
            
            _roadsList[_roadCounter].transform.position = new Vector3(pos.x, pos.y, pos.z + _roadData.RoadPrefab.transform.localScale.z);
            if (!_roadsList[_roadCounter].activeInHierarchy)
            {
                _roadsList[_roadCounter].SetActive(true);
            }
            _roadCounter++;
            if (_roadCounter >= _roadsList.Count) _roadCounter = 0;
            SetRoadIndex?.Invoke(_roadCounter);
        }

        //~RoadController() 
        public void Clear()
        {
            
            foreach(GameObject go in _roadsList)
            {
                if (go)
                {
                    for (int i = 0; i < go.transform.childCount; i++)
                    {
                        SpawnColliderObserver colliderObserver;
                        go.transform.GetChild(i).TryGetComponent(out colliderObserver);
                        if (colliderObserver != null)
                        {
                            colliderObserver.OnTriggerColliderEnter -= SpawnNewRoad;
                        }
                    }
                }
            }
        }

        public void ScriptUpdate()
        {
            foreach(GameObject road in _roadsList)
            {
                road.transform.Translate(-Vector3.forward * _scrollSpeed * Time.deltaTime);
            }
        }

    }
}