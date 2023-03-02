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

    private void Awake(){
        controls = new PlayerControls();

        controls.Gameplay.Movement.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => move = Vector2.zero;
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
    }

    // Update is called once per frame
    void Update()
    {
        print(move);
        Movement();
    }

    void Movement()
    {
        moveDirection = new Vector3(move.x, 0f, move.y);
        rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);

        anim.SetFloat("inputX", move.x);
        anim.SetFloat("inputY", move.y);
    }
}
