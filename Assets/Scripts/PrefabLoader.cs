using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Infinite_story
{
    public class PrefabLoader : MonoBehaviour
    {
        List<GameObject> PreloadedPrefabs;
        private PreloadPrefabs _preloadPrefabs;

        // Start is called before the first frame update
        void Start()
        {

            _preloadPrefabs = new PreloadPrefabs();
            List<GameObject> PrespawnedGoodBonuses = _preloadPrefabs.LoadPrefab(GoodBonuses);

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
