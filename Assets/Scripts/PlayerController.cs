using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 5.0f;
    float maxHealth = 5;
    public float damageDelayTime = 2.0f;
    public float collisionOffset = 0.5f;
    float currentHealth;
    Rigidbody2D rb;
    float horizontal;
    float vertical;
    bool isDamageDelay;
    float damageDelay;
    Animator animator;
    Vector2 lookDir = new Vector2(1, 0);
    Vector2 move;
    // public SwordBehaviour melee;

    [SerializeField]
    public GameObject projectilePrefab1, projectilePrefab2;
    public GameObject projectilePrefab;
    Vector2 respawn;
    Collider2D cld;

    public Transform attackPoints;
    public float attackRange;
    public LayerMask enemyLayers;
    float _lastxpos;
    // int limitProjectiles = 3;
    int shootingcase = -1;
    Projectiles projectile1;
    Projectiles projectile2;
    public GameObject[] projectileList { get; private set; }

    void Awake()
    {
        projectileList = new GameObject[]{
            projectilePrefab1,projectilePrefab2
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 10;
        rb = GetComponent<Rigidbody2D>();
        cld = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        maxHealth = PlayerStats.MaxHealth;
        speed = PlayerStats.MoveSpeed;
        currentHealth = maxHealth;
        respawn = transform.position;
        _lastxpos = transform.position.x;

        // projectile1 = projectilePrefab1.GetComponent<Projectiles>();
        // projectile2 = projectilePrefab2.GetComponent<Projectiles>();
    }

    private void OnEnable()
    {
        ChaliceCollectible.OnChaliceCollected += AddHealth;
    }

    private void OnDisable()
    {
        ChaliceCollectible.OnChaliceCollected -= AddHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        move = new Vector2(horizontal, vertical);
        // Debug.Log("Player move: "+move);

        if (!Mathf.Approximately(move.x, 0.0f))
        {
            lookDir.Set(move.x, move.y);
            lookDir.Normalize();
            _lastxpos = lookDir.x;
        }
        else if (!Mathf.Approximately(move.y, 0.0f))
        {
            lookDir.Set(_lastxpos, move.y);
            lookDir.Normalize();
        }

        // Debug.Log("lookDir: " + lookDir);

        if (isDamageDelay)
        {
            damageDelay -= Time.deltaTime;
            if (damageDelay < 0) isDamageDelay = false;
        }

        if (lookDir.x > 0) animator.SetFloat("Look X", 1);
        else if (lookDir.x < 0) animator.SetFloat("Look X", -1);
        animator.SetFloat("Speed", move.magnitude);

        if (Input.GetKeyDown(KeyCode.C))
        {
            shootingcase = 0;
            animator.SetTrigger("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Melee");
            Attack();
        }

        if (projectilePrefab1)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            { 
                shootingcase = 1;
                Debug.Log("Q down");
                animator.SetTrigger("Shoot");
            }
        }

        if (projectilePrefab2)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                shootingcase = 2;
                Debug.Log("E down");
                animator.SetTrigger("Shoot");
            }
        }

    }
    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rb.MovePosition(position);

    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void ChangeHealth(int amt)
    {
        if (amt < 0)
        {
            animator.SetTrigger("Hurt");
            if (isDamageDelay) return;
            isDamageDelay = true;
            damageDelay = damageDelayTime;
        }
        currentHealth = Mathf.Clamp(currentHealth + amt, 0, maxHealth);
        if (currentHealth == 0) Die();
        Debug.Log(currentHealth + "/" + maxHealth);
        HealthBar.instance.SetHealth((float)currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        Vector2 xProjectile = Vector2.right;
        Debug.Log("lookdir.x = " + lookDir);
        if (lookDir.x < 0) xProjectile = xProjectile * -0.7f;
        else if (lookDir.x > 0) xProjectile = xProjectile * 0.7f;
        GameObject projectileObj;
        Projectiles projectile;
        
        switch(shootingcase){
            case 0:
                projectileObj = Instantiate(projectilePrefab, rb.position + xProjectile, Quaternion.identity);
                projectile = projectileObj.GetComponent<Projectiles>();
                if (projectile)
                {
                    projectile.Launch(lookDir, 300, false);
                }
                break;
            case 1:
                projectileObj = Instantiate(projectilePrefab1, rb.position + xProjectile, Quaternion.identity);
                projectile = projectileObj.GetComponent<Projectiles>();
                if (projectile)
                {
                    projectile.Launch(lookDir, 300, false);
                }
                break;
            case 2:
                projectileObj = Instantiate(projectilePrefab2, rb.position + xProjectile, Quaternion.identity);
                projectile = projectileObj.GetComponent<Projectiles>();
                if (projectile)
                {
                    projectile.Launch(lookDir, 300, false);
                }
                break;
            default:
                Debug.Log("No projectile for this character");
                break;
        }
        // animator.SetTrigger("Launch");
    }

    public void Attack()
    {
        Vector2 transition = attackPoints.localPosition;
        Debug.Log("attackpoint: " + attackPoints.localPosition);
        if ((lookDir.x > 0 && attackPoints.localPosition.x < 0) || (lookDir.x < 0 && attackPoints.localPosition.x > 0))
        {
            Debug.Log("change attackpoint");
            transition.x = transition.x * -1;
        }
        attackPoints.localPosition = transition;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoints.position, attackRange, enemyLayers);
        foreach (Collider2D e in hitEnemies)
        {
            Debug.Log(e);
            EnemyController enemy = e.GetComponent<EnemyController>();
            if (enemy)
            {
                Debug.Log("Enemy hit");
                enemy.ChangeHealth(PlayerStats.Damage);
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

    public void Die()
    {
        animator.SetTrigger("Die");
        transform.position = respawn;
        currentHealth = maxHealth;
    }

    public void AddHealth()
    {
        currentHealth = maxHealth;
        Debug.Log("Cureent health restored to max:" + currentHealth);
    }
}
