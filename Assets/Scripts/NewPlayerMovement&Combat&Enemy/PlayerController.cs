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
    public float moveSpeed = 5;
    public float gravity = -9.82f;
    public float jumpHeight = 1.2f;
    private bool sprinting;
    private bool isTeleporting = false; // Flag to check if teleporting
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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

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
            isTeleporting = false; // Reset teleport flag
            return;
        }

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
        _PlayerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && _PlayerVelocity.y < 0)
            _PlayerVelocity.y = -2f;
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

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            moveSpeed = 8f;
        }
        else
        {
            moveSpeed = 5f;
        }
    }

    void AssignInputs()
    {
        //input.Jump.performed += ctx => Jump();
        input.Attack.started += ctx => Attack();
        input.Sprint.performed += ctx => Sprint();
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
            if(!sprinting)
            { 
                ChangeAnimationState(IDLE);
                
            }
            
            else if (sprinting && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) == true)
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

        if (!sprinting) 
        {
            readyToAttack = false;
            attacking = true;

            Invoke(nameof(ResetAttack), attackSpeed);
            Invoke(nameof(AttackRaycast), attackDelay);

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(swordSwing);
        }

        if(attackCount == 0 && !sprinting)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else if (attackCount == 1 && !sprinting)
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
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer) && !sprinting)
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