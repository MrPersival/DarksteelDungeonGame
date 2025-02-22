using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip bossMusic;
    public AudioClip startMenuMusic;
    public AudioClip normalMusic;
    public AudioClip deathMusic;

    private AudioSource audioSource;
    private GameObject bossLevel;
    private GameObject startMenu;
    private PlayerHitPoints playerHitPointsScript;
    private Scene currentScene;
    private bool deathMusicPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentScene = SceneManager.GetActiveScene();
        Debug.Log("Active scene: " + currentScene.name);
    }

    void Update()
    {
        if (currentScene.name == "MainMenu")
        {
            if (startMenu == null)
            {
                startMenu = GameObject.Find("Main Menu");
            }

            if (startMenu != null && startMenu.activeInHierarchy)
            {
                if (audioSource.clip != startMenuMusic || !audioSource.isPlaying) // plays the start menu music
                {
                    audioSource.clip = startMenuMusic;
                    audioSource.volume = 0.8f;
                    audioSource.Play();
                }
            }
        }
        else
        {
            playerHitPointsScript = GameObject.Find("Player").GetComponent<PlayerHitPoints>();

            if (!playerHitPointsScript.isDead)
            {
                if (bossLevel == null)
                {
                    bossLevel = GameObject.Find("BossLevel(Clone)");
                }

                if (bossLevel != null && bossLevel.activeInHierarchy)
                {
                    if (audioSource.clip != bossMusic || !audioSource.isPlaying) // plays the boss music
                    {
                        audioSource.clip = bossMusic;
                        audioSource.volume = 0.8f;
                        audioSource.Play();
                    }
                }
                else if (bossLevel == null || (bossLevel != null && !bossLevel.activeInHierarchy))
                {
                    //Debug.Log("Playing normal music..."); // Debugging line

                    if (audioSource.clip != normalMusic) // plays the normal music
                    {
                        audioSource.clip = normalMusic;
                        audioSource.volume = 0.6f;
                        audioSource.Play();
                    }
                }
                else
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
            }
            else
            {
                if (!deathMusicPlayed)
                {
                    audioSource.Stop();
                    audioSource.clip = deathMusic;
                    audioSource.loop = false;
                    audioSource.volume = 0.8f;
                    audioSource.Play();
                    deathMusicPlayed = true;
                }
            }
        }
    }
}
