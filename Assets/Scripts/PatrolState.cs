using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649

public class PatrolState : State
{
    public GameObject waypointContainer;
    private List<Transform> waypoints;
    private bool playerSpotted;

    private bool shouldIdle;
    [SerializeField] private float idleChance;

    [SerializeField] private float radiusTreshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float visionSpread;
    [SerializeField] private int numRays;
    [SerializeField] private Material visionMaterial;

    private int waypointIndex = 0;

    private void Start()
    {
        ResetValues();
        waypoints = new List<Transform>();
        for (int i = 0; i < waypointContainer.transform.childCount; i++)
        {
            waypoints.Add(waypointContainer.transform.GetChild(i));
        }
    }

    protected override void DoActions(FiniteStateMachine controller)
    {
        Transform kidTransform = controller.kid.transform;

        // MOVEMENT
        Transform target = waypoints[waypointIndex];
        // Calculate direction
        Vector3 heading = target.position - kidTransform.position;
        // if target reached, go to next waypoint
        float distance = heading.magnitude;
        if (distance <= radiusTreshold)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Count;
            // Maybe idle
            float rand = Random.Range(0, 100) / 100.0f;
            shouldIdle = (rand < idleChance) ? true : false;
        }
        // move to target
        var direction = heading / distance;
        kidTransform.position = kidTransform.position + direction * moveSpeed * Time.deltaTime;

        // rotate to target
        kidTransform.LookAt(target.position);

        // Check raycasts
        DoRayCasts(kidTransform);
    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        // check if we should do a transistion
        if (playerSpotted)
        {
            ResetValues();
            AttackState state = GetComponent<AttackState>();
            controller.TransitionToState(state);
            return;
        }

        if (shouldIdle)
        {
            ResetValues();
            IdleState state = GetComponent<IdleState>();
            controller.TransitionToState(state);
            return;
        }

        
    }

    private void DoRayCasts(Transform kidTransform)
    {
        //Collect points for drawing mesh
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        Vector3 startingPoint = kidTransform.position;
        vertices.Add(startingPoint);

        float angleStep = visionSpread / numRays;
        for (int i = 0; i < numRays; i++)
        {
            float currentAngle = -visionSpread/2.0f + angleStep * i;
            Vector3 positiveTarget = new Vector3(Mathf.Sin(currentAngle), 0, Mathf.Cos(currentAngle));

            CastRay(kidTransform, kidTransform.position, positiveTarget, vertices);

            //if (currentAngle != 0)
            //{
            //    Vector3 negativeTarget = new Vector3(Mathf.Sin(-currentAngle), 0, Mathf.Cos(-currentAngle));
            //    CastRay(kidTransform, kidTransform.position, negativeTarget, vertices);
            //}

            if (i > 0)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
            }
        }
        DrawRay(vertices, triangles);
    }

    private void DrawRay(List<Vector3> vertices, List<int> triangles)
    {
        Mesh m = new Mesh();
        //https://www.linkedin.com/pulse/using-raycasts-dynamically-generated-geometry-create-line-thomas/
        m.vertices = vertices.ToArray();
        m.triangles = triangles.ToArray();
        Graphics.DrawMesh(m, Vector3.zero, Quaternion.identity, visionMaterial, 0, null, 0, null, false, false, true);
    }

    private void CastRay(Transform kidTransform, Vector3 start, Vector3 end, List<Vector3> vertices)
    {
        int layerMask = 1 << 2;
        layerMask = ~layerMask;
        RaycastHit hit;
        int maxDistance = 8;
        // Does the ray intersect any objects excluding the player layer
        Vector3 sightVector = kidTransform.TransformDirection(end);
        if (Physics.Raycast(start, sightVector, out hit, maxDistance, layerMask))
        {
            Debug.DrawRay(start, sightVector * hit.distance, Color.yellow);
            if (hit.collider.gameObject.tag == "Player")
            {
                playerSpotted = true;
            }
            vertices.Add(start + sightVector * hit.distance);
        }
        else
        {
            Debug.DrawRay(start, sightVector * maxDistance, Color.white);
            vertices.Add(start + sightVector * maxDistance);
        }
    }

    private void ResetValues()
    {
        playerSpotted = false;
        shouldIdle = false;
    }

    public override string GetStateName()
    {
        return "PatrolState";
    }
}
