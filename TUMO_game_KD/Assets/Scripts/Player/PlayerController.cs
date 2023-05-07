using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Transform cam;
    Animator anim;
    CharacterController character;

    private Vector3 impact = Vector3.zero;

    private bool canTurn180;

    //Input variables
    PlayerInputs inputManager = new PlayerInputs();

    //Impact variables
    private Vector3 impactDirection = Vector3.zero;

    //Attack variables
    public GameObject weaponObject;
    private AttackAbility weaponAbility;
    private Weapon weapon;
    private bool isAttacking;

    //Movement variables
    private float actualSpeed;
    public float walkingSpeed = 3f;
    public float runningSpeed = 10f;
    public Vector3 desiredMoveDirection;
    public Vector3 currentMovement;
    private bool canMove = true;
    private bool isWalking;
    private bool isRunning;
    private bool isMoving;

    //Gravity variables
    private float gravity = -9.81f;
    private float groundedGravity = -0.05f;

    //GroundCheck variables
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;

    //Jumping variables
    private bool isJumping = false;
    private float maxJumpHeight = 3f;
    private float maxJumpTime = 1f;
    private float initialJumpVelocity;



    private void Awake()
    {
        inputManager.SetInput();

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
        inputManager.controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
        inputManager.controls.Gameplay.Disable();
    }



    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        weaponAbility = weaponObject.GetComponent<AttackAbility>();
        weapon = weaponAbility.weapon;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        actualSpeed = walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", character.isGrounded);
        handleAttack();
        handleMovement();
        handleImpact();
        handleAnimation();

        character.Move(currentMovement * Time.deltaTime);


        handleGravity();
        handleJump();
    }

    void handleAnimation()
    {
        isAttacking = anim.GetBool("isAttacking");
        isMoving = anim.GetBool("isMoving");
        isJumping = anim.GetBool("isJumping");
        isGrounded = anim.GetBool("isGrounded");
        isWalking = anim.GetBool("isWalking");
        isRunning = anim.GetBool("isRunning");
    }

    void handleMovement()
    {
        Vector2 move = inputManager.move;
        desiredMoveDirection = Vector3.zero;

        if (canMove)
        {
            if(isRunning)
            {
                actualSpeed = runningSpeed;
            }
            else
            {
                actualSpeed = walkingSpeed;
                if(inputManager.isRunPressed)
                {
                    actualSpeed = runningSpeed;
                    isRunning = true;
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

                if(!isRunning)
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

    private void handleGravity()
    {
        bool isFalling = !character.isGrounded && currentMovement.y < 0f;
        float fallMultiplier = 2.0f;

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
        if (inputManager.isJumpPressed && character.isGrounded && !isJumping)
        {
            anim.SetBool("isJumping", true);

            currentMovement.y = initialJumpVelocity * 0.5f;
        }
        else if (!inputManager.isJumpPressed && isJumping && character.isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
    }
    
    void handleAttack()
    {
        if (character.isGrounded && !isAttacking)
        {
            if(inputManager.isPrimaryAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                anim.Play("Attacks");
                anim.Play(weapon.primaryAttack.attackAnimation);
                //activeAttackHit();
            }
            if(inputManager.isSecondaryAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                anim.Play("Attacks");
                anim.Play(weapon.secondaryAttack.attackAnimation);
                //
            }
            if(inputManager.isUltimateAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                //
            }
        }
    }

    void AddImpact()
    {
        float force = 1f;
        Vector3 dir = transform.forward;
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
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", false);
        //isAttacking = false;
        canMove = true;
        //anim.Play("Walking");
    }
    void disableTurn()
    {
        anim.SetBool("canTurn180", false);
        canMove = true;
        canTurn180 = false;
    }

    public void activeAttackHit(Attack attack)
    {
        weaponAbility.EnableAttack(attack.attackType);
    }


    bool IsGrounded()
    {
        return Physics.Raycast(groundCheck.transform.position, Vector3.down, 0.05f, groundMask);
    }
}
