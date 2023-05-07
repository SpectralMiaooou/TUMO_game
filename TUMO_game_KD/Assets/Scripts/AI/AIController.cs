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

    //HealthLife variables
    private float maxHealthLife = 100f;
    private float healthLife;
    //public Text

    //Impact variables
    private Vector3 impactDirection = Vector3.zero;

    public NavMeshAgent agent;
    CharacterController character;
    public Transform player;

    public FieldOfView field;

    //Movement variables
    private float actualSpeed;
    public float walkingSpeed = 3f;
    public float runningSpeed = 10f;
    public Vector3 move;
    public Vector3 currentMovement;
    private bool canMove = true;
    private bool isWalking;
    private bool isRunning;
    private bool isMoving;

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
    public Weapon weapon;
    private bool isAttacking;

    //Jumping variables
    private bool isJumpPressed = false;
    private bool isJumping = false;
    private float maxJumpHeight = 3f;
    private float maxJumpTime = 1f;
    private float initialJumpVelocity;

    private void Awake()
    {
        setupJumpVariables();
    }

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
        character = GetComponent<CharacterController>();

        disableTargeting();
        
        healthLife = maxHealthLife;
        actualSpeed = walkingSpeed;

        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(character.isGrounded);
        currentMovement.x = 0f;
        currentMovement.z = 0f;

        print(IsGrounded());
        anim.SetBool("isGrounded", IsGrounded());
        handleDecision("attack");

        
        handleAttack();
        //handleMovement();
        if (agent.velocity.magnitude > 0.1)
        {
            Vector3 dirEnemy = agent.desiredVelocity.normalized;
            dirEnemy.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirEnemy), Time.deltaTime * 10f);
        }
        else if(Vector3.Distance(transform.position, player.position) < field.radiusAttack)
        {
            Vector3 dirEnemy = player.position - transform.position;
            dirEnemy.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dirEnemy), Time.deltaTime * 10f);
        }
        handleImpact();
        handleAnimation();

        //agent.Move(currentMovement.normalized * runningSpeed * Time.deltaTime);
        character.Move(currentMovement * Time.deltaTime);
        agent.velocity = character.velocity;


        handleGravity();
        return;
        //handleJump();
    }

    void handleAnimation()
    {
        isAttacking = anim.GetBool("isAttacking");
        isMoving = anim.GetBool("isMoving");
        isJumping = anim.GetBool("isJumping");
        isGrounded = anim.GetBool("isGrounded");
        isWalking = anim.GetBool("isWalking");
        isRunning = anim.GetBool("isRunning");
    }

    void handleMovement()
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

            move = transform.forward * move.magnitude * actualSpeed;
        }
        currentMovement.x = move.x;
        currentMovement.z = move.z;
    }

    void handleAttack()
    {
        if (IsGrounded()&& !isAttacking)
        {
            if(isPrimaryAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                //
            }
            if(isSecondaryAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                //
            }
            if(isUltimateAttackPressed)
            {
                canMove = false;
                anim.SetBool("isAttacking", true);
                //
            }
        }
    }

    private void handleGravity()
    {
        bool isFalling = !IsGrounded() && currentMovement.y < 0f;
        float fallMultiplier = 2.0f;

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
        return Physics.Raycast(transform.position, Vector3.down, 0.05f, groundMask);
    }

    void handleDecision(string type)
    {
        //enableTargeting(player.position, "run");
        if (type == "attack")
        {
            isPrimaryAttackPressed = false;
            if (field.canSeePlayer && IsGrounded())
            {
                lastShotChase = Time.time;
                if (field.isLineOfSight || Vector3.Distance(transform.position, player.position) < field.radiusAttack)
                {
                    disableTargeting();
                    isPrimaryAttackPressed = true;

                }
                else
                {
                    enableTargeting(player.position, "run");
                }
            }
            else if (!field.canSeePlayer && IsGrounded())
            {
                if (Time.time - lastShotChase < cooldownChase)
                {
                    enableTargeting(player.position, "run");
                }
                else
                {
                    disableTargeting();
                }
            }
        }
    }

    void enableTargeting(Vector3 pos, string type)
    {
        if(canMove)
        {
            agent.enabled = true;
            anim.SetBool("isMoving", true);
            if (type == "run")
            {
                anim.SetBool("isRunning", true);
                anim.SetBool("isWalking", false);
                agent.speed = runningSpeed;
            }
            else if (type == "walk")
            {
                anim.SetBool("isWalking", true);
                anim.SetBool("isRunning", false);
                agent.speed = walkingSpeed;
            }
            //agent.SetDestination(pos);
            agent.destination = player.position;
            currentMovement = agent.desiredVelocity;
        }
    }
    void disableTargeting()
    {
        canMove = true;
        agent.enabled = false;
        anim.SetBool("isRunning", false);
        anim.SetBool("isWalking", false);
        anim.SetBool("isMoving", false);
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
