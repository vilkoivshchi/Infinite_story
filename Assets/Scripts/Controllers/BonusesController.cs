using System.Collections.Generic;
using UnityEngine;

namespace Infinite_story
{
    public class BonusesController : IInit, IScriptUpdate, IController
    {
        private List<GameObject> _roadsList;
        //private RoadController RoadCtl;
        public BonusesController(RoadController roadctl)
        {
            _roadsList = roadctl.RoadsList;
        }

        public void Init()
        {
            
        }

        public void ScriptUpdate()
        {

        }
    }
}