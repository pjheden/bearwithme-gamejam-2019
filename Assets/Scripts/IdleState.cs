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
    [SerializeField] private Material visionMaterial;

    public Animator anim;


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
            state.StartAnimation();
            controller.TransitionToState(state);
            return;
        }

        // Check if finish idling
        if (idleTimer > maxIdleTime)
        {
            ResetValues();
            PatrolState state = GetComponent<PatrolState>();
            state.StartAnimation();
            controller.TransitionToState(state);
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
            state.StartAnimation();
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
            float currentAngle = -visionSpread / 2.0f + angleStep * i;
            Vector3 positiveTarget = new Vector3(Mathf.Sin(currentAngle), 0, Mathf.Cos(currentAngle));

            CastRay(kidTransform, kidTransform.position, positiveTarget, vertices);

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

    protected override void DoActions(FiniteStateMachine controller)
    {
        // Add time to idle timer
        idleTimer += Time.deltaTime;

        Transform kidTransform = controller.kid.transform;

        // Check raycasts
        DoRayCasts(kidTransform);
    }   

    private void ResetValues()
    {
        idleTimer = 0.0f;
        playerSpotted = false;
        anim.SetBool("idle", false);
    }

    public void SetNextState(State state)
    {
        nextState = state;
    }

    public override string GetStateName()
    {
        return "IdleState";
    }

    public void StartAnimation()
    {
        anim.SetBool("idle", true);
    }

}
