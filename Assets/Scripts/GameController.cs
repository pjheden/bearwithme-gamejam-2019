using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public TextMeshPro lazerTimer;

    [SerializeField] private float roundTime;
    [SerializeField] private float gameoverTime;

    private bool gameOver;
    private float score;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        SetTimeText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            roundTime -= Time.deltaTime;
            SetTimeText();
        }

        if (roundTime <= 0.0f)
        {
            //Gameover
            gameOver = true;
            gameoverTime -= Time.deltaTime;
            SetGameOverText();
            if (gameoverTime <= 0.0f)
            {
                SceneManager.LoadScene("KidScene");
            }
        }
    }

    public void AddTime(float time)
    {
        roundTime += time;
    }

    public void AddScore(float scr)
    {
        score += scr;
    }

    private void SetGameOverText()
    {
        lazerTimer.SetText("Game Over \n You collected " + score );
        lazerTimer.fontSize = 19;
    }

    private void SetTimeText()
    {
        lazerTimer.SetText("00:"+ Mathf.Round(roundTime).ToString());
    }

}
