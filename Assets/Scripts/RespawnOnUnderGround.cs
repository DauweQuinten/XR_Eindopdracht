using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnUnderGround : MonoBehaviour
{
    Vector3 positionOnStart;
    Quaternion rotationOnStart;

    private void Awake()
    {
        positionOnStart = transform.position; 
        rotationOnStart = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0f)
        {
            StartCoroutine(RespawnAfterDelay());
        }      
    }

    IEnumerator RespawnAfterDelay(float delay=3f)
    {
        yield return new WaitForSeconds(delay);
        transform.position = positionOnStart;
        transform.rotation = rotationOnStart;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }
}
