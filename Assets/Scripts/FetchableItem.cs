using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class FetchableItem : MonoBehaviour
{
    [SerializeField] GameObject agent = null;
    [SerializeField] GameObject target = null;
    [SerializeField] Grabber pointOfAttachment = null;

    private MoveToTarget mover = null;
    private float initialMoverOffset = 0f;

    private float grabDistance = 2f;
    private float releaseDistance = 2f;

    private bool isFetching = false;
    private bool isReturning = false;
    private bool isCoolingDown = true;


    private void Awake()
    {
        if (!agent.TryGetComponent<MoveToTarget>(out mover))
        {
            Debug.LogError("agent has no MoveToTarget component");
            return;
        }
        initialMoverOffset = mover.GetOffset();
    }

    private void Update()
    {
        if (!mover) return;

        if (isFetching)
        {
            if (ItemIsReachedByAgent())
            {               
                mover.StopWalking();
                StartCoroutine(StartReturnSequence());
            }
        }

        if(isReturning)
        { 
            if (ItemIsReturnedByAgent())
            {
                mover.StopWalking();
                StartCoroutine(DetachMe());           
            }
        }

        if (isCoolingDown)
        {
            StartCoroutine(FetchCoolDown());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !isFetching && !isReturning && !isCoolingDown)
        {          
            StartCoroutine(StartFetching());
        }
    }

    IEnumerator StartFetching()
    {
        if (!mover) yield break;
        yield return new WaitForSeconds(1);
        mover.SetTarget(this.gameObject);
        mover.SetOffset(0);
        mover.StartWalking();
        isFetching = true;
    }

    IEnumerator StartReturnSequence()
    {
        yield return new WaitForSeconds(1);
        pointOfAttachment.Attach(this.gameObject);
        yield return new WaitForSeconds(1);
        mover.SetTarget(target.gameObject);
        mover.SetOffset(initialMoverOffset);
        mover.StartWalking();
        isFetching = false;
        isReturning = true;
    }

    IEnumerator DetachMe()
    {      
        yield return new WaitForSeconds(1);
        pointOfAttachment.Detach();
        isReturning = false;
        isCoolingDown = true;
        mover.SetTarget(null);
    }   

    IEnumerator FetchCoolDown()
    {
        yield return new WaitForSeconds(2);
        isCoolingDown = false;
    }

    bool ItemIsReachedByAgent()
    {
        return Vector3.Distance(this.transform.position, agent.transform.position) < grabDistance;
    }

    bool ItemIsReturnedByAgent()
    {
        return Vector3.Distance(this.transform.position, target.transform.position) < releaseDistance;
    }
}
