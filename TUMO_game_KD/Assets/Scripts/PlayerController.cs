using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Transform cam;
    Animator anim;
    CharacterController character;

    public float speed = 10f;
    private Vector3 impact = Vector3.zero;
    public Vector3 desiredMoveDirection;

    Vector2 move;
    Vector3 moveDirection;

    PlayerControls controls;

    private bool isGrounded;
    private bool isAttacking;

    public Transform groundCheck;
    public LayerMask groundMask;


    //Attack variables
    private bool isAttackPressed = false;

    //Movement variables
    public Vector3 currentMovement;
    private bool canMove = true;

    //Gravity variables
    private float gravity = -9.81f;
    private float groundedGravity = -0.05f;


    //Jumping variables
    private bool isJumpPressed = false;
    private bool isJumping = false;
    private float maxJumpHeight = 3f;
    private float maxJumpTime = 1f;
    private float initialJumpVelocity;



    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Jump.started += onJump;
        controls.Gameplay.Jump.canceled += onJump;
        controls.Gameplay.Attack.started += onAttack;
        controls.Gameplay.Attack.canceled += onAttack;
        //controls.Gameplay.Attack.performed += Attack;

        setupJumpVariables();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        handleAttack();
        Movement();
        handleAnimation();
        //UpdateImpact();

        character.Move(currentMovement * Time.deltaTime);

        isGrounded = character.isGrounded;
        anim.SetBool("isGrounded", IsGrounded());

        handleGravity();
        handleJump();
    }

    void handleAnimation()
    {
        isAttacking = anim.GetBool("isAttacking");
        isJumping = anim.GetBool("isJumping");
    }

    void Movement()
    {
        desiredMoveDirection = Vector3.zero;

        if (canMove)
        {
            anim.SetFloat("inputX", move.magnitude, 0.0f, Time.deltaTime * 2f);
            currentMovement.x = 0f;
            currentMovement.z = 0f;


            var forward = cam.transform.forward;
            var right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            desiredMoveDirection = forward * move.y + right * move.x;

            if (move.magnitude > 0.1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), Time.deltaTime * 10f);
            }
            
            desiredMoveDirection = transform.forward * move.magnitude * speed;
        }
        currentMovement.x = desiredMoveDirection.x;
        currentMovement.z = desiredMoveDirection.z;
    }
    private void handleGravity()
    {
        bool isFalling = currentMovement.y <= 0.0f;
        float fallMultiplier = 2.0f;

        //anim.SetBool( "isFalling", isFalling);

        if (character.isGrounded)
        {
            anim.SetBool("isFalling", false);
            currentMovement.y = groundedGravity;
	    }
        else if(isFalling)
        {
            anim.SetBool("isFalling", true);
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f,-20.0f) ;
            currentMovement.y = nextYVelocity;
        }
	    else
        {
            anim.SetBool("isFalling", false);
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
        }
    }


    void handleJump()
    {
        if (isJumpPressed && character.isGrounded && !isJumping)
        {
            anim.SetBool("isJumping", true);

            currentMovement.y = initialJumpVelocity * 0.5f;
        }
        else if (!isJumpPressed && isJumping && character.isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
    }
    void handleAttack()
    {
        if (isAttackPressed && character.isGrounded && !isAttacking)
        {
            anim.SetBool("isAttacking", true);
        }
    }

    void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / 10;
    }
    void UpdateImpact()
    {
        if (impact.magnitude > 0.2) character.Move(impact * Time.deltaTime);
        // consumes the impact energy each cycle:
        impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.05f, groundMask);
    }






    void enableMovement()
    {
        canMove = true;
    }






    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onAttack(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
    }
}
