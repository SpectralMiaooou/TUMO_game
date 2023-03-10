using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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



    private void Awake(){
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
        if(!isAttacking){
            moveDirection = transform.forward * move.y + transform.right * move.x;
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

            anim.SetFloat("inputX", move.x);
            anim.SetFloat("inputY", move.y);
        }
    }

    void Jump(InputAction.CallbackContext context){
        if(context.performed && isGrounded){
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            isJumping = true;
            anim.SetTrigger("isJumping");
        }
    }
    void Attack(InputAction.CallbackContext context){
        if(context.performed && isGrounded){
            //isAttacking = true;
            anim.SetTrigger("isAttacking");
        }
    }

    void FallingCheck(){
        if(rb.velocity.y < -0.1 && !isGrounded){
            anim.SetBool("isFalling", true);
        }
        else{
            anim.SetBool("isFalling", false);
        }
    }

    void GroundCheck(){
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.4f, groundMask);

        if(isGrounded){
            anim.SetBool("isGrounded", true);
        }
        else{
            anim.SetBool("isGrounded", false);
        }
    }
}
