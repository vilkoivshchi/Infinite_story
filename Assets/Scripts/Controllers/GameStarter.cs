using System;
using System.Collections.Generic;
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
            PlayerData playerData = null;
            BonusesData bonusesData = null;
            for (int i = 0; i < _gameData.GameDataList.Count; i++)
            {
                if(_gameData.GameDataList[i] is RoadData rd)
                {
                    roadData = rd;
                }
                if (_gameData.GameDataList[i] is PlayerData pd)
                {
                    playerData = pd;
                }
                if(_gameData.GameDataList[i] is BonusesData bd)
                {
                    bonusesData = bd;
                }
            }

            RoadController roadCtl = new RoadController(roadData);
            _controllers.Add(roadCtl);
            _controllers.Add(new PlayerController(playerData));
            _controllers.Add(new BonusesController(roadCtl, bonusesData));
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