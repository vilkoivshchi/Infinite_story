using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace Infinite_story
{
    
    public class Spawner
    {
        public GameObject RoadPrefab;
        private string RoadPrefabPath = "Assets/Resources/Prefabs/Road.prefab";
        private string RoadPrefabShortPath = "Prefabs/Road";

        private string PrefabFolderName = "Prefabs/";
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

        public GameObject SpawnLoadedObject(GameObject LoadedBlank)
        {
            string LoadedPrefabName = Regex.Replace(LoadedBlank.name, @"\(.+\)", String.Empty);
            
            if(LoadedPrefabName != string.Empty)
            {
                LoadedPrefabName = PrefabFolderName + LoadedPrefabName;
                Debug.Log($"LoadedPrefabName: {LoadedPrefabName}");
                GameObject LoadedPrefab = (GameObject)Resources.Load(LoadedPrefabName);
                if (LoadedPrefab != null)
                {
                    GameObject LoadedSpawnedRoad = GameObject.Instantiate(LoadedPrefab, new Vector3(
                        LoadedBlank.transform.position.x,
                        LoadedBlank.transform.position.y,
                        LoadedBlank.transform.position.z), Quaternion.identity);
                    return LoadedSpawnedRoad;
                }
                else
                {
                    Debug.LogError($"{LoadedPrefab} is null!");
                    return null;
                }
                //GameObject SpawnedRoad = GameObject.Instantiate()
            }
            else
            {
                Debug.LogError("Can't resolve prefab name!");
                return null;
            }
        }

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