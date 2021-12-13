using UnityEngine;
using System;

namespace Infinite_story
{
    public class EndGameColliiderObserver : MonoBehaviour, ITriggerColliderExit
    {
        public event Action OnTriggerColliderExit;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnTriggerColliderExit?.Invoke();
            }
        }
    }

}
