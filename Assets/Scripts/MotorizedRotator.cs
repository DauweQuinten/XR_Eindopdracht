using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorizedRotator : MonoBehaviour
{
    Quaternion targetAngle;
    [SerializeField] float rotationSpeed = 10f;
    
    void Rotate()
    {     
        transform.Rotate(transform.up, 90);
    }

    //public void SetTargetAngle(float angle)
    //{
    //    targetAngle = 
    //}

    private void Update()
    {
        if(transform.rotation == targetAngle)
        {
            return;
        }

        Rotate();
    }
}
