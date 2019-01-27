
using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public static class Lerper 
{
    public static IEnumerator MoveToPosition(Vector3 newPosition, GameObject objectToMove, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = objectToMove.transform.position;
        while (elapsedTime < time)
        {
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, newPosition, (elapsedTime / time));
            objectToMove.transform.position = new Vector3(objectToMove.transform.position.x, 1, objectToMove.transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}