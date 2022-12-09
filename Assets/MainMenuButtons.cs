using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("Day1");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
