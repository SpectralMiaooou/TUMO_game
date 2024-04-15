using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JUMPING_ANIMATION = Animator.StringToHash("jumping");

    public override void EnterState(PlayerStateManager player)
    {
        player.anim.Play(JUMPING_ANIMATION);
    }
    public override void UpdateState(PlayerStateManager player)
    {

    }
    public override void ExitState(PlayerStateManager player)
    {

    }
}
