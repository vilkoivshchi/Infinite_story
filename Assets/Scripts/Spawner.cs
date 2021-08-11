using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Infinite_story
{
    
    public class Spawner
    {
        public GameObject RoadPrefab;
        private string RoadPrefabPath = "Assets/Resources/Prefabs/Road.prefab";
        private string RoadPrefabShortPath = "Prefabs/Road";
/*
        private Vector3 SpawnPos;
  
        public Spawner(Vector3 Position)
        {
            SpawnPos = Position;
        }
*/
        public void Start()
        {
            if (File.Exists(RoadPrefabPath))
            {
                RoadPrefab = (GameObject)Resources.Load(RoadPrefabShortPath);
                if(RoadPrefab == null)
                {
                    Debug.LogError($"{RoadPrefabShortPath} is null!");
                }
            }
            else
            {
                Debug.LogError($"Не могу найти {RoadPrefabPath}");
            }
        }

        // событие нужно для модификаторов игрока
        
        
        // спавним следующий сегмент встык к текущему
        public GameObject SpawnNewRoad(Vector3 pos)
        {
            /*
            BoxCollider PrefabCollider;
            if(!RoadPrefab.TryGetComponent<BoxCollider>(out PrefabCollider))
            {
                Debug.Log("Can't get PrefabCollider!");
            }
            */
                GameObject Road = GameObject.Instantiate(RoadPrefab, new Vector3(pos.x, pos.y, pos.z + RoadPrefab.transform.localScale.z), Quaternion.identity);
            return Road;
        }
        
    }
}