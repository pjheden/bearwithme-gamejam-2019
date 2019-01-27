using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryMenu : MonoBehaviour
{
    public Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(NextScene);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void NextScene()
    {
        SceneManager.LoadScene("KidScene 1");
    }
}
