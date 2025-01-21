using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private float distance = 3f;
    [SerializeField] private LayerMask mask; // can change the name of the mask later maybe
    private PlayerUI playerUI;
    private PlayerController playerController;

    private Outline currentOutline; // Keep track of the currently active Outline

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerController>().cam;
        playerUI = GetComponent<PlayerUI>();
        playerController = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);

        // create a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo; // variable to store our collision information

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            // Check if the hit object has an Interactable component
            Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
            if (interactable != null)
            {
                playerUI.UpdateText(interactable.promptMessage);

                // Activate the Outline script on the interactable object
                Outline outline = hitInfo.collider.GetComponent<Outline>();
                if (outline != null && outline != currentOutline)
                {
                    // Deactivate the previous outline if different
                    if (currentOutline != null)
                    {
                        currentOutline.enabled = false;
                    }

                    // Activate the new outline
                    outline.enabled = true;
                    currentOutline = outline;
                }

                // Handle interaction
                if (playerController.input.Interact.triggered)
                {
                    playerController.audioSource.PlayOneShot(playerController.pickUpItemSound);
                    interactable.BaseInteract();
                }
            }
        }
        else
        {
            // If the raycast doesn't hit anything, deactivate the current outline
            if (currentOutline != null)
            {
                currentOutline.enabled = false;
                currentOutline = null;
            }
        }
    }
}
