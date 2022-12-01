using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomShooting : MonoBehaviour
{

    bool needsNewBehaviour = true;
    bool currentlyShooting;
    bool currentlyIdling;
    public float maxShootTime;
    public float minShootTime;
    float shootTime;
    float shootPeriod;
    float idleTime;
    float idleLength;
    public float idleMin;
    public float idleMax;
    string behaviour;
    bool vertical;
    public Animator anim;
    float pauseTime;
    float pausePeriod = 0.2f;
    public void Rando(int direction, Vector2 pos)
    {
        if (needsNewBehaviour)
        {
            behaviour = ChooseBehaviour();
            needsNewBehaviour = false;
        }
        // Debug.Log(behaviour);
        switch (behaviour)
        {
            case "shoot":
                if (!currentlyShooting)
                {
                    shootTime = 0;
                    shootPeriod = Random.Range(minShootTime, maxShootTime);
                    currentlyShooting = true;
                    pauseTime = pausePeriod;
                }
                if (shootTime < shootPeriod)
                {
                    Debug.Log("Shooting time");
                    if (anim)
                    {
                        anim.SetFloat("X", direction);
                        anim.SetFloat("Y", 0);
                        shootTime += Time.deltaTime;
                        anim.SetTrigger("Launch");
                    }
                    else
                    {
                        Debug.Log("Rando Shooting Go Else");
                        StationaryShooting station = gameObject.GetComponent<StationaryShooting>();
                        if (station)
                        {
                            Debug.Log("Station found");
                            pauseTime -= Time.deltaTime;
                            Debug.Log("station pause time: " + pauseTime);
                            if (pauseTime <= 0)
                            {
                                Debug.Log("station no pause, shoot");
                                station.Launch();
                                pauseTime = pausePeriod;
                            }
                            else return;
                        }
                    }
                }
                else
                {
                    currentlyShooting = false;
                    needsNewBehaviour = true;
                }
                break;

            case "idle":

                if (!currentlyIdling)
                {
                    idleTime = 0;
                    idleLength = Random.Range(idleMin, idleMax);
                    currentlyIdling = true;
                }

                if (idleTime < idleLength)
                {
                    Debug.Log("Idle time");
                    idleTime += Time.deltaTime;

                }
                else
                {
                    currentlyIdling = false;
                    needsNewBehaviour = true;
                }
                break;

            default:
                Debug.Log("default case here");
                break;
        }
    }

    string ChooseBehaviour()
    {
        int behaviour = Random.Range(0, 2);
        switch (behaviour)
        {
            case 0:
                return "shoot";
            default:
                return "idle";
        }
    }
}
