using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAdditionalEnemyState : StateMachineBehaviour
{
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public float delayBetweenSpawning = 2f;
    public GameObject spawnerEffect;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isSpawningEnemies", false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("isSpawnedEnemies", true);
        //animator.gameObject.GetComponent<SpawnEnemiesBehind>().spawnEnemies();
    }
}