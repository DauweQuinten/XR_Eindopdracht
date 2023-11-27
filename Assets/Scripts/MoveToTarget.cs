using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;


[RequireComponent(typeof(NavMeshAgent))]
public class MoveToTarget : MonoBehaviour
{
    private GameObject target = null;
    [SerializeField] private float lookAroundSpeed = 0.5f;
    [SerializeField] private float offset = 5f;
    [SerializeField] private float lookatAccuracy = 0.9f;
    [SerializeField] Animator animator = null;
    NavMeshAgent navMeshAgent = null;

    private bool isWalking = false;
    private bool isTurning = false;
    private bool isWalkingEnabled = false;

    private void Awake()
    {
        if(!TryGetComponent<NavMeshAgent>(out navMeshAgent))
        {
            Debug.LogError($"Agent {gameObject.name} has no NavMeshAgent component");
        }
    }


    // Update is called once per frame
    void Update()
    {            
        if (target)
        {
            //Look at target
            isTurning = !TargetIsInView();
            isWalking = isWalkingEnabled && !isTurning && !TargetIsReached();

            if (isTurning)
            {
                TurnTowardsTarget();
            }


            if (isWalking)
            {
                navMeshAgent.SetDestination(target.transform.position);
            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
            }

            if (animator)
            {
                bool currentWalkSate = animator.GetBool("IsWalking");
                if (currentWalkSate != isWalking)
                {
                    animator.SetBool("IsWalking", isWalking);
                }
            }
        }
    }

    private bool TargetIsInView()
    {
        // The dotproduct of 2 normalized vectors returns the cos-alpha of the angle between those 2 vectors
        // cos-alpha == 1 -> Looking at target
        Vector3 dirToTarget = target.transform.position - transform.position;
        float dotProduct = Vector3.Dot(dirToTarget.normalized, transform.forward);

        if (dotProduct > lookatAccuracy)
        {
            // Looking at target
            return true;
        }
        else return false;
    }

    private bool TargetIsReached()
    {
        return Vector3.Distance(transform.position, target.transform.position) < offset;
    }

    public void StartWalking()
    {       
        isWalkingEnabled = true;
    }

    public void StopWalking()
    {
        isWalkingEnabled = false;
    }

    private void TurnTowardsTarget()
    {
        Vector3 forwardVector = target.transform.position - transform.position;
        Quaternion LookAtRotation = Quaternion.LookRotation(forwardVector, Vector3.up);
        Quaternion newRotation = Quaternion.Lerp(transform.rotation, LookAtRotation, lookAroundSpeed * Time.deltaTime);
        transform.rotation = newRotation;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public GameObject GetTarget()
    {        
        return target;
    }

    public void SetOffset(float offset)
    {
        this.offset = offset;
    }

    public float GetOffset()
    {
        return this.offset;
    }
}
