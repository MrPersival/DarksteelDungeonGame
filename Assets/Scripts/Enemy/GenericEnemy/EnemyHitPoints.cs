using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHitPoints : MonoBehaviour
{
    float currentHitPoints;
    public float maxHitPoints;
    public GameObject deathEffect;

    void Start()
    {
        currentHitPoints = maxHitPoints;
    }
    private void Update()
    {
    }
    public void TakeDamage(float damage, Vector3 knockBackForce)
    {
        if(!GetComponent<Animator>().GetBool("isHit")) GetComponent<Animator>().SetBool("isHit", true);
        //Debug.Log("Dealing damage");
        currentHitPoints -= damage;
        if(currentHitPoints <= 0) Death();
        //if(!GetComponent<Animator>().GetBool("isHitAnimationPlaying")) StartCoroutine(knockBack(knockBackForce));
    }


    //Not working right now, sends enemy to flight
    IEnumerator knockBack(Vector3 knockBackForce)
    {
        yield return null;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(knockBackForce, ForceMode.Impulse);

        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() => rigidbody.velocity.magnitude < 0.05f);
        yield return new WaitForSeconds(0.25f);

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        agent.Warp(transform.position);
        agent.enabled = true;
    }

    void Death()
    {
        GameObject deathParticle = Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
        //deathEffect.transform.position = transform.position;
        Destroy(deathParticle, 20f);
        // TEMPORARY: Destroy Object
        Destroy(gameObject, 0.25f);
    }
}
