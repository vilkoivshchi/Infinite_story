using UnityEngine;

namespace Infinite_story
{

    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private GameData _gameData;
        private Controllers _controllers;
        
        private void Start()
        {
            _controllers = new Controllers();
            RoadData roadData = null;
            for(int i = 0; i < _gameData.GameDataList.Count; i++)
            {
                if(_gameData.GameDataList[i] is RoadData rd)
                {
                    roadData = rd;
                }
            }

            RoadController roadCtl = new RoadController(roadData);
            _controllers.Add(roadCtl);
            _controllers.Add(new PlayerController());
            _controllers.Add(new BonusesController(roadCtl));
            _controllers.Init();
        }

        private void OnDestroy()
        {
            _controllers.Clear();
        }

        private void FixedUpdate()
        {
            _controllers.FixUpdate();
        }

        private void Update()
        {
            _controllers.ScriptUpdate();
        }
    }
}