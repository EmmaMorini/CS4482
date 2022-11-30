using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    Rigidbody2D rb;
    float direction;
    EnemyRandomShooting randoShoot;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        randoShoot = GetComponent<EnemyRandomShooting>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        randoShoot.Rando(-1,Vector2.down);
    }

    public void Launch()
    {
        GameObject projectileObj = Instantiate(projectilePrefab, rb.position + Vector2.up * -0.7f, Quaternion.identity);
        Projectiles projectile = projectileObj.GetComponent<Projectiles>();
        if (projectile){
            projectile.Launch(true,Vector2.down, 300);
        }
        else Debug.Log("No projectile for this character");
        // animator.SetTrigger("Launch");
    }
}
