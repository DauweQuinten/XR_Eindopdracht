using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Unity Events

    public UnityEvent onTutorialAnyStepCompleted;
    public UnityEvent onTutorialPart1Completed;
    public UnityEvent onTutorialPart2Completed;
    public UnityEvent onTutorialPart3Completed;
    public UnityEvent onTutorialPart4Completed;
    public UnityEvent onTutorialPart5Completed;
    public UnityEvent onTutorialPart6Completed;
    public UnityEvent onTutorialPart7Completed;
    public UnityEvent onTutorialCompleted;

    #endregion

    #region Serialized Fields

    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject pet;
    [SerializeField] private GameObject tableTarget;
    [SerializeField] private GameObject teleportationTarget;
    [SerializeField] private Outline petOutline;
    [SerializeField] private Outline letterOutline;
    [SerializeField] private Outline brancheOutline;
    [SerializeField] private HighlightControllerParts leftController;
    [SerializeField] private HighlightControllerParts rightController;
    [SerializeField] private FruitTree[] trees = new FruitTree[0];
    [SerializeField] private MoodPanel moodPanel;

    #endregion

    #region Private Fields

    private MoveToTarget petMover = null;
    private float defaultMoverOffset = 0;
    private int currentStepIndex = 1;
    private int prevStepIndex = 1;
    private bool tutorialStarted = false;
    private bool tutorialCompleted = false;

    private float maxFullness = 100;
    private float maxAmusement = 100;
    private float maxHappiness = 100;
    private float fullnessAmount = 50;
    private float amusementAmount = 50;
    private float happinessAmount = 50;
    private float hungerBoundary = 20;
    private float amusementBoundary = 20;
    private float happinessBoundary = 20;
    private float fullnessDecreaseSpeed = 1;
    private float amusementDecreaseSpeed = 4;
    private float happinessDecreaseSpeed = 2;

    #endregion


    private void Awake()
    {
        if (!pet.TryGetComponent<MoveToTarget>(out petMover))
        {
            Debug.LogError("pet has no MoveToTarget component");            
        }
        
        if (petMover)
        {
            defaultMoverOffset = petMover.GetOffset();
        }
    }

    private void Start()
    {
        InvokeRepeating("DecreaseParameters", 0f, 1f);
    }

    private void Update()
    {                  
        if (tutorialCompleted)
        {         
            CheckParameters();

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
        else if(tutorialStarted)
        {
            HandleTutorialFLow();                     
        }
        else
        {
            StartCoroutine(StartTutorialWhenReady());
        }


        HandleEvents(); 
    }

    void HandleTutorialFLow()
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
                petOutline.enabled = true;
                moodPanel.SetMood(MoodPanel.Mood.PetMe);
                break;
            case 5:
                // Teap5: Pluck fruit & feed Anky
                petOutline.enabled = false;
                moodPanel.SetMood(MoodPanel.Mood.Hungry);
                foreach (FruitTree tree in trees)
                {
                    tree.TogglePositionIndicator(true);
                }
                break;
            case 6:
                // Play with branche
                foreach (FruitTree tree in trees)
                {
                    tree.TogglePositionIndicator(false);
                }
                moodPanel.SetMood(MoodPanel.Mood.bored);
                brancheOutline.enabled = true;
                break;
            case 7:
                brancheOutline.enabled = false;
                moodPanel.SetMood(MoodPanel.Mood.neutral);
                // tutorial completed
                tutorialCompleted = true;
                break;
        }
    }

    void HandleEvents()
    {
        if (currentStepIndex != prevStepIndex)
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
                case 6:
                    onTutorialPart6Completed.Invoke();
                    break;
                case 7:
                    onTutorialPart7Completed.Invoke();
                    break;
            }
            prevStepIndex = currentStepIndex;
        }
    }

    void CheckParameters()
    {
        if (fullnessAmount < hungerBoundary)
        {
            moodPanel.SetMood(MoodPanel.Mood.Hungry);
        }
        else if(happinessAmount < happinessBoundary)
        {
            moodPanel.SetMood(MoodPanel.Mood.PetMe);
        }
        else if(amusementAmount < amusementBoundary)
        {
            moodPanel.SetMood(MoodPanel.Mood.bored);
        }
        else
        {
            moodPanel.SetMood(MoodPanel.Mood.neutral);
        }
    }

    IEnumerator StartFollowingPlayerWithDelay(float delay = 2f)
    {
        yield return new WaitForSeconds(delay);
        petMover.SetTarget(xrOrigin);
        petMover.SetOffset(defaultMoverOffset);
        petMover.StartWalking();
    }

    IEnumerator StartTutorialWhenReady()
    {
        yield return new WaitUntil(() => leftController.ControllerMappingCompleted());
        yield return new WaitUntil(() => rightController.ControllerMappingCompleted());
        Debug.Log("Tutorial can start");
        tutorialStarted = true;
    }

    void DecreaseParameters()
    {
        if (tutorialCompleted)
        {
            fullnessAmount -= fullnessDecreaseSpeed;
            happinessAmount -= happinessDecreaseSpeed;
            amusementAmount -= amusementDecreaseSpeed;
            fullnessAmount = Mathf.Clamp(fullnessAmount, 0f, maxFullness);
            happinessAmount = Mathf.Clamp(happinessAmount, 0f, maxHappiness);
            amusementAmount = Mathf.Clamp(amusementAmount, 0f, maxAmusement);
        }
    }

    public void AddPointsToFullness(float amount)
    {
        fullnessAmount += amount;
    }

    public void AddPointsToHappiness(float amount)
    {
        happinessAmount += amount;
    }

    public void AddPointsToAmusement(float amount)
    {
        amusementAmount += amount;
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
