using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    public State currentState;
    public GameObject kid;
    public GameObject doll;
    public GameController gameController;

    private bool aiActive;


    // Start is called before the first frame update
    void Start()
    {
        aiActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!aiActive)
            return;
        currentState.UpdateState (this);
    }

    public void TransitionToState(State nextState)
    {
        currentState = nextState;
    }

    // Debug function for Kid
    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 100.0f, 100.0f), "Kid State: " + currentState.GetStateName());
    }
}
