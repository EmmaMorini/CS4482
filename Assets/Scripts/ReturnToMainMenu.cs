using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    float startTime;
    private void OnEnable()
    {
        startTime = Time.unscaledTime;
    }

    public void Update()
    {
        if (Time.unscaledTime - startTime > 5f)
        {
            SceneManager.LoadScene("MainMenu");
            this.enabled = false;
        }
    }
}
