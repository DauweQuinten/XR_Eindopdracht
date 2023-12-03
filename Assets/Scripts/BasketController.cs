using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasketController : MonoBehaviour
{
    bool playerIsInPlace = false;
    [SerializeField] UnityEvent onGoal;


    public void SetPlayerInPlace(bool isInPlace)
    {
        playerIsInPlace = isInPlace;
    }

    public void Score()
    {
        if(playerIsInPlace)
        {
            onGoal.Invoke();
        }
    }
}
