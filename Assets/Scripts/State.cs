using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public abstract class State : MonoBehaviour
{

    public void UpdateState(FiniteStateMachine controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    public abstract void PrintStateName();

    protected abstract void DoActions(FiniteStateMachine controller);

    protected abstract void CheckTransitions(FiniteStateMachine controller);

}