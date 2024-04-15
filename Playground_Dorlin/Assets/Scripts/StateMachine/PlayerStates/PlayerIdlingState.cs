using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdlingState : PlayerBaseState
{
    private readonly int IDLING_ANIMATION = Animator.StringToHash("idling");

    public override void EnterState(PlayerStateManager player)
    {
        player.anim.Play(IDLING_ANIMATION);
    }
    public override void UpdateState(PlayerStateManager player)
    {

        if (player.controls.move.magnitude > 0.2f) { player.SwitchState(player.WalkingState); }
        if (player.controls.move.magnitude > 0.7f && player.controls.isRunPressed) { player.SwitchState(player.RunningState); }

        if (!player.character.isGrounded && player.gravity.currentHeight < 0) { player.SwitchState(player.FallingState); }
        if (!player.character.isGrounded && player.gravity.currentHeight > 0) { player.SwitchState(player.JumpingState); }
    }
    public override void ExitState(PlayerStateManager player)
    {

    }
}
