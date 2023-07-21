using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Animator variables
    Animator anim;

    public PlayerController player;

    //Impact variables
    private Vector3 impactDirection = Vector3.zero;

    //Camera variables
    Transform cam;

    //CharacterController variables
    CharacterController character;

    //Movement variables
    private float actualSpeed;
    public float walkingSpeed = 3f;
    public float runningSpeed = 10f;
    public Vector3 desiredMoveDirection;
    public Vector3 currentMovement;
    public bool canMove = true;
    private bool isWalking;
    private bool isRunning;
    private bool isMoving;
    public bool isRunPressed = false;

    //Gravity variables
    private float gravity = -9.81f;
    private float groundedGravity = -0.05f;

    //GroundCheck variables
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;

    //Jumping variables
    public bool isJumpPressed = false;
    private bool isJumping = false;
    private float maxJumpHeight = 3f;
    private float maxJumpTime = 1f;
    private float initialJumpVelocity;


    private void Awake()
    {
        setupJumpVariables();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        actualSpeed = walkingSpeed;
    }
    private void Update()
    {
        anim.SetBool("isGrounded", character.isGrounded);
        handleMovement();
        handleGravity();
        handleJump();
    }
    public void handleMovement()
    {
        Vector2 move = player.move;
        desiredMoveDirection = Vector3.zero;

        if (player.canMove)
        {
            if (player.isRunning)
            {
                actualSpeed = runningSpeed;
            }
            else
            {
                actualSpeed = walkingSpeed;
                if (player.isRunPressed)
                {
                    actualSpeed = runningSpeed;
                    player.isRunning = true;
                    anim.SetBool("isRunning", true);
                }
            }

            currentMovement.x = 0f;
            currentMovement.z = 0f;

            var forward = cam.transform.forward;
            var right = cam.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            desiredMoveDirection = forward * move.y + right * move.x;

            anim.SetBool("isMoving", false);
            if (move.magnitude < 0.2)
            {
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
            }
            else if (move.magnitude < 0.9)
            {
                anim.SetBool("isWalking", false);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 10f * Time.deltaTime);
            }
            else //if (move.magnitude > 0.9)
            {
                desiredMoveDirection *= actualSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 10f * Time.deltaTime);

                if (!player.isRunning)
                {
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isRunning", false);
                }
                else
                {
                    anim.SetBool("isRunning", true);
                    anim.SetBool("isWalking", false);
                }
                anim.SetBool("isMoving", true);
            }
        }
        currentMovement.x = desiredMoveDirection.x;
        currentMovement.z = desiredMoveDirection.z;
    }

    public void handleGravity()
    {
        character.Move(currentMovement * Time.deltaTime);

        bool isFalling = !isGrounded && currentMovement.y < 0f;
        float fallMultiplier = 2.0f;

        if (isGrounded)
        {
            anim.SetBool("isFalling", false);
            currentMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            anim.SetBool("isFalling", true);
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20.0f);
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

    public void handleJump()
    {
        if (player.isJumpPressed && isGrounded && !player.isJumping)
        {
            anim.SetBool("isJumping", true);

            currentMovement.y = initialJumpVelocity * 0.5f;
        }
        else if (!player.isJumpPressed && player.isJumping && isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
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
}
