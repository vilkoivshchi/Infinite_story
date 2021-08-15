using System;
using UnityEngine;

public class DisableForceAtStack : MonoBehaviour
{
    public static Action DisableForce;
    public static Action EnableForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DisableForce?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnableForce?.Invoke();
        }
    }
}
