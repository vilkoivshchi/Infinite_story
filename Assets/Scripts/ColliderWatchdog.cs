using System;
using UnityEngine;

public class ColliderWatchdog : MonoBehaviour
{
    public static Action SpawnColliderHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnColliderHit?.Invoke();
        }
    }
}
