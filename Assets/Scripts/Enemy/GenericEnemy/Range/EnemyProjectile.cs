using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float damage;
    public GameObject destructionEffect;

    PlayerController playerControllerScript;

    private void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void accelerateProjectile(float speed)
    {
        GetComponent<Rigidbody>().velocity = Vector3.Normalize(GameObject.FindGameObjectWithTag("Player").transform.position + Vector3.up * 1.5f - transform.position) * speed;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player")) collision.gameObject.GetComponent<PlayerHitPoints>().TakeDamage(damage);
        playerControllerScript.audioSource.PlayOneShot(playerControllerScript.rangeGhostOrbExplosionSFX);
        GameObject effect = Instantiate(destructionEffect);
        effect.transform.position = transform.position;
        Destroy(effect, 10f);
        Destroy(gameObject);
    }
}
