using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State/Patrol")]
public class PatrolState : State
{

    public GameObject[] waypoints;
    [SerializeField] private float radiusTreshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float visionSpread;
    [SerializeField] private int numRays;

    private int waypointIndex = 0;

    protected override void DoActions(FiniteStateMachine controller)
    {
        // MOVEMENT
        Transform target = waypoints[waypointIndex].transform;
        // Calculate direction
        Vector3 heading = target.position - controller.kid.transform.position;
        // if target reached, go to next waypoint
        float distance = heading.magnitude;
        if (distance <= radiusTreshold)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
        }
        // move to target
        var direction = heading / distance;
        controller.kid.transform.position = controller.kid.transform.position + direction * moveSpeed * Time.deltaTime;




    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        Transform kidTransform = controller.kid.transform;
        // check if we should do a transistion
        State state;
        if (false)
        {
            controller.TransitionToState(state);
        }


        // Check raycasts
        float angleStep = visionSpread / numRays;
        for (int i = 0; i < numRays; i++)
        {
            float currentAngle = angleStep * i;
            Vector3 positiveTarget = new Vector3( 1*Mathf.Cos(currentAngle), 0 ,1*Mathf.Sin(currentAngle) );

            CastRay(kidTransform, kidTransform.position, positiveTarget);

            if (currentAngle != 0)
            {
                Vector3 negativeTarget = new Vector3(1 * Mathf.Cos(-currentAngle), 0, 1 * Mathf.Sin(-currentAngle));
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
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(start, kidTransform.TransformDirection(end) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
}
