using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Transform cam;
    Animator anim;

    //CharacterController variables
    CharacterController character;

    private Vector3 impact = Vector3.zero;

    //HealthBar variables
    public Image healthBar;

    //Input variables
    public Vector2 move;

    //Impact variables
    private Vector3 impactDirection = Vector3.zero;

    //Attack variables
    public bool isAttacking;
    public bool isPrimaryAttackPressed = false;
    public bool isSecondaryAttackPressed = false;
    public bool isUltimateAttackPressed = false;

    //Movement variables
    public bool canMove = true;
    public bool isWalking;
    public bool isRunning;
    public bool isMoving;
    public bool isRunPressed = false;

    //GroundCheck variables
    public bool isGrounded;

    //Jumping variables
    public bool isJumping = false;
    public bool isJumpPressed = false;
    public bool isFalling;

    //Other Behaviours variables
    public HealthBehaviour health;
    public AttackBehaviour attack;
    public MoveBehaviour movement;
    public RotationBehaviour rotation;
    public JumpBehaviour jump;
    public InputHandler controls;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
        health = GetComponent<HealthBehaviour>();
        attack = GetComponent<AttackBehaviour>();
        movement = GetComponent<MoveBehaviour>();
        rotation = GetComponent<RotationBehaviour>();
        jump = GetComponent<JumpBehaviour>();
        controls = GetComponent<InputHandler>();

        character = GetComponent<CharacterController>();

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        anim.SetBool("canMove", true);
    }

    // Update is called once per frame
    void Update()
    {
        //handleImpact();
        //ENABLE RUNNING
        handleBoolean();
        if (isRunPressed && isGrounded)
        {
            anim.SetBool("isRunning", true);
        }

        //ATTACKS
        if(isGrounded)
        {
            if (isPrimaryAttackPressed)
            {
                attack.handleAttack(1);
            }
            else if (isSecondaryAttackPressed)
            {
                attack.handleAttack(2);
            }
            else if (isUltimateAttackPressed)
            {
                attack.handleAttack(3);
            }
        }

        //ANIMATION
        handleAnimation();

        //ROTATION AND MOVEMENT
        if (!isAttacking)
        {
            rotation.handleRotation(move, cam);
        }
        if(canMove)
        {
            movement.handleMovement(move, isRunning);
        }

        //JUMP AND GRAVITY
        jump.handleGravity();
        if(isJumpPressed && canMove)
        {
            jump.handleJump(isJumping);
        }

        handleHealthBar();

        //Debug.Log(isJumpPressed);
    }

    void handleAnimation()
    {
        anim.SetBool("isGrounded", character.isGrounded);

        isGrounded = anim.GetBool("isGrounded");
        isAttacking = anim.GetBool("isAttacking");
        isMoving = anim.GetBool("isMoving");
        isJumping = anim.GetBool("isJumping");
        isWalking = anim.GetBool("isWalking");
        isRunning = anim.GetBool("isRunning");
        isFalling = anim.GetBool("isFalling");
        canMove = anim.GetBool("canMove");
    }

    void handleBoolean()
    {
        move = controls.move;

        isPrimaryAttackPressed = controls.isPrimaryAttackPressed;
        isSecondaryAttackPressed = controls.isSecondaryAttackPressed;
        isUltimateAttackPressed = controls.isUltimateAttackPressed;

        isRunPressed = controls.isRunPressed;

        isJumpPressed = controls.isJumpPressed;
}

    void handleHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp( healthBar.fillAmount, (health.healthLife / health.maxHealthLife), 3f * Time.deltaTime);

        Color healthColor = Color.Lerp(Color.red, Color.green, (health.healthLife / health.maxHealthLife));
        healthBar.color = healthColor;
    }
    /*
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
        }*/
}
