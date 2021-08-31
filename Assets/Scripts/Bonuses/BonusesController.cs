using System.Collections.Generic;
using UnityEngine;

namespace Infinite_story
{
    public class BonusesController : IInit, IScriptUpdate
    {
        private List<GameObject> _roadsList;
        public List<GameObject> Roads
        {
            get
            {
                return _roadsList;
            }
            set
            {
                _roadsList = value;
            }
        }
        
        public void Init()
        {
            Debug.Log(_roadsList.Count);
        }

        public void ScriptUpdate()
        {

        }
    }
}