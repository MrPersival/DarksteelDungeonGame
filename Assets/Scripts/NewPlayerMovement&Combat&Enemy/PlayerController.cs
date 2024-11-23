using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.MainActions input;

    CharacterController controller;
    Animator animator;
    AudioSource audioSource;

    [Header("Controller")]
    public float walkSpeed = 5;
    public float sprintSpeed = 8;
    //public float moveSpeed = 5;
    public float gravity = -9.82f;
    public float jumpHeight = 1.2f;
    private bool isSprinting;
    private bool isTeleporting = false; // Flag to check if teleporting

    public float dodgeDistance = 5f;     // Distance covered in a dodge
    public float dodgeDuration = 0.2f;  // Time it takes to complete the dodge
    public float dodgeCooldown = 1f;    // Time before another dodge can be performed
    private bool dodgeInputHeld = false;  // Tracks if the dodge key is held

    private bool isDodging = false;     // Flag to track if the player is dodging
    private bool canDodge = true;       // Cooldown state

    // Add a variable to track if Sprint is being pressed
    private bool isSprintButtonPressed = false;

    //private Vector2 moveInput;
    GameObject enterPoint;

    Vector3 _PlayerVelocity;

    bool isGrounded;

    [Header("Camera")]
    public Camera cam;
    public float sensitivity;

    float xRotation = 0f;

    void Awake()
    { 
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();


        playerInput = new PlayerInput();
        input = playerInput.Main;
        AssignInputs();

        // Subscribe to sprint input.


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // Track if the dodge key is pressed
        dodgeInputHeld = input.Dodge.IsPressed();

        // Check if both dodge and movement inputs are valid
        Dodge();

        // Repeat Inputs
        if (input.Attack.IsPressed())
        { Attack(); }

        SetAnimations();

        if (Input.GetKeyDown(KeyCode.P))
        {
            isTeleporting = true; // Set teleport flag
            enterPoint = GameObject.Find("EnterPoint(Clone)");
            gameObject.transform.position = enterPoint.transform.position + new Vector3(0, 2, 0);
        }
    }

    void FixedUpdate() 
    { MoveInput(input.Movement.ReadValue<Vector2>()); }

    void LateUpdate() 
    { LookInput(input.Look.ReadValue<Vector2>()); }

    void MoveInput(Vector2 input)
    {
        if (isTeleporting)
        {
            isTeleporting = false;
            return;
        }

        // Calculate the movement direction
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);

        // Check if sprinting conditions are met
        if (isSprintButtonPressed)
        {
            // Allow sprinting when moving forward or diagonally forward
            if (input.y > 0 && input.magnitude >= 0.5f) // Magnitude check allows diagonal movement
            {
                isSprinting = true;
            }
            else
            {
                isSprinting = false;
            }
        }
        else
        {
            isSprinting = false; // Not sprinting if sprint button isn't pressed
        }

        // Determine the movement speed
        float speed = isSprinting ? sprintSpeed : walkSpeed;

        // Apply gravity and move the player
        _PlayerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && _PlayerVelocity.y < 0)
            _PlayerVelocity.y = -2f;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        controller.Move(_PlayerVelocity * Time.deltaTime);
    }

    void LookInput(Vector3 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime * sensitivity);
        xRotation = Mathf.Clamp(xRotation, -80, 80);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime * sensitivity));
    }

    void OnEnable() 
    { input.Enable(); }

    void OnDisable()
    { input.Disable(); }

    //void Jump()
    //{
    // Adds force to the player rigidbody to jump
    // if (isGrounded)
    //_PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
    // }

    public void Sprint(bool isPressed)
    {
        isSprinting = isPressed; // Set the sprint state based on Shift key press
    }

    void Dodge()
    {
        if (!canDodge || isDodging) return;

        Vector2 moveInput = input.Movement.ReadValue<Vector2>();

        // Only trigger dodge if both dodge key is held and there's valid movement input
        if (dodgeInputHeld && moveInput != Vector2.zero)
        {
            // Determine the dodge direction
            Vector3 dodgeDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            dodgeDirection = transform.TransformDirection(dodgeDirection);

            // Start the dodge
            StartCoroutine(PerformDodge(dodgeDirection));
        }
    }




    IEnumerator PerformDodge(Vector3 direction)
    {
        canDodge = false;  // Start cooldown
        isDodging = true;

        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;

        // Calculate the dodge target position
        Vector3 targetPosition = initialPosition + direction * dodgeDistance;

        // Smoothly move towards the target over dodgeDuration
        while (elapsedTime < dodgeDuration)
        {
            controller.Move(direction * (dodgeDistance / dodgeDuration) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isDodging = false;

        // Cooldown before another dodge can occur
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }



    void AssignInputs()
    {
        input.Attack.started += ctx => Attack();

        // Update sprint button press state
        input.Sprint.performed += ctx => isSprintButtonPressed = true; // Shift key pressed
        input.Sprint.canceled += ctx => isSprintButtonPressed = false; // Shift key released
        input.Dodge.performed += ctx => Dodge();
    }



    // ---------- //
    // ANIMATIONS //
    // ---------- //

    public const string IDLE = "Idle";
    public const string WALK = "Walk";
    public const string RUN = "Run";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";

    string currentAnimationState;


    public void ChangeAnimationState(string newState) 
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;

        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);

    }

    void SetAnimations()
    {
        // If player is not attacking
        if (!attacking)
        {
            if(!isSprinting)
            { 
                ChangeAnimationState(IDLE);
                
            }
            
            else if (isSprinting)
            {
                ChangeAnimationState(RUN); // Fix one problem, you can hold shift and then take one step. If you then stop, the running animation will then continue to play until you don't hold shift anymore
               
            } // VERY VERY LOW PRIORETY: If the player holds shift, then exists out and then goes back in (while still holding shift), then the player will run when not holding shift and walk when holding shift
        }
    }

    // ------------------- //
    // ATTACKING BEHAVIOUR //
    // ------------------- //

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;
    public float enemyThrowBackForce = 100f;

    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public void Attack()
    {
        if(!readyToAttack || attacking) return;

        if (!isSprinting) 
        {
            readyToAttack = false;
            attacking = true;

            Invoke(nameof(ResetAttack), attackSpeed);
            Invoke(nameof(AttackRaycast), attackDelay);

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(swordSwing);
        }

        if(attackCount == 0 && !isSprinting)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else if (attackCount == 1 && !isSprinting)
        {
            ChangeAnimationState(ATTACK2);
            attackCount = 0;
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        Debug.Log("Starting raycast");
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer) && !isSprinting)
        { 
            Debug.Log(hit.collider.name);
            HitTarget(hit.point);
            Vector3 knockbackForce = transform.forward * enemyThrowBackForce;
            if(hit.transform.TryGetComponent<EnemyHitPoints>(out EnemyHitPoints T)) T.TakeDamage(attackDamage, knockbackForce);

        }
    }

    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
}
