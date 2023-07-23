using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBehaviour : MonoBehaviour
{
    //Animator variables
    Animator anim;

    //CharacterController variables
    CharacterController character;

    //Jumping variables
    public bool isJumping = false;
    private float maxJumpHeight = 3f;
    private float maxJumpTime = 1f;
    private float initialJumpVelocity;
    private float currentHeight;

    //Gravity variables
    private float gravity = -9.81f;
    private float groundedGravity = -0.05f;

    private void Awake()
    {
        setupJumpVariables();
    }

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    public void handleGravity()
    {
        character.Move(Vector3.up * currentHeight * Time.deltaTime);
        bool isFalling = !character.isGrounded && currentHeight < 0f;
        float fallMultiplier = 2.0f;

        if (character.isGrounded)
        {
            isFalling = false;

            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);

            currentHeight = groundedGravity;
        }
        else if (isFalling)
        {
            isFalling = true;

            anim.SetBool("isFalling", true);
            float previousYVelocity = currentHeight;
            float newYVelocity = currentHeight + (gravity * fallMultiplier * Time.deltaTime);
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -20.0f);
            currentHeight = nextYVelocity;
        }
        else
        {
            isFalling = false;

            anim.SetBool("isFalling", false);
            float previousYVelocity = currentHeight;
            float newYVelocity = currentHeight + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentHeight = nextYVelocity;
        }
    }
    public void handleJump(bool isJumping)
    {
        if (character.isGrounded && !isJumping)
        {
            anim.SetBool("isJumping", true);

            currentHeight = initialJumpVelocity * 0.5f;
        }/*
        else if (isJumping && character.isGrounded)
        {
            isJumping = false;

            //anim.SetBool("isJumping", false);
        }*/
    }
}
