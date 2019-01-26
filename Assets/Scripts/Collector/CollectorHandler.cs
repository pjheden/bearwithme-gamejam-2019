using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectorHandler : MonoBehaviour
{
    [SerializeField] private TextMeshPro collectedText;
    [SerializeField] private float bonusDeliverTime;
    public GameController gameController;
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
        collectedText.SetText(collectedObjects.Count.ToString());
    }

    public void AddItemToCollectedObjects(GameObject item)
    {
        collectedObjects.Add(item);
        gameController.AddTime(bonusDeliverTime);
        gameController.AddScore(1);

        // Debug.Log("COLLECTED: " + item.gameObject.name);
        SetCountText();
        item.gameObject.active = false;

        if (collectedObjects.Count == 10)
            gameController.SetWin(true);
    }
}
