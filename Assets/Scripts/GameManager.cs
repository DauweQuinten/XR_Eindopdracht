using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject xrOrigin;
    [SerializeField] private GameObject pet;
    private MoveToTarget petMover = null;
    private float defaultMoverOffset = 0;


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
        if (petMover)
        {
            if(petMover.GetTarget() == null)
            {
                StartCoroutine(StartFollowingPlayerWithDelay());
            }
        }
    }

    IEnumerator StartFollowingPlayerWithDelay(float delay=2f)
    {
        yield return new WaitForSeconds(delay);
        petMover.SetTarget(xrOrigin);
        petMover.SetOffset(defaultMoverOffset);
        petMover.StartWalking();
    }
}
