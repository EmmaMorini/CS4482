using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    Rigidbody2D rb;
    float direction;
    EnemyRandomShooting randoShoot;
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

<<<<<<< HEAD
    public void Launch(bool active)
    {
        if (active){            
            GameObject projectileObj = Instantiate(projectilePrefab, rb.position + Vector2.down * 0.5f, Quaternion.identity);
            Projectiles projectile = projectileObj.GetComponent<Projectiles>();
            if (projectile){
                projectile.Launch(Vector2.down, 300, true);
            }
            else Debug.Log("No projectile for this character");
        }
        else return;
=======
    public void Launch()
    {
        GameObject projectileObj = Instantiate(projectilePrefab, rb.position + Vector2.down * 0.5f, Quaternion.identity);
        Projectiles projectile = projectileObj.GetComponent<Projectiles>();
        if (projectile){
            projectile.Launch(Vector2.down, 300, true);
        }
        else Debug.Log("No projectile for this character");
        // animator.SetTrigger("Launch");
>>>>>>> emma-map-creation
    }
}
