using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepState : State
{
    private bool sleeping;
    private float sleepTime;
    private bool inBed;
    [SerializeField] private float maxSleep;
    [SerializeField] private GameObject bed;
    [SerializeField] private float radiusTreshold;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        sleeping = true;
        inBed = false;
    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        if (!sleeping)
        {
            ResetValues();
            PatrolState state = GetComponent<PatrolState>();
            controller.TransitionToState(state);
        }
    }

    protected override void DoActions(FiniteStateMachine controller)
    {
        if (inBed)
        {
            //if in bed, sleep
            sleepTime += Time.deltaTime;
            if (sleepTime > maxSleep)
            {
                Wake();
            }
        } else
        {
            //go to bed
            Transform target = bed.transform;
            // Calculate direction
            Vector3 heading = target.position - controller.kid.transform.position;
            // if target reached, go to next waypoint
            float distance = heading.magnitude;
            if (distance <= radiusTreshold)
            {
                inBed = true;
            }
            // move to target
            var direction = heading / distance;
            controller.kid.transform.position = controller.kid.transform.position + direction * moveSpeed * Time.deltaTime;

            // rotate to target
            controller.kid.transform.LookAt(target.position);
        }

        
    }

    private void ResetValues()
    {
        sleeping = true;
        inBed = false;
    }

    public override void PrintStateName()
    {
        Debug.Log("SleepState");
    }

    public void Wake()
    {
        sleeping = false;
        sleepTime = 0.0f;
    }

}
