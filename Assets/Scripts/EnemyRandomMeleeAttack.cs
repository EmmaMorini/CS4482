using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomMeleeAttack : MonoBehaviour
{
    bool needsNewBehaviour = true;
    bool currentlyAttacking;
    bool currentlyIdling;
    public float maxAttackTime;
    public float minAttackTime;
    float shootTime;
    float shootPeriod;
    float idleTime;
    float idleLength;
    public float idleMin;
    public float idleMax;
    string behaviour;
    bool vertical;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rando(int direction, Vector2 pos)
    {
        if (needsNewBehaviour)
        {
            behaviour = ChooseBehaviour();
            needsNewBehaviour = false;
            Debug.Log(gameObject.name);
        }
        switch (behaviour)
        {
            case "shoot":
                if (!currentlyAttacking)
                {
                    shootTime = 0;
                    shootPeriod = Random.Range(minAttackTime, maxAttackTime);
                    currentlyAttacking = true;
                }
                if (shootTime < shootPeriod)
                {
                    // Debug.Log("Melee active");
                    if (anim){  
                        anim.SetFloat("Look X", direction); 
                        if (gameObject.name == "finalBoss"){
                            string attackType = BossAttack();
                            shootTime += Time.deltaTime;
                            anim.SetTrigger(attackType);
                        } 
                        else{
                            shootTime += Time.deltaTime;
                            anim.SetTrigger("Attack");
                        }                  
                    }
                }
                else
                {
                    currentlyAttacking = false;
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
                    // Debug.Log("Melee deactive");
                    idleTime += Time.deltaTime;

                }
                else
                {
                    currentlyIdling = false;
                    needsNewBehaviour = true;
                }
                break;

            default:
                // Debug.Log("default case here");
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

    string BossAttack()
    {
        int behaviour = Random.Range(0, 2);
        switch (behaviour)
        {
            case 0:
                return "Attack2";
            case 1:
                return "Attack3";
            default:
                return "Attack";
        }
    }
}
