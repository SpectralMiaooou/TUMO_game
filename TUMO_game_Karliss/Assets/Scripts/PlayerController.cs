using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed;
    Animator anim;
    Rigidbody rb;

    public float speed = 10f;
    public float jumpForce = 10f;

    Vector2 move;
    Vector3 moveDirection;

    PlayerControls controls;

    private bool isJumping;
    private bool isGrounded;
   

    private void Awake(){
        controls = new PlayerControls();

        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
        controls.Gameplay.Jump.performed += Jump;
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
        print(move);
        Movement();
        FallingCheck();
        print(rb.velocity.y);
    }

    void Movement()
    {
        moveDirection = transform.forward * move.y + transform.right * move.x;
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

        anim.SetFloat("inputX", move.x);
        anim.SetFloat("inputY", move.y);
    }

    void Jump(InputAction.CallbackContext context){
        Debug.Log(context);
        if(context.performed){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetBool("isJumping", true);
        }
    }

    void FallingCheck(){
        if(rb.velocity.y < -0.1){
            anim.SetBool("isFalling", true);
        }
        else{
            anim.SetBool("isFalling", true);
        }
    }

    void OnCollisionEnter(Collision col){
        if(col.transform.tag == "Ground"){
            anim.SetBool("isGrounded", true);
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
         }
     }
    void OnCollisionExit(Collision col){
        if(col.transform.tag == "Ground"){
            anim.SetBool("isGrounded", false);
            if(isJumping){
                anim.SetBool("isJumping", true);
                anim.SetBool("isFalling", false);
            }
            else{
                anim.SetBool("isFalling", true);
                anim.SetBool("isJumping", false);
            }
        }
    }
}
