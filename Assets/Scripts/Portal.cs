using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    List<GameObject> listOfOpponents = new List<GameObject>();
    bool portalActive;
    bool isDone;
 
    void Start()
    {
        listOfOpponents.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        print(listOfOpponents.Count);
        portalActive = false;
    }

    private void Update()
    {
        if (AreOpponentsDead())
        {
            portalActive = true;
        }
        if (isDone)
        {
            SceneManager.LoadScene("Day2");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            isDone = true;
            //OnGameOver?.Invoke();
        }
    }

    public void KilledOpponent(GameObject opponent)
    {
        if(listOfOpponents.Contains(opponent))
        {
            listOfOpponents.Remove(opponent);
        }
 
        print(listOfOpponents.Count);
    }
 
    public bool AreOpponentsDead()
    {
        if(listOfOpponents.Count <= 0)
        {
            // They are dead!
            print("Portal is activated");
            return true;
        }
        else
        {
            // They're still alive dangit
            return false;
        }
    }
}