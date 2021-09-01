using System.Collections.Generic;
using UnityEngine;

namespace Infinite_story
{
    public class BonusesController : IInit, IScriptUpdate, IClear, IController
    {
        private List<GameObject> _roadsList;
        private BonusesData _bonusesData;
        private RoadController _roadController;
        private int _scrollSpeed;

        public BonusesController(RoadController roadctl, BonusesData bonusesData)
        {
            _bonusesData = bonusesData;
            _roadController = roadctl;
        }

        public void Init()
        {
            _roadsList = _roadController.RoadsList;
            for(int i = 0; i < _roadsList.Count; i++)
            {
                _roadsList[i].GetComponentInChildren<SpawnColliderObserver>().OnTriggerColliderEnter += SpawnBonuses;
            }
            _roadsList = new List<GameObject>();
            for(int i = 0; i < _roadController.RoadsData.PoolSize; i++)
            {
                _roadsList.Add(new GameObject());
            }
            _scrollSpeed = _roadController.RoadsData.ScrollSpeed;
        }

        private void SpawnBonuses(Vector3 pos)
        {
            //Debug.Log($"pos: {pos}");
        }

        public void ScriptUpdate()
        {

        }

        public void Clear()
        {
            for (int i = 0; i < _roadsList.Count; i++)
            {
                _roadsList[i].GetComponentInChildren<SpawnColliderObserver>().OnTriggerColliderEnter -= SpawnBonuses;
            }
        }
    }
}