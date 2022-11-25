using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomMovement : MonoBehaviour
{
    Rigidbody2D rb;
    private string behaviour;
    private bool needsNewBehaviour = true;
    private bool currentlyMoving = false;
    private bool currentlyIdling = false;
    private float moveTime;
    bool vertical;

    // Floats used to process the pig's idle behaviour.
    public float idleMin;
    public float idleMax;
    public float moveLengthMin;
    public float moveLengthMax;
    float moveLength;
    private float idleLength;
    private float idleTime;
    public Animator anim;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Rando(float speed, int direction, Vector2 pos)
    {
        if (needsNewBehaviour)
        {
            behaviour = ChooseBehaviour();
            needsNewBehaviour = false;
        }
        switch (behaviour)
        {
            case "move":
                if (!currentlyMoving)
                {
                    moveTime = 0;
                    moveLength = Random.Range(moveLengthMin, moveLengthMax);
                    // Debug.Log("Move vector: " + pos);

                    // Debug.Log("The pig started moving at " + Time.time);
                    currentlyMoving = true;
                }
                if (moveTime < moveLength)
                {
                    rb.MovePosition(pos);
                    // Debug.Log("pos to: " + pos);
                    if (vertical)
                    {
                        anim.SetFloat("X", 0);
                        anim.SetFloat("Y", direction);
                    }
                    else
                    {
                        anim.SetFloat("X", direction);
                        anim.SetFloat("Y", 0);
                    }
                    moveTime += Time.deltaTime;
                    anim.SetFloat("Speed", speed);
                }
                else
                {
                    // Debug.Log("The enemy finished moving at " + Time.time);
                    currentlyMoving = false;
                    needsNewBehaviour = true;
                    anim.SetFloat("Speed", 0.09f);
                }
                break;

            case "idle":

                if (!currentlyIdling)
                {
                    idleTime = 0;
                    idleLength = Random.Range(idleMin, idleMax);
                    // Debug.Log($"The enemy is going to start idling at {Time.time} for {idleLength} seconds.");
                    currentlyIdling = true;
                }

                if (idleTime < idleLength)
                {
                    idleTime += Time.deltaTime;
                    anim.SetFloat("Speed", 0.09f);

                }
                else
                {
                    // Debug.Log("The enemy finished idling at" + Time.time);
                    currentlyIdling = false;
                    needsNewBehaviour = true;
                }
                break;
        }
    }

    string ChooseBehaviour()
    {
        int behaviour = Random.Range(0, 2);
        switch (behaviour)
        {
            case 0:
                return "move";
            default:
                return "idle";
        }
    }
}
