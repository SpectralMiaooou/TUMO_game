using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FALLING_ANIMATION = Animator.StringToHash("falling");

    public override void EnterState(PlayerStateManager player)
    {
        player.anim.Play(FALLING_ANIMATION);
    }
    public override void UpdateState(PlayerStateManager player)
    {

    }
    public override void ExitState(PlayerStateManager player)
    {

    }
}
