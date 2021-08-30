using System.IO;
using UnityEngine;

namespace Infinite_story
{
    public class PlayerController : IInit, IFixUpdate, IScriptUpdate
    {
        private GameObject _player;
        private PlayerData _playerData;
        private Rigidbody _playerRigidBody;
        private SphereCollider _playerCollider;
        private float _deltaX;

        public void Init()
        {
            _playerData = Resources.Load<PlayerData>("GameData/PlayerData");
            if (_playerData == null) throw new FileNotFoundException($"Player Data not found");
            _player = GameObject.Instantiate(_playerData.Player, _playerData.SpawnCoordinates, Quaternion.identity);
            _playerRigidBody = _player.GetComponent<Rigidbody>();
            _playerCollider = _player.GetComponent<SphereCollider>();
        }

        public void FixUpdate()
        {
            _playerRigidBody.AddTorque(Vector3.right * 20);
        }

        public void ScriptUpdate()
        {
            
            // определяем, приземлён ли игрок
            bool hitGround = false;
            if (Physics.Raycast(_player.transform.position, Vector3.down, out RaycastHit hit))
            {
                float check = _playerCollider.radius + 0.1f;
                hitGround = hit.distance <= check;  // to be sure check slightly beyond bottom of capsule
            }
            // контроль игрока
            _deltaX = Input.GetAxis("Horizontal");
            if (_deltaX != 0)
            {
                _playerRigidBody.AddForce(new Vector3(_deltaX * _playerData.Sensivity, 0, 0));
            }
            if (Input.GetButtonDown("Jump") && hitGround)
            {
                _playerRigidBody.AddForce(new Vector3(0, _playerData.JumpForce, 0), ForceMode.Impulse);
            }
            
        }
    }
}