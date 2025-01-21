using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource movingAudioSource;
    public AudioClip movingSound;
    private PlayerController playerControllerScript;

    private bool isPlaying = false; // Track if audio is currently playing

    void Start()
    {
        playerControllerScript = GetComponent<PlayerController>();
        // Assign the clip if not set directly on the AudioSource
        if (movingAudioSource.clip == null && movingSound != null)
        {
            movingAudioSource.clip = movingSound;
        }

        // Ensure looping is enabled for continuous playback
        movingAudioSource.loop = true;
    }

    void Update()
    {
        // Check if movement keys are pressed
        bool isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving)
        {
            // Adjust pitch based on Left Shift
            if (playerControllerScript.isSprinting)
            {
                movingAudioSource.pitch = 2f; // Running pitch
            }
            else
            {
                movingAudioSource.pitch = 1f; // Walking pitch
            }

            // Start playing if not already playing
            if (!isPlaying)
            {
                movingAudioSource.Play();
                isPlaying = true;
            }
        }
        else
        {
            // Stop audio if movement stops
            if (isPlaying)
            {
                movingAudioSource.Stop();
                isPlaying = false;
            }
        }
    }
}

// Remember to fix so the sound does not play when is not on ground