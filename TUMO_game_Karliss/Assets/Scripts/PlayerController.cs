using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;

    public float rotationSpeed;
    Animator anim;
    Rigidbody rb;

    public float speed = 10f;
    public float jumpForce = 10f;

    Vector2 move;
    Vector2 rotation;
    Vector3 moveDirection;

    PlayerControls controls;
   

    private void Awake(){
        controls = new PlayerControls();

        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;

        controls.Gameplay.Rotation.performed += ctx => rotation = ctx.ReadValue<Vector2>();
        controls.Gameplay.Rotation.canceled += ctx => rotation = Vector2.zero;
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
        print(rotation);
        Movement();
        //rotationCam();
    }

    void Movement()
    {
        moveDirection = transform.forward * move.y + transform.right * move.x;
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

        anim.SetFloat("inputX", move.x);
        anim.SetFloat("inputY", move.y);
    }

    void rotationCam(){
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 inputDir = orientation.forward * rotation.x + orientation.right * rotation.y;
        
        if (inputDir != Vector3.zero){
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
