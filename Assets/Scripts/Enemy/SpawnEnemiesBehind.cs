using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesBehind : MonoBehaviour
{
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public float delayBetweenSpawning = 2f;
    public GameObject spawnerEffect;


    public void spawnEnemies()
    {
        GetComponent<Animator>().SetBool("isSpawningEnemies", true);
        Vector3 positionToSpawn = transform.position + Vector3.back * 2;
        GameObject spawner =  Instantiate(spawnerEffect, positionToSpawn, Quaternion.identity);
        float delay = delayBetweenSpawning;
        foreach (GameObject enemy in enemiesToSpawn)
        {
            StartCoroutine(spawnEnemyWithDelay(enemy, delay, positionToSpawn));
            delay += delayBetweenSpawning;
        }
        Destroy(spawner, delay + delayBetweenSpawning);
    }

    IEnumerator spawnEnemyWithDelay(GameObject enemy, float delay, Vector3 position)
    {
        yield return new WaitForSeconds(delay);
        Instantiate(enemy, position, Quaternion.identity);
    }
}
