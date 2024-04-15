using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GravityBehaviour))]
public class JumpBehaviour : MonoBehaviour
{
    GravityBehaviour gravity;

    //CharacterController variables
    CharacterController character;

    //Animator variables
    Animator anim;

    private void Start()
    {
        gravity = GetComponent<GravityBehaviour>();
    }

    public void handleJump(bool isJumping)
    {
        if (character.isGrounded && !isJumping)
        {
            anim.SetBool("isJumping", true);

            gravity.currentHeight = gravity.initialJumpVelocity * 0.5f;
        }/*
        else if (isJumping && character.isGrounded)
        {
            isJumping = false;

            //anim.SetBool("isJumping", false);
        }*/
    }
}
