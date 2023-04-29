using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    //Time variables
    private float lastShotChase;
    private float cooldownChase = 10f;

    //Animation variables
    Animator anim;

    //Impact variables
    private Vector3 impactDirection = Vector3.zero;

    public NavMeshAgent agent;
    CharacterController character;
    public Transform player;

    public FieldOfView field;

    //Movement variables
    public float speed = 10f;
    public Vector3 move;
    public Vector3 currentMovement;
    private bool canMove = true;

    //Gravity variables
    private float gravity = -9.81f;
    private float groundedGravity = -0.05f;

    //GroundCheck variables
    public bool isGrounded;
    public Transform groundCheck;
    public LayerMask groundMask;

    //Attack variables
    private bool isPrimaryAttackPressed = false;
    private bool isSecondaryAttackPressed = false;
    private bool isUltimateAttackPressed = false;
    public Weapon weapon;/*
    public Attack primary_attack;
    public Attack secondary_attack;
    public Attack ultimate_attack;*/
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
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        disableChasing();
    }

    // Update is called once per frame
    void Update()
    {

        handleDecision();
        anim.SetBool("isGrounded", IsGrounded());
        handleAttack();
        //Movement();
        handleAnimation();

        agent.Move(currentMovement * Time.deltaTime);


        handleGravity();
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
            if(isPrimaryAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                anim.SetInteger("attackID", weapon.primaryAttack.attackID);
                anim.SetInteger("attackType", weapon.primaryAttack.attackType);
            }
            if(isSecondaryAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                anim.SetInteger("attackID", weapon.secondaryAttack.attackID);
                anim.SetInteger("attackType", weapon.secondaryAttack.attackType);
            }
            if(isUltimateAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                anim.SetInteger("attackID", weapon.ultimateAttack.attackID);
                anim.SetInteger("attackType", weapon.ultimateAttack.attackType);
            }
        }
    }

    private void handleGravity()
    {
        bool isFalling = !IsGrounded() && currentMovement.y < 0f;
        float fallMultiplier = 2.0f;

        //anim.SetBool( "isFalling", isFalling);

        if (IsGrounded())
        {
            anim.SetBool("isFalling", false);
            currentMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            anim.SetBool("isFalling", true);
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20.0f);
            currentMovement.y = nextYVelocity;
        }
        else
        {
            anim.SetBool("isFalling", false);
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
        }
    }


    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, groundMask);
    }

    void handleDecision()
    {
        isPrimaryAttackPressed = false;
        if(field.canSeePlayer && IsGrounded())
        {
            if(field.isLineOfSight)
            {
                disableChasing();
                isPrimaryAttackPressed = true;
            }
            else if(!field.isLineOfSight )
            {
                enableChasing(player.position);
                lastShotChase = Time.time;
            }
        }
        else if(!field.canSeePlayer && IsGrounded())
        {
            if(Time.time - lastShotChase < cooldownChase)
            {
                enableChasing(player.position);
            }
            else

            {
                disableChasing();
            }
        }
    }
    void enableChasing(Vector3 pos)
    {
        canMove = false;
        agent.isStopped = false;
        anim.SetFloat("inputX", 1f);
        agent.SetDestination(pos);
    }
    void disableChasing()
    {
        canMove = true;
        agent.isStopped = true;
        anim.SetFloat("inputX", 0f);
    }
    void disableAttack()
    {
        anim.SetBool("isAttacking", false);
        //isAttacking = false;
        canMove = true;
        //anim.Play("Walking");
    }

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
    }

}
