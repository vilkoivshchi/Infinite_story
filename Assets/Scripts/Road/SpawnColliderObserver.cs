using UnityEngine;
using System;

namespace Infinite_story
{
    public class SpawnColliderObserver : MonoBehaviour, ITriggerColliderEnter<Vector3>
    {
        public event Action<Vector3> OnTriggerColliderEnter = delegate(Vector3 v) { };
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                    OnTriggerColliderEnter?.Invoke(transform.parent.position);
                Debug.Log($"{transform.parent.name} was send action");
            }
        }
    }
}