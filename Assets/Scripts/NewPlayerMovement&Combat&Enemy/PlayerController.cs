using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.MainActions input;
    public PlayerTutorial playerTutorial;

    CharacterController controller;
    private InventoryItem inventoryItemScript;
    Animator animator;

    public AudioSource audioSource;

    [Header("SFX")]
    public AudioClip dodgeSound;
    public AudioClip drinkPotionSound;
    public AudioClip pickUpItemSound;
    public AudioClip levelUpSound;

    [Header("Controller")]
    public float walkSpeed = 5;
    public float sprintSpeed = 8;
    //public float moveSpeed = 5;
    public float gravity = -9.82f;
    public float jumpHeight = 1.2f;
    public bool isSprinting;
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

    AttributesSystem attributesSystem;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inventoryItemScript = GetComponent<InventoryItem>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        attributesSystem = GetComponent<AttributesSystem>();


        playerInput = new PlayerInput();
        input = playerInput.Main;
        AssignInputs();

        // Subscribe to sprint input.


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    private void Start()
    {
        isTeleporting = true; // Set teleport flag
        enterPoint = GameObject.Find("EnterPoint(Clone)");
        teleport(enterPoint.transform.position);
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // Track if the dodge key is pressed
        dodgeInputHeld = input.Dodge.IsPressed();

        // Check if both dodge and movement inputs are valid
        Dodge();

        // Repeat Inputs
        // if (input.Attack.IsPressed())
        // { Attack(); }

        SetAnimations();

        /* if (Input.GetKeyDown(KeyCode.P))
        {
            //isTeleporting = true; // Set teleport flag
            enterPoint = GameObject.Find("EnterPoint(Clone)");
            teleport(enterPoint.transform.position);
        } */

        if(Input.GetMouseButton(0))
        {
            holdingLMBSeconds += Time.deltaTime;
            if(holdingLMBSeconds > 0.5)
            {
                heavyAttackIndicatorSlider.gameObject.SetActive(true);
                heavyAttackIndicatorSlider.value = holdingLMBSeconds;
                heavyAttackIndicatorSlider.maxValue = holdUntilHeavyAttack;
            }
        }
        if(Input.GetMouseButtonUp(0) || isHeavyAttack)
        {
            holdingLMBSeconds = 0;
            heavyAttackIndicatorSlider.gameObject.SetActive(false);
        }
    }


    void FixedUpdate()
    { MoveInput(input.Movement.ReadValue<Vector2>()); }

    void LateUpdate()
    { LookInput(input.Look.ReadValue<Vector2>()); }

    public void teleport(Vector3 position)
    {
        isTeleporting = true; // Set teleport flag
        controller.Move(Vector3.zero);
        gameObject.transform.position = position;
    }

    void MoveInput(Vector2 input)
    {
        if (isTeleporting)
        {
            isTeleporting = false;
            return;
        }

        // Calculate the movement direction
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        if (moveDirection != Vector3.zero) playerTutorial.playerMoved();

        // Check if sprinting conditions are met
        if (isSprintButtonPressed)
        {
            playerTutorial.playerSprintet();
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
        float speed = isSprinting ? attributesSystem.playerSprintSpeed : attributesSystem.playerMoveSpeed;


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
            playerTutorial.playerDodged();
            // Start the dodge
            StartCoroutine(PerformDodge(dodgeDirection));
            audioSource.PlayOneShot(dodgeSound);
        }
    }




    IEnumerator PerformDodge(Vector3 direction)
    {
        canDodge = false;  // Start cooldown
        isDodging = true;

        float elapsedTime = 0f;
        Vector3 initialPosition = transform.position;

        // Calculate the dodge target position
        Vector3 targetPosition = initialPosition + direction * attributesSystem.playerDodgeDistance;

        // Smoothly move towards the target over dodgeDuration
        while (elapsedTime < dodgeDuration)
        {
            controller.Move(direction * (attributesSystem.playerDodgeDistance / dodgeDuration) * Time.deltaTime);
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
        // input.Attack.started += ctx => Attack();
        input.Attack.performed += OnPress;
        input.Attack.canceled += OnRelease;

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
    public const string DODGE = "Dodge";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";
    public const string HEAVY_ATTACK = "Heavy Attack";

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
            if (isDodging)
            {
                ChangeAnimationState(DODGE);
            }

            if (!isSprinting && !isDodging)
            {
                ChangeAnimationState(IDLE);

            }

            else if (isSprinting && !isDodging)
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
    public float attackDamage = 1;
    public LayerMask attackLayer;
    public float enemyThrowBackForce = 100f;
    public float holdUntilHeavyAttack = 2.5f;

    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;
    //public AudioClip heavySound; If we have a special attacking sound for the heavy
    public Slider heavyAttackIndicatorSlider;

    private float pressTime;
    private float releaseTime;
    private bool buttonPressed = false;  // To track if the button is currently being held down.
    private bool isHeavyAttack = false;
    private float time_until_heavy_hits = 0.6f;
    private Coroutine holdMonitorCoroutine;
    private float holdingLMBSeconds;

    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public void Attack(float holdDuration)
    {
        if (!readyToAttack || attacking || Time.timeScale == 0f) return;

        if (!isSprinting)
        {
            readyToAttack = false;
            attacking = true;

            // Determine if the attack is heavy or normal
            float finalDamage = attackDamage;
            if (holdDuration >= holdUntilHeavyAttack) // Heavy attack condition
            {
                playerTutorial.playerHeavyAttacked();
                isHeavyAttack = true;
                ChangeAnimationState(HEAVY_ATTACK);
                Debug.Log("Heavy Attack! Double damage");
                StartCoroutine(HeavyAttackSound(0.4f));
                StartCoroutine(HeavyAttackDelay(finalDamage * 2, time_until_heavy_hits));
            }
            else
            {
                playerTutorial.playerAttacked();
                Debug.Log("Normal Attack");
                isHeavyAttack = false;
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(swordSwing);
                if (attackCount == 0)
                {
                    ChangeAnimationState(ATTACK1);
                    attackCount++;
                }
                else if (attackCount == 1)
                {
                    ChangeAnimationState(ATTACK2);
                    attackCount = 0;
                }

                // Set up normal attack with delay logic
                StartCoroutine(DelayedAttack(finalDamage, attackDelay));
            }

            // Reset attack cooldown
            Invoke(nameof(ResetAttack), attackSpeed);

        }
    }

    private IEnumerator DelayedAttack(float damage, float delay)
    {
        if (!isHeavyAttack) // Only delay normal attacks
        {
            yield return new WaitForSeconds(delay);
            AttackRaycast(damage); // Perform the attack immediately for normal attacks
        }
    }

    private IEnumerator HeavyAttackSound(float time_until_sound)
    {
        yield return new WaitForSeconds(time_until_sound); // Wait 0.4 seconds until high pitched sound is played
        audioSource.pitch = 1.5f;
        audioSource.PlayOneShot(swordSwing);

    }

    private IEnumerator HeavyAttackDelay(float damage, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay (e.g., 0.12 seconds)
        AttackRaycast(damage); // Perform the heavy attack
    }

    private IEnumerator MonitorHoldDuration()
    {
        yield return new WaitForSeconds(holdUntilHeavyAttack);

        if (buttonPressed) // Only trigger if still holding the button
        {
            buttonPressed = false; // Simulate button release
            releaseTime = Time.time; // Record release time
            //Debug.Log("Auto-triggering heavy attack!");
            Attack(holdUntilHeavyAttack); // Trigger the heavy attack
        }
    }



    private void OnPress(InputAction.CallbackContext context)
    {
        if (context.performed && !buttonPressed) // Ensure it's triggered only once when the button is pressed
        {
            pressTime = Time.time; // Record the time when the button is pressed
            buttonPressed = true; // Set the flag to true, indicating the button is pressed

            // Start monitoring the hold duration
            holdMonitorCoroutine = StartCoroutine(MonitorHoldDuration());
        }
    }

    private void OnRelease(InputAction.CallbackContext context)
    {
        if (context.canceled && buttonPressed) // Ensure it's triggered only once when the button is released
        {
            if (holdMonitorCoroutine != null)
            {
                StopCoroutine(holdMonitorCoroutine); // Stop the hold monitor coroutine
            }

            releaseTime = Time.time; // Record the time the button is released
            buttonPressed = false; // Reset the flag after releasing the button

            // Calculate the hold duration
            float holdDuration = releaseTime - pressTime;
            //Debug.Log("Button held for: " + holdDuration + " seconds");

            // Trigger the attack with the calculated hold duration
            Attack(holdDuration);
        }
    }

    // void TriggerAttack()  ---------Don't do anything with this code for now, let it stay!-----------
    // {
    //  if (attacking)
    //  {
    //     float damage = isHeavyAttack ? attackDamage * 2 : attackDamage;
    //      AttackRaycast(damage);
    //  }
    // }  ---------Don't do anything with this code for now, let it stay!-----------

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast(float damage)
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer) && !isSprinting)
        {
            Debug.Log(hit.collider.name);
            HitTarget(hit.point);

            // Apply damage and knockback
            Vector3 knockbackForce = transform.forward * enemyThrowBackForce;
            if(hit.transform.TryGetComponent<EnemyHitPoints>(out EnemyHitPoints T)) T.TakeDamage(attributesSystem.playerFinalDamage, knockbackForce);

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
