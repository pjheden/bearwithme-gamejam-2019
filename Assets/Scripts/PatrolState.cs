using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public GameObject waypointContainer;
    private List<Transform> waypoints;
    private bool playerSpotted;

    [SerializeField] private float radiusTreshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float visionSpread;
    [SerializeField] private int numRays;

    private int waypointIndex = 0;

    private void Start()
    {
        playerSpotted = false;
        waypoints = new List<Transform>();
        for (int i = 0; i < waypointContainer.transform.childCount; i++)
        {
            waypoints.Add(waypointContainer.transform.GetChild(i));
        }
    }

    protected override void DoActions(FiniteStateMachine controller)
    {
        // MOVEMENT
        Transform target = waypoints[waypointIndex];
        // Calculate direction
        Vector3 heading = target.position - controller.kid.transform.position;
        // if target reached, go to next waypoint
        float distance = heading.magnitude;
        if (distance <= radiusTreshold)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Count;
        }
        // move to target
        var direction = heading / distance;
        controller.kid.transform.position = controller.kid.transform.position + direction * moveSpeed * Time.deltaTime;

        // rotate to target
        controller.kid.transform.LookAt(target.position);
    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        Transform kidTransform = controller.kid.transform;
        // check if we should do a transistion
        if (playerSpotted)
        {
            playerSpotted = false;
            //AttackState state;
            AttackState state = GetComponent<AttackState>();
            controller.TransitionToState(state);
        }


        // Check raycasts
        float angleStep = visionSpread / numRays;
        for (int i = 0; i < numRays; i++)
        {
            float currentAngle = angleStep * i;
            Vector3 positiveTarget = new Vector3( Mathf.Sin(currentAngle), 0 , Mathf.Cos(currentAngle) );

            CastRay(kidTransform, kidTransform.position, positiveTarget);

            if (currentAngle != 0)
            {
                Vector3 negativeTarget = new Vector3(Mathf.Sin(-currentAngle), 0, Mathf.Cos(-currentAngle));
                CastRay(kidTransform, kidTransform.position, negativeTarget);
            }

        }
    }

    private void CastRay(Transform kidTransform, Vector3 start, Vector3 end)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(start, kidTransform.TransformDirection(end), out hit, 30))
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

    public override void PrintStateName()
    {
        Debug.Log("PatrolState");
    }
}
