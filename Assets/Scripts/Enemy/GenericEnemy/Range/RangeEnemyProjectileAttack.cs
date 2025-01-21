using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyProjectileAttack : MonoBehaviour
{
    public GameObject Projectile;
    public float damage = 0f;
    public float speed = 0f;


    public void spawnProjectile()
    {
        EnemyProjectile projectile = Instantiate(Projectile, transform.position + Vector3.up * 2.5f, Quaternion.identity).GetComponent<EnemyProjectile>();
        //projectile.gameObject.GetComponent<Rigidbody>().velocity = Vector3.Normalize(GameObject.FindGameObjectWithTag("Player").transform.position - transform.position) * speed;
        projectile.damage = damage;
        projectile.accelerateProjectile(speed);
    }
}
