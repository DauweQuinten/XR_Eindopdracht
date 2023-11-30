using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PurrBox : MonoBehaviour
{
    Vector3 PrevHandPosition = Vector3.zero;
    float minimalPurrDistance = 0.06f;
    [SerializeField]AudioSource purrAudio;
    bool isPetting = false;
    bool isPurring = false;
    float timeSinceLastPed = 0f;
    float stopPurringDelay = 2f;
    public UnityEvent onPurrStarted;

    private void Update()
    {
        if (isPetting)
        {           
            isPurring = true;
            timeSinceLastPed = 0;         
        }
        else
        {
            // If not petting for 2s -> Stop purring
            timeSinceLastPed += Time.deltaTime;
            if(timeSinceLastPed > stopPurringDelay)
            {
                isPurring = false;
            }
        }
        
        
        if (isPurring)
        {
            if (!purrAudio.isPlaying)
            {
                purrAudio.Play();
                onPurrStarted.Invoke();
            }         
        }
        else
        {
            purrAudio.Stop();
        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Controller"))
        {
            Vector3 currentHandPosition = other.transform.position;
            if(Vector3.Distance(currentHandPosition, PrevHandPosition) > minimalPurrDistance)
            {
                isPetting = true;
            }
            else
            {                             
                isPetting = false;
            }

            PrevHandPosition = currentHandPosition;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isPetting = false;
    }
}
