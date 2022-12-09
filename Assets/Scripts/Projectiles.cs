using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D cld;
    public Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cld = GetComponent<Collider2D>();
        // Debug.Log(rb);
        // anim = GetComponent<Animator> ();
    }

    void Start(){
        StartCoroutine(SelfDestruct());
    }

    void Update()
    {
        // Debug.Log("sleeping: "+sleeping);
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile") || other.gameObject.CompareTag("EnemyProjectile"))
        {
            Physics2D.IgnoreCollision(other.collider, cld);
            Debug.Log("Ignore " + other.gameObject.tag);
        }
        else Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile Trigger with " + other.gameObject);
        if (other.CompareTag("Hitbox"))
        {
            if (anim) anim.SetTrigger("Hit");
            Debug.Log("Hit on");
            rb.velocity = Vector2.zero;
            EnemyController e = other.gameObject.transform.parent.GetComponent<EnemyController>();
            PlayerController p = other.gameObject.transform.parent.GetComponent<PlayerController>();
            if (e)
            {
                Debug.Log("hit the enemy hitbox, parent = " + e);
                e.ChangeHealth(PlayerStats.Damage);
                if (!anim) Destroy(gameObject);
            }
            else if (p)
            {
                Debug.Log("hit the player hitbox, parent = " + p);
                p.ChangeHealth(-1);
                if (!anim) Destroy(gameObject);
            }
        }
    }

        IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    public void Launch(Vector2 direction, float force, bool vertical)
    {
        if (anim){
            if (direction.x > 0) anim.SetFloat("Look X", 1);
            else if (direction.x < 0) anim.SetFloat("Look X", -1);
            anim.SetTrigger("Shoot");
            // Debug.Log("Shoot on");            
        }
        if (vertical){
            rb.AddForce(Vector2.up * direction.y * force);
        }
        else rb.AddForce(Vector2.right * direction.x * force);
    }
}
