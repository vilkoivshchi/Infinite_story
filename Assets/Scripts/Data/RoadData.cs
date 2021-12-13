using UnityEngine;

namespace Infinite_story
{


    [CreateAssetMenu(fileName = "New Road Data", menuName = "Road Data", order = 52)]
    public class RoadData : ScriptableObject
    {
        [SerializeField] private GameObject _roadPrefab;
        [SerializeField] private Vector3 _spawnPosition;
        [SerializeField] private int _poolSize;
        [SerializeField] private int _scrollSpeed;

        public GameObject RoadPrefab
        {
            get
            {
                return _roadPrefab;
            }
        }

        public Vector3 SpawnPosition
        {
            get
            {
                return _spawnPosition;
            }
        }

        public int ScrollSpeed
        {
            get
            {
                return _scrollSpeed;
            }
        }

        public int PoolSize
        {
            get
            {
                return _poolSize;
            }
        }

    }
}