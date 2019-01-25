using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State/Attack")]
public class AttackState : State
    /*
     * Go to player
     * Throw away player
     */
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        throw new System.NotImplementedException();
    }
    protected override void DoActions(FiniteStateMachine controller)
    {
        //// MOVEMENT
        //Transform target = controller.kid.transform;
        //// Calculate direction
        //Vector3 heading = target.position - controller.kid.transform.position;
        //// if target reached, go to next waypoint
        //float distance = heading.magnitude;
        //if (distance <= radiusTreshold)
        //{
        //    waypointIndex = (waypointIndex + 1) % waypoints.Count;
        //}
        //// move to target
        //var direction = heading / distance;
        //controller.kid.transform.position = controller.kid.transform.position + direction * moveSpeed * Time.deltaTime;

        //// rotate to target
        //controller.kid.transform.LookAt(target.position);
    }
}
