using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Grabber : MonoBehaviour
{
    GameObject attachedObject = null;
    Rigidbody  objectRb = null;
 
    public void Attach(GameObject grabbedObject)
    {
        if (attachedObject == null)
        {
            attachedObject = grabbedObject;

            if (!attachedObject.TryGetComponent<Rigidbody>(out objectRb))
            {
                Debug.LogError($"grabbed object {attachedObject.name} has no rigidbody component");
                return;
            }
       
            objectRb.isKinematic = true;
            attachedObject.transform.SetParent(gameObject.transform, true);
            attachedObject.transform.rotation = transform.rotation;
            attachedObject.transform.position = transform.position;
        }
    }

    public void Detach()
    {
        if (attachedObject)
        {
            objectRb.isKinematic = false;
            attachedObject.transform.SetParent(null);
            attachedObject = null;
        }
    }
}
