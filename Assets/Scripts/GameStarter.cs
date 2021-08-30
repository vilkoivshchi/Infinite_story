using UnityEngine;

namespace Infinite_story
{

    public class GameStarter : MonoBehaviour
    {
        private RoadController _roadctl;
        void Start()
        {
            _roadctl = new RoadController();
            _roadctl.Init();
        }

        void OnDestroy()
        {
            _roadctl.Clear();
        }

        void Update()
        {
            _roadctl.Update();
        }
    }
}