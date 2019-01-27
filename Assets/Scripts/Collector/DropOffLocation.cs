using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropOffLocation : MonoBehaviour
{
    public CollectorHandler collectorHandler;
    public CollectorTypes collectorType;
    public float deniedDistance;
    public PlayerController playerController;

    [HideInInspector]
    public Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineColor = ColorChanger.GetTypeColor(collectorType);
        outline.OutlineWidth = 10f;
        outline.enabled = false;
        outline.OutlineMode = Outline.Mode.OutlineVisible; // Prevents outline from drawig over objects infront
        // ColorChanger.ChangeObjectCorToMatchType(transform.gameObject, collectorType);
    }

    // The pickupables colliders are disables when held.
    // If object is dropped when jn trigger this will fire!
    private void OnTriggerEnter(Collider other) 
    {   
        if(other.gameObject.tag == "Pickupable")
        {   
            if(IsInCorrectDropOff(other.gameObject)){
                collectorHandler.AddItemToCollectedObjects(other.gameObject);
                playerController.SetObjectHoldingNull();
                outline.enabled = false;
            }
        }
    }

    bool IsInCorrectDropOff(GameObject item)
    {
        // If wrong location
        if(item.GetComponent<ObjectToCollect>().dropOffLocation != collectorType){
            return false;
        }
        return true;
    }

    // If you place the item in the wrong collector
    void MoveObjectOutOfCollector(GameObject item)
    {
        Vector3 deniedPosition = transform.position + transform.forward * deniedDistance;
        StartCoroutine(MoveToPosition(deniedPosition, item, 0.5f));
    }


    IEnumerator MoveToPosition(Vector3 newPosition, GameObject objectToMove, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = transform.position;
        while (elapsedTime < time)
        {
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, newPosition, (elapsedTime / time));
            objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, 1, objectToMove.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
