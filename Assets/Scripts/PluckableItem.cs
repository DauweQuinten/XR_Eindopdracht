using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PluckableItem : MonoBehaviour
{
    Rigidbody itemRb = null;
    bool isPlucked = false;


    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out itemRb))
        {
            Debug.LogError($"Pluckable item {gameObject.name} has no rigidbody component");
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        itemRb.isKinematic = true;
    }

    private void Update()
    {
        if (isPlucked)
        {
            itemRb.isKinematic = false;
            gameObject.transform.SetParent(null);
        }

    }

    public void Pluck()
    {
        Debug.Log("Object plucked");
        isPlucked=true;     
    }
}
