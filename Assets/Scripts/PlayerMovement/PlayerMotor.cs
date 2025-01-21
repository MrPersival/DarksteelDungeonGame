using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    private bool isGrounded;
    public float gravity = -9.82f;
    private bool isTeleporting = false; // Flag to check if teleporting
    GameObject enterPoint;
    private bool sprinting;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (Input.GetKeyDown(KeyCode.P))
        {
            isTeleporting = true; // Set teleport flag
            enterPoint = GameObject.Find("EnterPoint(Clone)");
            gameObject.transform.position = enterPoint.transform.position + new Vector3(0, 2, 0);
        }
    }

    public void ProcessMove(Vector2 input) // recieve the inputs for our InputManager.cs and apply them to our character controller
    {
        if (isTeleporting)
        {
            isTeleporting = false; // Reset teleport flag
            return;
        }
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed *  Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8f;
        }
        else
        {
            speed = 5f;
        }
    }
}
