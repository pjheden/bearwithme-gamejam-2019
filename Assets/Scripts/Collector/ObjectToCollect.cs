using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToCollect : MonoBehaviour
{
    public CollectorTypes dropOffLocation;

    void Start() 
    {
        Color objectColor;
        switch (dropOffLocation)
        {
            case CollectorTypes.BOOKSHELF:
                objectColor = Color.blue;
            break;
            case CollectorTypes.CLOSET:
                objectColor = Color.green;
            break;
            case CollectorTypes.DRAWER:
                objectColor = Color.red;
            break;
        }

        // GetComponent<Renderer>().material.SetColor(objectColor);
    }
}
