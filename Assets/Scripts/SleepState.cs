using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State/Sleep")]
public class SleepState : State
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void CheckTransitions(FiniteStateMachine controller)
    {
        throw new System.NotImplementedException();
    }

    protected override void DoActions(FiniteStateMachine controller)
    {
        throw new System.NotImplementedException();
    }
}
