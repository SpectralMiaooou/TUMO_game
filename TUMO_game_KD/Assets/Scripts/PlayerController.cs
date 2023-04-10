using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Transform cam;
    Animator anim;
    Rigidbody rb;

    public float speed = 10f;
    public float jumpForce = 10f;

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
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        GroundCheck();
        FallingCheck();
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

            rb.AddForce(transform.forward * moveDirection.magnitude * speed * 1f, ForceMode.Force);
        }
    }

    void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isJumping = true;
            anim.SetTrigger("isJumping");
        }
    }
    void Attack(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            //isAttacking = true;
            anim.SetTrigger("isAttacking");
        }
    }

    void FallingCheck()
    {
        if (rb.velocity.y < -0.1 && !isGrounded)
        {
            anim.SetBool("isFalling", true);
        }
        else
        {
            anim.SetBool("isFalling", false);
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);

        if (isGrounded)
        {
            anim.SetBool("isGrounded", true);
        }
        else
        {
            anim.SetBool("isGrounded", false);
        }
    }
}
