using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Transform cam;
    Animator anim;

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

    //Gravity variables
    private float gravity = -9.81f;
    private float groundedGravity = -0.05f;

    //GroundCheck variables
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;

    //Jumping variables
    public bool isJumping = false;
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
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    void OnDisable()
    {
        controls.Gameplay.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        life = new HealthLife();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //handleImpact();
        handleAnimation();

        handleHealthBar();

        Debug.Log(isPrimaryAttackPressed);
    }

    void handleAnimation()
    {
        isAttacking = anim.GetBool("isAttacking");
        isMoving = anim.GetBool("isMoving");
        isJumping = anim.GetBool("isJumping");
        isGrounded = anim.GetBool("isGrounded");
        isWalking = anim.GetBool("isWalking");
        isRunning = anim.GetBool("isRunning");
        canMove = anim.GetBool("canMove");
    }

    void handleHealthBar()
    {
        healthBar.fillAmount = Mathf.Lerp( healthBar.fillAmount, (life.healthLife / life.maxHealthLife), 3f * Time.deltaTime);

        Color healthColor = Color.Lerp(Color.red, Color.green, (life.healthLife / life.maxHealthLife));
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
