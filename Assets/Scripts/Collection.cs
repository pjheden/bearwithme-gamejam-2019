using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 0649

public class Collection : MonoBehaviour
{
    private List<GameObject> collectedObjects;
    [SerializeField] private Text collectedText;


    // Start is called before the first frame update
    void Start()
    {
        collectedObjects = new List<GameObject>();
    }

    private void SetCountText()
    {
        collectedText.text = collectedObjects.Count.ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter! " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Pickupable")
        {
            collectedObjects.Add(collision.gameObject);
            SetCountText();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Pickupable")
        {
            collectedObjects.Remove(collision.gameObject);
            SetCountText();
        }
    }
}
