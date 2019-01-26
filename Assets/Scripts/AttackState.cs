using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649

public class AttackState : State
{
    /*
     * Go to player
     * Throw away player
     */
    private bool playerThrown;
    [SerializeField] private float radiusTreshold;
    [SerializeField] private float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        playerThrown = false;
    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        if (playerThrown)
        {
            ResetValues();
            //Transition
            //PatrolState state = GetComponent<PatrolState>();
            //controller.TransitionToState(state);

            SleepState state = GetComponent<SleepState>();
            controller.TransitionToState(state);
        }
    }
    protected override void DoActions(FiniteStateMachine controller)
    {
        // MOVEMENT
        Transform target = controller.doll.transform;
        // Calculate direction
        Vector3 heading = target.position - controller.kid.transform.position;
        // if target reached, pick up doll
        float distance = heading.magnitude;
        if (distance <= radiusTreshold)
        {
            // pick up doll
            controller.doll.transform.position = Vector3.zero;
            playerThrown = true;
        }
        // move to target
        var direction = heading / distance;
        controller.kid.transform.position = controller.kid.transform.position + direction * moveSpeed * Time.deltaTime;

        // rotate to target
        controller.kid.transform.LookAt(target.position);
    }

    private void ResetValues()
    {
        playerThrown = false;
    }

    public override string GetStateName()
    {
        return "AttackState";
    }
}
