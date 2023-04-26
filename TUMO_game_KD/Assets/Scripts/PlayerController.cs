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

    //Impact variables
    private Vector3 impactDirection = Vector3.zero;


    //Attack variables
    private bool isAttackPressed = false;
    public Attack light_attack;
    public Attack heavy_attack;

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
        anim.SetBool("isGrounded", IsGrounded());
        handleAttack();
        Movement();
        handleImpact();
        handleAnimation();

        character.Move(currentMovement * Time.deltaTime);


        handleGravity();
        handleJump();
    }

    void handleAnimation()
    {
        //isAttacking = anim.GetBool("isAttacking");
        isJumping = anim.GetBool("isJumping");
        isGrounded = anim.GetBool("isGrounded");
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
        bool isFalling = !IsGrounded() && currentMovement.y < 0f;
        float fallMultiplier = 2.0f;

        //anim.SetBool( "isFalling", isFalling);

        if (IsGrounded())
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
        if (isJumpPressed && IsGrounded() && !isJumping)
        {
            anim.SetBool("isJumping", true);

            currentMovement.y = initialJumpVelocity * 0.5f;
        }
        else if (!isJumpPressed && isJumping && IsGrounded())
        {
            anim.SetBool("isJumping", false);
        }
    }
    void handleAttack()
    {
        if (isAttackPressed && IsGrounded() && !isAttacking)
        {
            canMove = false;
            isAttacking = true;
            anim.Play(light_attack.attackAnimation);
            //anim.SetBool("isAttacking", true);
            //ddImpact(transform.forward, 2f);
        }
    }

    void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        impactDirection = dir.normalized * force * 10f;
    }
    void handleImpact()
    {
        if (impactDirection.magnitude > 0.2)
        {
            currentMovement += impactDirection;
        }
        // consumes the impact energy each cycle:
        impactDirection = Vector3.Lerp(impactDirection, Vector3.zero, 4 * Time.deltaTime);
    }




    void disableAttack()
    {
        //anim.SetBool("isAttacking", false);
        canMove = true;
    }


    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.01f, groundMask);
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
