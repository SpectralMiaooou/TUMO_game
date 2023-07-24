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
    private float radiusActionZone;

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
    private Attack currentAttack;

    //Jumping variables
    private bool isJumpPressed = false;
    private bool isJumping = false;
    private bool isFalling;
    private float maxJumpHeight = 3f;
    private float maxJumpTime = 1f;
    private float initialJumpVelocity;

    //Other Behaviours variables
    public HealthBehaviour health;
    public AttackBehaviour attack;
    public ChaseBehaviour chase;
    public RotationBehaviour rotation;
    public JumpBehaviour jump;
    public VisionBehaviour vision;

    public enum EnemyStates
    {
        IDLE,
        PATROLLING,
        CHASE,
        FLEE
    }
    public EnemyStates enemyState = EnemyStates.IDLE;

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

        health = GetComponent<HealthBehaviour>();
        attack = GetComponent<AttackBehaviour>();
        chase = GetComponent<ChaseBehaviour>();
        rotation = GetComponent<RotationBehaviour>();
        jump = GetComponent<JumpBehaviour>();
        vision = GetComponent<VisionBehaviour>();

        disableTargeting();

        actualSpeed = walkingSpeed;
        agent.updatePosition = false;
        agent.updateRotation = false;

        anim.SetBool("canMove", true);
    }

    // Update is called once per frame
    void Update()
    {
        radiusActionZone = attack.weapon.maxRange;
        vision.radiusAttack = radiusActionZone;
        agent.stoppingDistance = radiusActionZone * 0.70f;
        //Debug.Log(character.isGrounded);

        Vector2 direction = Vector2.right * agent.desiredVelocity.x + Vector2.up * agent.desiredVelocity.z;
        
        //ROTATION AND MOVEMENT
        if (!isAttacking)
        {
            if(radiusActionZone >= agent.remainingDistance)
            {
                rotation.handleRotation(direction, null);
            }
            else
            {
                rotation.handleRotation(direction, null);
            }
        }

        //ATTACKS
        if (!isAttacking && isGrounded && vision.isLineOfSight)
        {
            if(Random.value <= 0.7f)
            {
                agent.enabled = false;
                attack.handleAttack(1);
            }
            else if (Random.value <= 0.3f)
            {
                agent.enabled = false;
                attack.handleAttack(2);
            }
            else if (Random.value <= 0.05f)
            {
                agent.enabled = false;
                attack.handleAttack(3);
            }
        }

        //ANIMATION
        handleAnimation();

        //CHASE
        if (isGrounded && !isAttacking && radiusActionZone < agent.remainingDistance)
        {
            chase.handleChase(player.transform);
        }

        //JUMP AND GRAVITY
        jump.handleGravity();
        if (isJumpPressed)
        {
            jump.handleJump(isJumping);
        }

        /*
        //print(character.isGrounded);
        currentMovement.x = 0f;
        currentMovement.z = 0f;

        //print(IsGrounded());
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
        //handleJump();*/
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
            if (vision.canSeePlayer && IsGrounded())
            {
                lastShotChase = Time.time;
                if (vision.isLineOfSight || Vector3.Distance(transform.position, player.position) < vision.radiusAttack)
                {
                    disableTargeting();
                    isPrimaryAttackPressed = true;

                }
                else
                {
                    enableTargeting(player.position, "run");
                }
            }
            else if (!vision.canSeePlayer && IsGrounded())
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
