using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject pet;
    [SerializeField] private GameObject tableTarget;
    [SerializeField] private GameObject teleportationTarget;
    [SerializeField] private Outline letterOutline;

    private MoveToTarget petMover = null;
    private float defaultMoverOffset = 0;

    private int currentStepIndex = 1;
    private bool tutorialCompleted = false;


    private void Awake()
    {
        if (!pet.TryGetComponent<MoveToTarget>(out petMover))
        {
            Debug.LogError("pet has no MoveToTarget component");
        }
        defaultMoverOffset = petMover.GetOffset();
    }

    private void Update()
    {                  
        if (tutorialCompleted)
        {
            // Player can do everything he wants
            if (petMover)
            {
                // Follow the player if Anky has no other target         
                if(petMover.GetTarget() == null)
                {
                    StartCoroutine(StartFollowingPlayerWithDelay());
                }
            }
        }
        else
        {
            // Follow the steps of the tutorial
            switch (currentStepIndex)
            {
                case 1:
                    // Step1: Move to the letter
                    tableTarget.SetActive(true);   
                    break;
                case 2:
                    // Step2: Read the letter
                    tableTarget.SetActive(false);
                    letterOutline.enabled = true;
                    break;
                case 3:
                    // Step3: Move to the teleportation target
                    letterOutline.enabled = false;
                    teleportationTarget.SetActive(true);
                    break;
                case 4:             
                    // Step4: Pet Anky
                    teleportationTarget.SetActive(false);
                    break;

            }
        }
        
    }

    IEnumerator StartFollowingPlayerWithDelay(float delay = 2f)
    {
        yield return new WaitForSeconds(delay);
        petMover.SetTarget(xrOrigin);
        petMover.SetOffset(defaultMoverOffset);
        petMover.StartWalking();
    }

    public void GoToNextStep(int currentStep)
    {      
        // Check if the step to set is indeed the next step in line
        currentStep++;
        if(currentStep == currentStepIndex + 1)
        {
            currentStepIndex++;
        }
    }
}
