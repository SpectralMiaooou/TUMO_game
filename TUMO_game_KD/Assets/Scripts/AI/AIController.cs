using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    //Animation variables
    Animator anim;

    //Movement variables
    public float speed = 10f;
    public Vector3 move;
    public Vector3 currentMovement;
    private bool canMove = true;

    //Gravity variables
    private float gravity = -9.81f;
    private float groundedGravity = -0.05f;

    //GroundCheck variables
    private bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;

    //Attack variables
    private bool isPrimaryAttackPressed = false;
    private bool isSecondaryAttackPressed = false;
    private bool isUltimateAttackPressed = false;
    public Attack primary_attack;
    public Attack secondary_attack;
    public Attack ultimate_attack;
    private bool isAttacking;

    //Jumping variables
    private bool isJumpPressed = false;
    private bool isJumping = false;
    private float maxJumpHeight = 3f;
    private float maxJumpTime = 1f;
    private float initialJumpVelocity;
    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", IsGrounded());
        handleAttack();
        Movement();
        handleAnimation();

        //character.Move(currentMovement * Time.deltaTime);


        //handleGravity();
        //handleJump();
    }

    void handleAnimation()
    {
        isAttacking = anim.GetBool("isAttacking");
        isJumping = anim.GetBool("isJumping");
        isGrounded = anim.GetBool("isGrounded");
    }
    void Movement()
    {
        move = Vector3.zero;

        if (canMove)
        {
            move = transform.forward * move.y + transform.right * move.x;
            move.Normalize();

            anim.SetFloat("inputX", move.magnitude, 0.0f, Time.deltaTime * 2f);
            currentMovement.x = 0f;
            currentMovement.z = 0f;


            if (move.magnitude > 0.1)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 10f);
            }

            move = transform.forward * move.magnitude * speed;
        }
        currentMovement.x = move.x;
        currentMovement.z = move.z;
    }
    void handleAttack()
    {
        if (IsGrounded() && !isAttacking)
        {
            if (isPrimaryAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                anim.SetInteger("attackID", primary_attack.attackID);
                anim.SetInteger("attackType", primary_attack.attackType);
            }
            if (isSecondaryAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                anim.SetInteger("attackID", secondary_attack.attackID);
                anim.SetInteger("attackType", secondary_attack.attackType);
            }
            if (isUltimateAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                anim.SetInteger("attackID", ultimate_attack.attackID);
                anim.SetInteger("attackType", ultimate_attack.attackType);
            }
        }
    }


    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.01f, groundMask);
    }

}
