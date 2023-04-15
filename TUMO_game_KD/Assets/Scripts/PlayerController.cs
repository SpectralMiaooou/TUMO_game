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
    public float jumpForce = 10f;
    private Vector3 impact = Vector3.zero;
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float vY;

    Vector2 move;
    Vector3 moveDirection;

    PlayerControls controls;

    private bool isJumping;
    private bool isGrounded;
    private bool isAttacking;

    public Transform groundCheck;
    public LayerMask groundMask;



    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Jump.performed += Jump;
        controls.Gameplay.Attack.performed += Attack;
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
        //ApplyGravity();
        Movement();
        //GroundCheck();
        //FallingCheck();
        //UpdateImpact();
    }

    void Movement()
    {
        if (!isAttacking)
        {
            moveDirection = new Vector3(move.x, 0, move.y);
            anim.SetFloat("inputX", moveDirection.magnitude);

            //print(moveDirection.magnitude);
            if (moveDirection.magnitude < 0.4)
            {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);
            Quaternion camOffset = Quaternion.Euler(0f, cam.rotation.eulerAngles.y, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * camOffset, Time.deltaTime * 10f);
		
	        character.Move(transform.forward * moveDirection.magnitude * speed* Time.deltaTime);

        }
    }
    private void ApplyGravity()
    {
	    if(IsGrounded() && vY < 0.0f)
	    {
		    vY = -1f;
	    }
	    else
	    {
            vY += _gravity * Time.deltaTime;
        }
        character.Move(transform.up * vY * Time.deltaTime);
    }


    void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            isGrounded = false;
            vY += jumpForce;

            isJumping = true;
            anim.SetBool("isJumping", true);
        }
    }
    void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            //isAttacking = true;
            anim.SetTrigger("isAttacking");
        }
    }

    void FallingCheck()
    {
        if (vY < -0.1 && !IsGrounded())
        {
            anim.SetBool("isFalling", true);
            isJumping = false;
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }

    void GroundCheck()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);
	    anim.SetBool("isGrounded", IsGrounded());
    }

    private bool IsGrounded()
    {
	    return character.isGrounded;
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
}
