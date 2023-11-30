using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onTutorialAnyStepCompleted;
    public UnityEvent onTutorialPart1Completed;
    public UnityEvent onTutorialPart2Completed;
    public UnityEvent onTutorialPart3Completed;
    public UnityEvent onTutorialPart4Completed;
    public UnityEvent onTutorialPart5Completed;
    public UnityEvent onTutorialCompleted;
    
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject pet;
    [SerializeField] private GameObject tableTarget;
    [SerializeField] private GameObject teleportationTarget;
    [SerializeField] private Outline letterOutline;
    [SerializeField] private HighlightControllerParts leftController;
    [SerializeField] private HighlightControllerParts rightController;
    [SerializeField] private AudioSource source;

    private MoveToTarget petMover = null;
    private float defaultMoverOffset = 0;

    private int currentStepIndex = 1;
    private int prevStepIndex = 1;
    private bool tutorialCompleted = false;


    private void Awake()
    {
        if (!pet.TryGetComponent<MoveToTarget>(out petMover))
        {
            Debug.LogError("pet has no MoveToTarget component");            
        }
        if(!TryGetComponent<AudioSource>(out source))
        {
            Debug.LogError("GameManager has no AudioSource component");
        }

        if (petMover)
        {
            defaultMoverOffset = petMover.GetOffset();
        }
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
                    leftController.Highlight(HighlightControllerParts.ControllerPart.ThumbStick);
                    break;
                case 2:
                    // Step2: Read the letter
                    tableTarget.SetActive(false);
                    letterOutline.enabled = true;
                    leftController.ResetHighLight(HighlightControllerParts.ControllerPart.ThumbStick);
                    leftController.Highlight(HighlightControllerParts.ControllerPart.Bumper);
                    rightController.Highlight(HighlightControllerParts.ControllerPart.Bumper);
                    break;
                case 3:
                    // Step3: Move to the teleportation target
                    letterOutline.enabled = false;
                    teleportationTarget.SetActive(true);
                    leftController.ResetHighLight(HighlightControllerParts.ControllerPart.Bumper);
                    rightController.ResetHighLight(HighlightControllerParts.ControllerPart.Bumper);
                    rightController.Highlight(HighlightControllerParts.ControllerPart.ThumbStick);
                    break;
                case 4:             
                    // Step4: Pet Anky
                    StartCoroutine(StartFollowingPlayerWithDelay());
                    teleportationTarget.SetActive(false);
                    rightController.ResetHighLight(HighlightControllerParts.ControllerPart.ThumbStick);
                    break;
                case 5:
                    // tutorial completed
                    tutorialCompleted = true;
                    break;
            }
        }

        if(currentStepIndex != prevStepIndex)
        {
            // A step has completed
            onTutorialAnyStepCompleted.Invoke();   

            switch (prevStepIndex)
            {                                        
                case 1:
                    onTutorialPart1Completed.Invoke();
                    break;
                case 2:
                    onTutorialPart2Completed.Invoke();
                    break;
                case 3:
                    onTutorialPart3Completed.Invoke();
                    break;
                case 4:
                    onTutorialPart4Completed.Invoke();
                    break;
                case 5:
                    onTutorialPart5Completed.Invoke();
                    break;
            }
            prevStepIndex = currentStepIndex;
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
