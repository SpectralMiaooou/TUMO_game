using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    private readonly int WALKING_ANIMATION = Animator.StringToHash("walking");

    //Movement variables
    private float walkingSpeed = 3f;
    public Vector3 desiredMoveDirection;

    public override void EnterState(PlayerStateManager player)
    {
        player.anim.Play(WALKING_ANIMATION);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        Vector2 direction = player.controls.move;

        var forward = player.cam.transform.forward;
        var right = player.cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * direction.y + right * direction.x;

        player.character.Move(desiredMoveDirection * walkingSpeed * Time.deltaTime);
        
        if (direction.magnitude < 0.2f) { player.SwitchState(player.IdlingState); }
        if (player.controls.isRunPressed) { player.SwitchState(player.RunningState); }
    }
    public override void ExitState(PlayerStateManager player)
    {

    }
}
