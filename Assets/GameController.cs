using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text timer;
    [SerializeField] private float roundTime;

    // Start is called before the first frame update
    void Start()
    {
        SetTimeText();
    }

    // Update is called once per frame
    void Update()
    {
        roundTime -= Time.deltaTime;
        roundTime = (roundTime < 0) ? 0 : roundTime;
        SetTimeText();

        if (roundTime <= 0.0f)
        {
            //Gameover
            Debug.Log("Game over!");
        }
    }

    public void AddTime(float time)
    {
        roundTime += time;
    }

    private void SetTimeText()
    {
        timer.text = Mathf.Round(roundTime).ToString();
    }
}
