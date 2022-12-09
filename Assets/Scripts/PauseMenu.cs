using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseScreen;
    private bool paused = false;
    public TMP_Text healthStat;
    public TMP_Text damageStat;
    public TMP_Text speedStat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause button pressed");
            if(paused)
            {
                unpause();
            }
            else
            {
                pause();
            }
        }
    }
    public void pause()
    {
        healthStat.text = string.Format("Health: {0}%", PlayerStats.Buffs.MaxHealth * 100);
        damageStat.text = string.Format("Damage: {0}%", PlayerStats.Buffs.Damage * 100);
        speedStat.text = string.Format("Speed: {0}%", PlayerStats.Buffs.MoveSpeed * 100);
        pauseScreen.active = true;
        Time.timeScale = 0.0f;
        paused = true;
    }

    public void unpause()
    {
        pauseScreen.active = false;
        Time.timeScale = 1.0f;
        paused = false;
    }
    public void main_menu()
    {
        SceneManager.LoadScene("", LoadSceneMode.Single);
    }
}
