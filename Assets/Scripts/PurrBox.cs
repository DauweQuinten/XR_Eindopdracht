using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurrBox : MonoBehaviour
{
    Vector3 PrevHandPosition = Vector3.zero;
    float minimalPurrDistance = 0.01f;
    [SerializeField]AudioSource purrAudio;
    bool isPetting = false;
    bool isPurring = false;
    float timeSinceLastPed = 0f;

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
            if(timeSinceLastPed > 2f)
            {
                isPurring = false;
            }
        }
        
        
        if (isPurring)
        {
            if (!purrAudio.isPlaying)
            {
                purrAudio.Play();
            }         
        }
        else
        {
            purrAudio.Stop();
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Controller"))
        {
            Debug.Log("Hand entered the trigger");
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