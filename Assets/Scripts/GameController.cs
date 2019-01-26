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
    private bool isWin;
    private float score;

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        isWin = false;
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

        if (roundTime <= 0.0f || (isWin && !gameOver))
        {
            //Gameover
            gameOver = true;
            gameoverTime -= Time.deltaTime;
            SetGameOverText(isWin);
            if (gameoverTime <= 0.0f)
            {
                SceneManager.LoadScene("KidScene");
            }
        }
    }

    public void SetWin(bool isWin)
    {
        this.isWin = isWin;
    }

    public void AddTime(float time)
    {
        roundTime += time;
    }

    public void AddScore(float scr)
    {
        score += scr;
    }

    private void SetGameOverText(bool win)
    {
        if (!win)
        {
            lazerTimer.SetText("Game Over \n You missed " + (10 - score) );
        } else
        {
            lazerTimer.SetText("You win! \n Time left " + Mathf.Round(roundTime).ToString());
        }
        lazerTimer.fontSize = 19;
    }

    private void SetTimeText()
    {
        string seconds = Mathf.Round(roundTime).ToString();
        if (seconds.Length == 1)
        {
            seconds = "0" + seconds;
        }
        lazerTimer.SetText("00:"+ seconds);
    }

}
