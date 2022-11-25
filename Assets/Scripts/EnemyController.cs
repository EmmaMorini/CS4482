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
    int damage;
    public int maxDamage = 5;
    Animator animator;
    bool broken = true;
    int direction = 1;
    Vector2 pos;
    EnemyRandomMovement randoMove;
    EnemyRandomMeleeAttack randoMelle;
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
        pos = transform.position;
        damage = maxDamage;
        animator.SetInteger("Health", maxDamage);
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
        ChangePos();
    }
    void FixedUpdate()
    {
        if (!broken) return;
        // if (TryGetComponent(out RandomSpawn bulletSpawn)) bulletSpawn.Rando(direction, pos);
        randoMove.Rando(speed, direction, pos);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // RubyController player = other.gameObject.GetComponent<RubyController>();
        // if (player != null)
        // {
        //     Debug.Log("Collision");
        //     player.ChangeHealth(-1);
        // }

        if (other.gameObject.CompareTag("Map"))
        {
            randoMove.Rando(speed, direction, pos);
        }
    }

    public void ChangeDamage(int amt)
    {
        if (amt < 0)
        {
            animator.SetTrigger("Hit");
            if (isBoostDelay) return;
            isBoostDelay = true;
            boostDelay = boostDelayTime;
        }
        damage = Mathf.Clamp(damage + amt, 0, maxDamage);
        animator.SetInteger("Health", damage);
        Debug.Log(damage + "/" + maxDamage);
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Fix()
    {
        broken = false;
        rb.simulated = false;
        animator.SetBool("broken", false);
        if (smokeEffect) smokeEffect.Stop();
    }

    public void OnHit(int damage, Vector2 direction)
    {
        Debug.Log("got hit");
        rb.AddForce(direction, ForceMode2D.Force);
        ChangeDamage(damage);
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
        Debug.Log("attackpoint: " + attackPoints.localPosition);
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
            EnemyController enemy = e.GetComponent<EnemyController>();
            if (enemy) {
                Debug.Log("Enemy hit");
                enemy.ChangeDamage(-1);
            }
            else Debug.Log("No enemy");
        }
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