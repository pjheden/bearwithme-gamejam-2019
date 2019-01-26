using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectorHandler : MonoBehaviour
{
    [SerializeField] private Text collectedText;
    
    private List<GameObject> collectedObjects;

    // Start is called before the first frame update
    void Start()
    {
        collectedObjects = new List<GameObject>();
        SetCountText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCountText()
    {
        collectedText.text = "Toys collected: " + collectedObjects.Count.ToString();
    }

    public void AddItemToCollectedObjects(GameObject item)
    {
        collectedObjects.Add(item);

        // Debug.Log("COLLECTED: " + item.gameObject.name);
        SetCountText();
        item.gameObject.active = false;
    }
}
