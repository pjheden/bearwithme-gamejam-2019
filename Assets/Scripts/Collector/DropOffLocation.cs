using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropOffLocation : MonoBehaviour
{
    public CollectorHandler collectorHandler;
    public CollectorTypes collectorType;
    public float deniedDistance;

    // Start is called before the first frame update
    void Start()
    {
        ColorChanger.ChangeObjectCorToMatchType(transform.gameObject, collectorType);

        // Color objectColor;
        // switch (collectorType)
        // {
        //     case CollectorTypes.BOOKSHELF:
        //         objectColor = Color.blue;
        //     break;
        //     case CollectorTypes.CLOSET:
        //         objectColor = Color.green;
        //     break;
        //     case CollectorTypes.DRAWER:
        //         objectColor = Color.red;
        //     break;
        //     default:
        //         objectColor = Color.white;
        //     break;
        // }

        // GetComponent<Renderer>().material.SetColor("_Color", objectColor);
    }

    // The pickupables colliders are disables when held.
    // If object is dropped when jn trigger this will fire!
    private void OnTriggerEnter(Collider other) 
    {   
        if(other.gameObject.tag == "Pickupable")
        {   
            if(IsInCorrectDropOff(other.gameObject)){
                collectorHandler.AddItemToCollectedObjects(other.gameObject);
            }
        }
    }

    bool IsInCorrectDropOff(GameObject item)
    {
        // If wrong location
        if(item.GetComponent<ObjectToCollect>().dropOffLocation != collectorType){
            MoveObjectOutOfCollector(item);
            return false;
        }
        return true;
    }

    // If you place the item in the wrong collector
    void MoveObjectOutOfCollector(GameObject item)
    {
        Vector3 deniedPosition = transform.position + transform.forward * deniedDistance;
        deniedPosition.y = 1;
        item.transform.position = deniedPosition;
    }
}
