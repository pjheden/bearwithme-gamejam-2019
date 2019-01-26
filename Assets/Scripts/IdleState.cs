using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649

public class IdleState : State
{
    private State nextState;
    private float idleTimer;

    [SerializeField] private float maxIdleTime;
    [SerializeField] private float radiusTreshold;

    // Ray variables
    [SerializeField] private float visionSpread;
    [SerializeField] private int numRays;
    private bool playerSpotted;


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

        if (playerSpotted)
        {
            ResetValues();
            AttackState state = GetComponent<AttackState>();
            controller.TransitionToState(state);
            return;
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

    private void CastRay(Transform kidTransform, Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        int layerMask = 1 << 2;
        layerMask = ~layerMask;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(start, kidTransform.TransformDirection(end), out hit, 30, layerMask))
        {
            Debug.DrawRay(start, kidTransform.TransformDirection(end) * hit.distance, Color.yellow);
            if (hit.collider.gameObject.tag == "Player")
            {
                playerSpotted = true;
            }
        }
        else
        {
            Debug.DrawRay(start, kidTransform.TransformDirection(end) * 1000, Color.white);
        }
    }

    protected override void DoActions(FiniteStateMachine controller)
    {
        // Add time to idle timer
        idleTimer += Time.deltaTime;

        Transform kidTransform = controller.kid.transform;

        // Check raycasts
        float angleStep = visionSpread / numRays;
        for (int i = 0; i < numRays; i++)
        {
            float currentAngle = angleStep * i;
            Vector3 positiveTarget = new Vector3(Mathf.Sin(currentAngle), 0, Mathf.Cos(currentAngle));

            CastRay(kidTransform, kidTransform.position, positiveTarget);

            if (currentAngle != 0)
            {
                Vector3 negativeTarget = new Vector3(Mathf.Sin(-currentAngle), 0, Mathf.Cos(-currentAngle));
                CastRay(kidTransform, kidTransform.position, negativeTarget);
            }

        }
    }   

    private void ResetValues()
    {
        idleTimer = 0.0f;
        playerSpotted = false;
    }

    public void SetNextState(State state)
    {
        nextState = state;
    }

    public override string GetStateName()
    {
        return "IdleState";
    }

}
