using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropOffLocation : MonoBehaviour
{
    public CollectorHandler collectorHandler;
    public CollectorTypes collectorType;

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
            // if(InCorrectDropOff(other.gameObject)){
            // }
        }
    }

    bool InCorrectDropOff(GameObject item)
    {
        // If wrong location
        if(item.GetComponent<ObjectToCollect>().dropOffLocation != collectorType){
            MoveObjectOutOfCollector(item);
        }
        return true;
    }

    void MoveObjectOutOfCollector(GameObject item){

    }
}
