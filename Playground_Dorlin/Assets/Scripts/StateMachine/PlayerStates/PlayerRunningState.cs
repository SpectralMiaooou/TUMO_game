using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    private readonly int RUNNING_ANIMATION = Animator.StringToHash("running");

    //Movement variables
    private float runningSpeed = 10f;
    public Vector3 desiredMoveDirection;

    public override void EnterState(PlayerStateManager player)
    {
        player.anim.Play(RUNNING_ANIMATION);
    }
    public override void UpdateState(PlayerStateManager player)
    {
        Vector2 direction = player.controls.move;

        desiredMoveDirection = Vector3.zero;

        var forward = player.cam.transform.forward;
        var right = player.cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = forward * direction.y + right * direction.x;

        if (direction.magnitude > 0.7f)
        {
            player.character.Move(desiredMoveDirection * runningSpeed * Time.deltaTime);
        }
        else if (direction.magnitude > 0.2f)
        {
            player.character.Move(desiredMoveDirection * runningSpeed * Time.deltaTime);
            player.SwitchState(player.WalkingState);
        }
        else
        {
            player.SwitchState(player.IdlingState);
        }
    }
    public override void ExitState(PlayerStateManager player)
    {

    }
}
