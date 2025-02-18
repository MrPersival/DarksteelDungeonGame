using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesBehind : MonoBehaviour
{
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    public float delayBetweenSpawning = 2f;
    public GameObject spawnerEffect;
    public AudioClip summonSFX;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void spawnEnemies()
    {
        GetComponent<Animator>().SetBool("isSpawningEnemies", true);
        Vector3 positionToSpawn = transform.position + Vector3.back * 2;
        GameObject spawner =  Instantiate(spawnerEffect, positionToSpawn, Quaternion.identity);
        ChangeSong(summonSFX);
        StartCoroutine(StopsummonSFX());
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

    IEnumerator StopsummonSFX()
    {
        yield return new WaitForSeconds(delayBetweenSpawning * enemiesToSpawn.Count + 4f);
        audioSource.Stop();

    }


    // Method to change the song
    public void ChangeSong(AudioClip newClip)
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is not assigned!");
            return;
        }

        if (newClip == null)
        {
            Debug.LogWarning("New AudioClip is null!");
            return;
        }

        audioSource.Stop();          // Stop current song
        audioSource.clip = newClip;  // Assign the new song
        audioSource.Play();          // Play the new song
    }
}


