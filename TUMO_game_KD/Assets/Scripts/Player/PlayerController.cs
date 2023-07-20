using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Transform cam;
    Animator anim;
    CharacterController character;

    private Vector3 impact = Vector3.zero;

    private bool canTurn180;

    //HealthBar variables
    public HealthLife life;
    public Image healthBar;

    //Input variables
    public PlayerControls controls;
    public Vector2 move;

    //Impact variables
    private Vector3 impactDirection = Vector3.zero;

    //Attack variables
    public Weapon weapon;
    private bool isAttacking;
    private Attack currentAttack;
    public bool isPrimaryAttackPressed = false;
    public bool isSecondaryAttackPressed = false;
    public bool isUltimateAttackPressed = false;


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
    public bool isRunPressed = false;

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
    public bool isJumpPressed = false;



    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Jump.started += onJump;
        controls.Gameplay.Jump.canceled += onJump;
        controls.Gameplay.Run.started += onRun;
        controls.Gameplay.Run.canceled += onRun;

        controls.Gameplay.PrimaryAttack.started += onPrimaryAttack;
        controls.Gameplay.PrimaryAttack.canceled += onPrimaryAttack;

        controls.Gameplay.SecondaryAttack.started += onSecondaryAttack;
        controls.Gameplay.SecondaryAttack.canceled += onSecondaryAttack;

        controls.Gameplay.UltimateAttack.started += onUltimateAttack;
        controls.Gameplay.UltimateAttack.canceled += onUltimateAttack;

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
        life = GetComponent<HealthLife>();
        anim = GetComponent<Animator>();

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

        handleHealthBar();
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
                if(isRunPressed)
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
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 25f * Time.deltaTime);
            }
            else //if (move.magnitude > 0.9)
            {
                desiredMoveDirection *= actualSpeed;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 25f * Time.deltaTime);

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
        if (character.isGrounded && !isAttacking)
        {
            if(isPrimaryAttackPressed)
            {
                _attackSetup(weapon.primaryAttack);
                //activeAttackHit();
            }
            if(isSecondaryAttackPressed)
            {
                _attackSetup(weapon.secondaryAttack);
            }
            if(isUltimateAttackPressed)
            {
                _attackSetup(weapon.ultimateAttack);
            }
        }
    }

    private void _attackSetup(Attack attack)
    {
        canMove = false;
        anim.SetBool("isAttacking", true);
        anim.Play("Attacks");
        anim.Play(attack.attackAnimation);
        currentAttack = attack;
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

    public void Attack()
    {
        GameObject attackObject = currentAttack.attackManager;
        GameObject _object = Instantiate(attackObject, transform.position + currentAttack.offset, transform.rotation);
        AttackController h = _object.GetComponent<AttackController>();
        h.user = gameObject;
        h.maxRange = currentAttack.maxRange;
        h.radius = currentAttack.radius;
        h.damage = currentAttack.attackDamage;
    }

    void handleHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp( healthBar.fillAmount, (life.healthLife / life.maxHealthLife), 3f * Time.deltaTime);

        Color healthColor = Color.Lerp(Color.red, Color.green, (life.healthLife / life.maxHealthLife));
        healthBar.color = healthColor;
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

    bool IsGrounded()
    {
        return Physics.Raycast(groundCheck.transform.position, Vector3.down, 0.05f, groundMask);
    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onPrimaryAttack(InputAction.CallbackContext context)
    {
        isPrimaryAttackPressed = context.ReadValueAsButton();
    }
    void onSecondaryAttack(InputAction.CallbackContext context)
    {
        isSecondaryAttackPressed = context.ReadValueAsButton();
    }
    void onUltimateAttack(InputAction.CallbackContext context)
    {
        isUltimateAttackPressed = context.ReadValueAsButton();
    }
    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }
}
