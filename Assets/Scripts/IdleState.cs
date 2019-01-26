using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    private State nextState;
    private float idleTimer;

    [SerializeField] private float maxIdleTime;
    [SerializeField] private float radiusTreshold;

    // Start is called before the first frame update
    void Start()
    {
        nextState = null;
    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        if (nextState == null)
        {
            nextState = GetComponent<PatrolState>();
        }
        // Check if finish idling
        if (idleTimer > maxIdleTime)
        {
            ResetValues();
            controller.TransitionToState(nextState);
            return;
        }

        // Check if doll is nearby to trigger AttackState
        Transform target = controller.doll.transform;
        // Calculate direction
        Vector3 heading = target.position - controller.kid.transform.position;
        // if target reached, go to next waypoint
        float distance = heading.magnitude;
        if (distance < radiusTreshold)
        {
            ResetValues();
            AttackState state = GetComponent<AttackState>();
            controller.TransitionToState(state);
            return;
        }
    }

    protected override void DoActions(FiniteStateMachine controller)
    {
        // Add time to idle timer
        idleTimer += Time.deltaTime;
    }   

    private void ResetValues()
    {
        idleTimer = 0.0f;
    }

    public void SetNextState(State state)
    {
        nextState = state;
    }

    public override void PrintStateName()
    {
        Debug.Log("IdleState");
    }

}
