using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Infinite_story
{
    public class RoadController : IInit, IClear
    {
        private RoadData _roadData;
        private List<GameObject> _roadsList;
        private int _scrollSpeed;
        private int _roadCounter = 0;
        /// <summary>
        /// Объектовый пул, но вместо включения-выключения переставляем сегменты дороги по мере приближения игрока.
        /// </summary>
        public void Init()
        {
            _roadsList = new List<GameObject>();
            _roadData = Resources.Load<RoadData>("GameData/RoadData");
            if (_roadData == null) throw new FileNotFoundException($"Road Data not found");
            for(int i = 0; i < _roadData.PoolSize; i++)
            {
                GameObject Road = GameObject.Instantiate(_roadData.RoadPrefab, new Vector3(
                _roadData.SpawnPosition.x,
                _roadData.SpawnPosition.y,
                _roadData.SpawnPosition.z), Quaternion.identity);
                _roadsList.Add(Road);
                Road.GetComponentInChildren<SpawnColliderObserver>().OnTriggerColliderEnter += SpawnNewRoad;
                Road.SetActive(false);
            }
            _roadsList[_roadCounter].SetActive(true);
            _roadCounter++;
            _scrollSpeed = _roadData.ScrollSpeed;
        }

        private void SpawnNewRoad()
        {
            _roadsList[_roadCounter].transform.position = new Vector3(
                _roadsList[_roadCounter].transform.position.x,
                _roadsList[_roadCounter].transform.position.y,
                _roadsList[_roadCounter].transform.position.z + _roadsList[_roadCounter].transform.localScale.z
                );
            
            if (!_roadsList[_roadCounter].activeInHierarchy)
            {
                _roadsList[_roadCounter].SetActive(true);
            }
            
            _roadCounter++;
            if (_roadCounter >= _roadsList.Count) _roadCounter = 0;
          
        }


        public void Clear()
        {
            foreach(GameObject go in _roadsList)
            {
                go.GetComponentInChildren<SpawnColliderObserver>().OnTriggerColliderEnter -= SpawnNewRoad;
            }
        }

        public void Update()
        {
            foreach(GameObject road in _roadsList)
            {
                road.transform.Translate(-Vector3.forward * _scrollSpeed * Time.deltaTime);
            }
        }

    }
}