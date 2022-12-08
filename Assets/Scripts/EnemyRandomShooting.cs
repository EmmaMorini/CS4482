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
            Debug.Log("Hello does it goes here?");
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
                    // Debug.Log("Shooting time");
                    if (anim)
                    {
                        Debug.Log(anim);
                        anim.SetFloat("Look X", direction);
                        // anim.SetFloat("Y", 0);
                        anim.SetTrigger("Launch");
                    }
                    else
                    {
                        StationaryShooting station = gameObject.GetComponent<StationaryShooting>();
                        if (station)
                        {
                            pauseTime -= Time.deltaTime;
                            if (pauseTime <= 0)
                            {
                                station.Launch(true);
                                pauseTime = pausePeriod;
                            }
                            else
                            {
                                station.Launch(false);
                            }
                        }
                    }
                    shootTime += Time.deltaTime;
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
