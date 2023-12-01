using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FoodEater : MonoBehaviour
{
    EdibleItem foodInRange = null;
    float timeSinceFoodEntered = 0f;
    [SerializeField]float eatDelay = 2f;
    [SerializeField] AudioSource EatSound;
    [SerializeField] UnityEvent onFoodEaten;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Edible") && foodInRange == null)
        {
            other.TryGetComponent<EdibleItem>(out foodInRange);
            timeSinceFoodEntered = 0f;
        }
    }

    private void OnTriggerStay(Collider other)
    {            
        if (foodInRange && other.name == foodInRange.gameObject.name)
        {
            timeSinceFoodEntered += Time.deltaTime;
            if(timeSinceFoodEntered > eatDelay)
            {
                foodInRange = null;
                Eat(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foodInRange = null;
    }

    private void Eat(GameObject food)
    {      
        EatSound.Play();
        Destroy(food);
        onFoodEaten.Invoke();
    }
}
