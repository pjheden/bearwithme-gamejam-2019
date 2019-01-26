using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 0649

public class SleepState : State
{
    private bool sleeping;
    private float sleepTime;
    private bool inBed;
    [SerializeField] private float maxSleep;
    [SerializeField] private GameObject bed;
    [SerializeField] private float radiusTreshold;
    [SerializeField] private float moveSpeed;
    [Header("Sounds")]
    public AudioClip enterBedClip;
    public AudioClip leaveBedClip;
    public AudioClip walkClip;
    public AudioSource audioSource;

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
                audioSource.clip = enterBedClip;
                audioSource.Play();
                inBed = true;
                return;
            }
            // move to target
            var direction = heading / distance;
            controller.kid.transform.position = controller.kid.transform.position + direction * moveSpeed * Time.deltaTime;
            if (!audioSource.isPlaying)
            {
                audioSource.clip = walkClip;
                audioSource.Play();
            }

            // rotate to target
            controller.kid.transform.LookAt(target.position);
        }

        
    }

    private void ResetValues()
    {
        sleeping = true;
        inBed = false;
    }

    public override string GetStateName()
    {
        return "SleepState";
    }


    public void Wake()
    {
        sleeping = false;
        sleepTime = 0.0f;
        
        audioSource.clip = leaveBedClip;
        audioSource.Play();
    }

}
