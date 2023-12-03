using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTriggerCheckEvent : MonoBehaviour
{
    [SerializeField] bool shouldCheckOnTag;
    [SerializeField] string tagOfTriggableObject;

    [Header("Events")]
    [SerializeField] UnityEvent onTriggerEntered;
    [SerializeField] UnityEvent onTriggerStayed;
    [SerializeField] UnityEvent onTriggerExited;


    private void OnTriggerEnter(Collider other)
    {
        if (!shouldCheckOnTag)
        {
            onTriggerEntered.Invoke();
        }
        else if (other.CompareTag(tagOfTriggableObject))
        {
            onTriggerEntered.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!shouldCheckOnTag)
        {
            onTriggerStayed.Invoke();
        }
        else if (other.CompareTag(tagOfTriggableObject))
        {
            onTriggerStayed.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!shouldCheckOnTag)
        {
            onTriggerExited.Invoke();
        }
        if (other.CompareTag(tagOfTriggableObject))
        {
            onTriggerExited.Invoke();
        }
    }
}
