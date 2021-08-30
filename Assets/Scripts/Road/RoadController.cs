using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Infinite_story
{
    public class RoadController : IInit
    {
        private RoadData _roadData;
        private List<GameObject> _roadsList;
        public void Init()
        {
            _roadsList = new List<GameObject>();
            _roadData = Resources.Load<RoadData>(@"GameData/RoadData");
            
            if (_roadData == null) throw new FileNotFoundException($"Road Data not found");
            GameObject Road = GameObject.Instantiate(_roadData.RoadPrefab, new Vector3(
                _roadData.SpawnPosition.x, 
                _roadData.SpawnPosition.y, 
                _roadData.SpawnPosition.z), Quaternion.identity);
            _roadsList.Add(Road);
        }
        public void Update()
        {
            foreach(GameObject road in _roadsList)
            {
                road.transform.Translate(-Vector3.forward * _roadData.ScrollSpeed * Time.deltaTime);
            }
        }

    }
}