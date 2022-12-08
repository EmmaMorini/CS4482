using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    bool vertical;
    Rigidbody2D rb;
    public ParticleSystem smokeEffect;
    public float changeTime = 3.0f;
    float timer;
    bool isBoostDelay;
    float boostDelayTime = 2.0f;
    float boostDelay;
    float health;
    public float maxHealth = 5;
    Animator animator;
    bool broken = true;
    int direction = 1;
    Vector2 pos;
    EnemyRandomMovement randoMove;
    EnemyRandomMeleeAttack randoMelle;
    EnemyRandomShooting randoShooting;
    Collider2D cld;
    public GameObject projectilePrefab;
    public Transform attackPoints;
    public float attackRange;
    public LayerMask playerLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cld = GetComponent<Collider2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        randoMove = GetComponent<EnemyRandomMovement>();
        randoMelle = GetComponent<EnemyRandomMeleeAttack>();
        randoShooting = GetComponent<EnemyRandomShooting>();
        pos = transform.position;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken) return;
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
        if (isBoostDelay)
        {
            boostDelay -= Time.deltaTime;
            if (boostDelay < 0) isBoostDelay = false;
        }
        ChangePos();
    }
    void FixedUpdate()
    {
        if (!broken) return;
        if (randoMelle) randoMelle.Rando(direction, pos);
        if (randoShooting) randoShooting.Rando(direction, pos);
        randoMove.Rando(speed, direction, pos);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            Debug.Log("Collision");
            player.ChangeHealth(-1);
        }

        // if (other.gameObject.CompareTag("Map"))
        // {
        // randoMove.Rando(speed, direction, pos);
        // }
    }

    public void ChangeHealth(float amt)
    {
        if (amt < 0)
        {
            Debug.Log("health lost: "+amt);
            animator.SetTrigger("GotHit");
            if (isBoostDelay) return;
            isBoostDelay = true;
            boostDelay = boostDelayTime;
        }
        health = Mathf.Clamp(health + amt, 0, maxHealth);
        if (health == 0) Die();
        Debug.Log(health + "/" + maxHealth);
    }

    public float GetHealth()
    {
        return health;
    }

    public void Die()
    {
        broken = false;
        rb.simulated = false;
        if (smokeEffect) smokeEffect.Stop();
        animator.SetTrigger("Die");
    }

    public void OnHit(int health, Vector2 direction)
    {
        Debug.Log("got hit");
        rb.AddForce(direction, ForceMode2D.Force);
        ChangeHealth(health);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    void ChangePos()
    {
        pos = rb.position;
        vertical = Random.value > 0.5;
        if (vertical)
        {
           pos.y = pos.y + Time.deltaTime * speed * direction;
        }
        else
        {
           pos.x = pos.x + Time.deltaTime * speed * direction;
        }
    }

    public void Attack()
    {
        Vector2 lookDir = new Vector2(1, 0);
        lookDir.x = lookDir.x * direction;
        Vector2 transition = attackPoints.localPosition;
        if ((lookDir.x > 0 && attackPoints.localPosition.x < 0) || (lookDir.x < 0 && attackPoints.localPosition.x > 0))
        {
            Debug.Log("change attackpoint");
            transition.x = transition.x * -1;
        }
        attackPoints.localPosition = transition;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoints.position, attackRange, playerLayer);
        foreach (Collider2D e in hitEnemies)
        {
            Debug.Log(e);
            PlayerController player = e.GetComponent<PlayerController>();
            if (player) {
                Debug.Log("Enemy hit");
                player.ChangeHealth(-1);
            }
            else Debug.Log("No enemy");
        }
    }

    void Launch()
    {
        GameObject projectileObj = Instantiate(projectilePrefab, rb.position + Vector2.up * 0.2f, Quaternion.identity);
        Projectiles projectile = projectileObj.GetComponent<Projectiles>();
        if (projectile){
            Vector2 lookDir = new Vector2(1, 0);
            lookDir.x = lookDir.x * direction;
            projectile.Launch(lookDir, 300, false);
        }
        else Debug.Log("No projectile for this character");
        // animator.SetTrigger("Launch");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoints == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoints.position, attackRange);
    }
}