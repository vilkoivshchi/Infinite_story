using UnityEngine;

namespace Infinite_story
{

    public class GameStarter : MonoBehaviour
    {
        private RoadController _roadCtl;
        private PlayerController _playerCtl;
        private void Start()
        {
            _roadCtl = new RoadController();
            _playerCtl = new PlayerController();
            _roadCtl.Init();
            _playerCtl.Init();
        }

        private void OnDestroy()
        {
            _roadCtl.Clear();
        }

        private void FixedUpdate()
        {
            _playerCtl.FixUpdate();
        }

        private void Update()
        {
            _roadCtl.ScriptUpdate();
            _playerCtl.ScriptUpdate();
        }
    }
}