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

    //Scroll Variables
    public float scroll;

    //GroundCheck variables
    public bool isGrounded;

    //Jumping variables
    public bool isJumping = false;
    public bool isJumpPressed = false;
    public bool isFalling;

    //Other Behaviours variables
    public HealthBehaviour health;
    public UseItemBehaviour use;
    public MoveBehaviour movement;
    public RotationBehaviour rotation;
    public JumpBehaviour jump;
    public InputHandler controls;
    public InventoryBehaviour inventory;

    //Weapon variables
    public WeaponItem weapon;

    public Transform target;

    [Header("Item Attach Body Parts")]
    public Transform rightHandPlacement;
    public Transform leftHandPlacement;
    public Transform rightFootPlacement;
    public Transform leftFootPlacement;
    public Transform torsoPlacement;
    public Transform headPlacement;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
        health = GetComponent<HealthBehaviour>();
        use = GetComponent<UseItemBehaviour>();
        movement = GetComponent<MoveBehaviour>();
        rotation = GetComponent<RotationBehaviour>();
        jump = GetComponent<JumpBehaviour>();
        controls = GetComponent<InputHandler>();
        inventory = GetComponent<InventoryBehaviour>();

        character = GetComponent<CharacterController>();

        CameraController.instance.player = this;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        anim.SetBool("canMove", true);
    }

    // Update is called once per frame
    void Update()
    {
        //handleImpact();
        //ENABLE RUNNING
        handleBoolean();
        CheckInteractable();

        if(!Mathf.Approximately(0f, scroll))
        {
            inventory.ChangeSlot(scroll);
        }

        if (isRunPressed && isGrounded)
        {
            anim.SetBool("isRunning", true);
        }

        //USE
        if(isGrounded)
        {
            Debug.Log(inventory.currentItem);
            Debug.Log(inventory.currentItem.item);
            use.UseItem(inventory.currentItem.item, controls);
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
        //Debug.Log(isJumpPressed);
    }

    public void LateUpdate()
    {
        CameraController.instance.HandleAllCameraActions();
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
        scroll = controls.scroll;
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
    public void CheckInteractable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        foreach (Collider collider in colliders)
        {
            //print(collider.transform.name);
            if (collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact(gameObject.transform);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
