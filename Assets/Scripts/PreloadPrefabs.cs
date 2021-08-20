using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Infinite_story
{
    public class PreloadPrefabs
    {
        //private readonly string _prefabPath = @"Prefabs/";
        private readonly string _prefabPath;
        
        public PreloadPrefabs(string path)
        {
            _prefabPath = path;
        }

        
        public Dictionary<int, GameObject> LoadPrefab(List<GameObject> BonusesList)
        {
            if (BonusesList.Count > 0)
            {
                Dictionary<int, GameObject> Prefabs = new Dictionary<int, GameObject>();
                foreach (GameObject go in BonusesList)
                {
                    //Debug.Log(go.name);
                    string PrefabName = go.name;
                    string PrefabPath = _prefabPath + "/" + PrefabName;
                    int PrefabNameHash = PrefabName.GetHashCode();
                    
                    GameObject PreloadedPrefab = (GameObject)Resources.Load(PrefabPath);
                    
                    if (PreloadedPrefab == null)
                    {
                        Debug.LogError($"{PrefabPath} is null!");
                    }
                    else
                    {
                        Prefabs.Add(PrefabNameHash, PreloadedPrefab);
                    }
                }
                return Prefabs;
            }
            else
            {
                return null;
            }
        }

    }

}
