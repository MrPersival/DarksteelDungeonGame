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
    //public AudioClip destroySFX;


    PlayerController playerControllerScript;
    //AudioSource audioSource;
    bool isFunctionCalled;
    float currentHitPoints;


    private void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        //audioSource = gameObject.GetComponent<AudioSource>();
        

    }

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
        //playerControllerScript.audioSource.PlayOneShot(playerControllerScript.destroyVase);
        DeathSoundSFX();
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
    
    void DeathSoundSFX()
    {
        if (gameObject.name == "Vase(Clone)")
        {
            playerControllerScript.audioSource.PlayOneShot(playerControllerScript.destroyVaseSFX);
        }
        else if (gameObject.name == "Crate(Clone)")
        {
            playerControllerScript.audioSource.PlayOneShot(playerControllerScript.destroyCrateSFX);
        }
        else if (gameObject.name == "Boss")
        {
            playerControllerScript.audioSource.PlayOneShot(playerControllerScript.bossDeathSFX);
        }
        else if (gameObject.name == "Character_Skeleton_Knight" || gameObject.name == "Character_Skeleton_Slave_01" || gameObject.name == "Character_Skeleton_Soldier_01" || gameObject.name == "MeleeSkeletonHeavy Variant(Clone)" || gameObject.name == "MeleeSkeletonLight Variant(Clone)" || gameObject.name == "MeleeSkeletonWarrior(Clone)")
        {
            float randomSound = Random.Range(1, 3);

            if (randomSound == 1)
            {
                playerControllerScript.audioSource.PlayOneShot(playerControllerScript.skeletonDeathSFX1);
            }
            else if (randomSound == 2)
            {
                playerControllerScript.audioSource.PlayOneShot(playerControllerScript.skeletonDeathSFX2);
            }
        }
        else if (gameObject.name == "Character_Ghost_01" || gameObject.name == "Character_Ghost_02" || gameObject.name == "RangeGhost(Clone)")
        {
            playerControllerScript.audioSource.volume = 0.55f;
            playerControllerScript.audioSource.PlayOneShot(playerControllerScript.ghostDeathSFX);
            playerControllerScript.audioSource.volume = 1f;
        }
    }
}

   
