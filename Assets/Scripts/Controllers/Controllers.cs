using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Infinite_story
{
    internal sealed class Controllers : IInit, IFixUpdate, IScriptUpdate, IClear
    {
        private readonly List<IInit> _initControllers;
        private readonly List<IFixUpdate> _fixUpdateControllers;
        private readonly List<IScriptUpdate> _scriptUpdateControllers;
        private readonly List<IClear> _clearControllers;

        internal Controllers()
        {
            _initControllers = new List<IInit>();
            _fixUpdateControllers = new List<IFixUpdate>();
            _scriptUpdateControllers = new List<IScriptUpdate>();
            _clearControllers = new List<IClear>();
        }

        internal Controllers Add(IController controller)
        {
            if(controller is IInit initcontroller)
            {
                _initControllers.Add(initcontroller);
            }
            if(controller is IFixUpdate fixupdate)
            {
                _fixUpdateControllers.Add(fixupdate);
            }
            if(controller is IScriptUpdate scriptupdate)
            {
                _scriptUpdateControllers.Add(scriptupdate);
            }
            if(controller is IClear clearcontroller)
            {
                _clearControllers.Add(clearcontroller);
            }
            return this;
        }
        
        public void Init()
        {
            for(int i = 0; i < _initControllers.Count; i++)
            {
                _initControllers[i].Init();
            }
        }
        public void FixUpdate()
        {
            for(int i = 0; i < _fixUpdateControllers.Count; i++)
            {
                _fixUpdateControllers[i].FixUpdate();
            }
        }
        public void ScriptUpdate()
        {
            for(int i = 0; i < _scriptUpdateControllers.Count; i++)
            {
                _scriptUpdateControllers[i].ScriptUpdate();
            }
        }
        public void Clear()
        {
            for(int i = 0; i < _clearControllers.Count; i++)
            {
                _clearControllers[i].Clear();
            }
        }
    }
}

