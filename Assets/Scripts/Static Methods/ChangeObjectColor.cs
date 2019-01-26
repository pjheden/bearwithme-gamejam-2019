using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorChanger 
{
    public static void ChangeObjectCorToMatchType(GameObject objectToChange, CollectorTypes collectorType) 
    {
        Color objectColor;
        switch (collectorType)
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
            default:
                objectColor = Color.white;
            break;
        }
        objectToChange.GetComponent<Renderer>().material.SetColor("_Color", objectColor);
    }
}


