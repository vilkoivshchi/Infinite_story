using UnityEngine;
using System;

namespace Infinite_story
{
    public class SpawnColliderObserver : MonoBehaviour, ITriggerColliderEnter
    {
        public event Action OnTriggerColliderEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnTriggerColliderEnter?.Invoke();
            }
        }
    }
}