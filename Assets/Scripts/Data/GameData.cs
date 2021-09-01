using System.Collections.Generic;
using UnityEngine;

namespace Infinite_story
{
    [CreateAssetMenu(fileName = "New Game Data", menuName = "Game Data", order = 54)]
    public class GameData : ScriptableObject
    {
        [SerializeField] List<ScriptableObject> _gameDataList;

        public List<ScriptableObject> GameDataList
        {
            get
            {
                return _gameDataList;
            }
        }
    }
}
