using UnityEngine;

namespace Infinite_story
{
    [CreateAssetMenu(fileName = "New Player Data", menuName = "Player Data", order = 53)]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private Vector3 _spawnCoordinates;
        [SerializeField] private int _sensivity;
        [SerializeField] private int _jumpForce;

        public GameObject Player
        {
            get
            {
                return _player;
            }
        }

        public Vector3 SpawnCoordinates
        {
            get
            {
                return _spawnCoordinates;
            }
        }

        public int Sensivity
        {
            get
            {
                return _sensivity;
            }
        }

        public int JumpForce
        {
            get
            {
                return _jumpForce;
            }
        }
    }
}