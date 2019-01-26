using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropOffLocation : MonoBehaviour
{
    public CollectorHandler collectorHandler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // The pickupables colliders are disables when held.
    // If object is dropped when jn trigger this will fire!
    private void OnTriggerEnter(Collider other) 
    {   
        if(other.gameObject.tag == "Pickupable")
        {
            collectorHandler.AddItemToCollectedObjects(other.gameObject);
        }
    }
}
