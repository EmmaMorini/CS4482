using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 5.0f;
<<<<<<< HEAD
    float maxHealth = 5;
    public float damageDelayTime = 2.0f;
    public float collisionOffset = 0.5f;
    float currentHealth;
=======
    public int maxHealth = 5;
    public float damageDelayTime = 2.0f;
    public float collisionOffset = 0.5f;
    int currentHealth;
    public int health { get { return currentHealth; } }
>>>>>>> emma-map-creation
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
    int limitProjectiles = 3;
    public GameObject[] projectileList {get; private set;}

    void Awake(){
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
<<<<<<< HEAD
        maxHealth = PlayerStats.MaxHealth;
        speed = PlayerStats.MoveSpeed;
        currentHealth = maxHealth;
=======
        currentHealth = maxHealth;
        animator.SetInteger("Health", maxHealth);
>>>>>>> emma-map-creation
        respawn = transform.position;
        _lastxpos = transform.position.x;
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
            animator.SetTrigger("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger("Melee");
            Attack();
        }
    }
    void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rb.MovePosition(position);

    }

<<<<<<< HEAD
    public float GetHealth(){
=======
    public int GetHealth(){
>>>>>>> emma-map-creation
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
<<<<<<< HEAD
=======
        else animator.SetInteger("Health", currentHealth);
>>>>>>> emma-map-creation
        Debug.Log(currentHealth + "/" + maxHealth);
        // HealthBar.instance.SetHealth((float)currentHealth / (float)maxHealth);
    }

    void Launch()
    {
        Vector2 xProjectile = Vector2.right;
        Debug.Log("lookdir.x = "+lookDir);
        if (lookDir.x < 0) xProjectile = xProjectile * -0.7f;
        else if (lookDir.x > 0) xProjectile = xProjectile * 0.7f;
        GameObject projectileObj = Instantiate(projectilePrefab, rb.position + xProjectile, Quaternion.identity);
        Projectiles projectile = projectileObj.GetComponent<Projectiles>();
        if (projectile){
            projectile.Launch(lookDir, 300, false);
        } 
        else Debug.Log("No projectile for this character");
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
            if (enemy) {
                Debug.Log("Enemy hit");
<<<<<<< HEAD
                enemy.ChangeHealth(PlayerStats.Damage);
=======
                enemy.ChangeHealth(-1);
>>>>>>> emma-map-creation
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
<<<<<<< HEAD
        animator.SetTrigger("Die");
        transform.position = respawn;
        currentHealth = maxHealth;
=======
        animator.SetInteger("Health", 0);
        transform.position = respawn;
        currentHealth = maxHealth;
        // HealthBar.instance.SetHealth(1);
>>>>>>> emma-map-creation
    }
}
