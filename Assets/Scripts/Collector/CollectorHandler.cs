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
  private DropOffLocation[] collectors;

  // Start is called before the first frame update
  void Start()
  {
    collectedObjects = new List<GameObject>();
    SetCountText();

    collectors = new DropOffLocation[transform.childCount];
    int count = 0;
    foreach (Transform child in transform)
    {
      collectors[count] = child.gameObject.GetComponent<DropOffLocation>();
      count++;
    }
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
    item.gameObject.SetActive(false);

    if (collectedObjects.Count == 10)
      gameController.SetWin(true);
  }

  public void HighLightCorrectCollector(CollectorTypes type)
  {
    foreach (DropOffLocation collector in collectors)
    {
      if (collector.collectorType == type)
      {
        collector.outline.enabled = true;
      }
    }
  }

  public void DisableHighlightOnCollector()
  {
    foreach (DropOffLocation collector in collectors)
    {
        collector.outline.enabled = false;
    }
  }
}
