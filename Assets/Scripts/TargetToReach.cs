using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetToReach : MonoBehaviour
{
    public UnityEvent targetReached;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Target entered
            targetReached.Invoke();
        }
    }
}
