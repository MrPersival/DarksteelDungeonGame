using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyHitPoints : MonoBehaviour
{
    public float maxHitPoints;
    public GameObject deathEffect;
    public float procentOfHpLeftToCalllFunction;
    public UnityEvent functionToCall;
    public GameObject objectToDeleteOnDeath;

    bool isFunctionCalled;
    float currentHitPoints;


    private void Update()
    {

    }
    public void TakeDamage(float damage, Vector3 knockBackForce)
    {
        if(TryGetComponent<Animator>(out Animator animator)) if (!animator.GetBool("isHit")) animator.SetBool("isHit", true);
        //Debug.Log("Dealing damage");
        currentHitPoints -= damage;
        if(currentHitPoints <= 0) Death();
        else if(functionToCall != null && procentOfHpLeftToCalllFunction != 0 && !isFunctionCalled && procentOfHpLeftToCalllFunction >= currentHitPoints / maxHitPoints * 100)
        {
            Debug.Log("Spawning enemies");
            functionToCall.Invoke();
            isFunctionCalled = true;
        }
        //if(!GetComponent<Animator>().GetBool("isHitAnimationPlaying")) StartCoroutine(knockBack(knockBackForce));
    }


    //Not working right now, sends enemy to flight
    /* IEnumerator knockBack(Vector3 knockBackForce)
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
    } */

    void Death()
    {
        GameObject deathParticle = Instantiate(deathEffect, transform.position, deathEffect.transform.rotation);
        //deathEffect.transform.position = transform.position;
        if (gameObject.TryGetComponent<ItemDrop>(out ItemDrop itemDrop)) itemDrop.dropItem();
        if (gameObject.TryGetComponent<XPDrop>(out XPDrop xPDrop)) xPDrop.giveXP();
        Destroy(deathParticle, 20f);
        // TEMPORARY: Destroy Object
        Destroy(gameObject, 0.25f);
        if (objectToDeleteOnDeath != null) Destroy(objectToDeleteOnDeath);
    }

    public void setmaxAndCurrentHp(float hp)
    {
        maxHitPoints = hp;
        currentHitPoints = hp;
    }
}
