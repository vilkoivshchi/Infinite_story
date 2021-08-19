using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Infinite_story
{
    public class PreloadPrefabs
    {
        private string PrefabPath = @"Prefabs/";
        
        public PreloadPrefabs()
        {

        }

        
        public List<GameObject> LoadPrefab(List<GameObject> BonusesList, GameObject Parent = null)
        {
            if (BonusesList.Count > 0)
            {
                List<GameObject> Prefabs = new List<GameObject>();
                foreach (GameObject go in BonusesList)
                {
                    //Debug.Log(go.name);
                    string PrefabName = PrefabPath + go.name;
                    
                    GameObject PreloadedPrefab = (GameObject)Resources.Load(PrefabName);
                    if (PreloadedPrefab == null)
                    {
                        Debug.LogError($"{PrefabName} is null!");
                    }
                    else
                    {
                        Prefabs.Add(PreloadedPrefab);
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
