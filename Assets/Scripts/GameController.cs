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
            SceneManager.LoadScene("KidScene");
        }
    }

    public void AddTime(float time)
    {
        roundTime += time;
    }

    private void SetTimeText()
    {
        lazerTimer.SetText("00:"+ Mathf.Round(roundTime).ToString());
    }

}
