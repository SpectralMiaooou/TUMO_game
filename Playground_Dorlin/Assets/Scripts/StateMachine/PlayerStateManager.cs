using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    PlayerBaseState currentState;

    public PlayerIdlingState IdlingState = new PlayerIdlingState();
    public PlayerWalkingState WalkingState = new PlayerWalkingState();
    public PlayerRunningState RunningState = new PlayerRunningState();
    public PlayerJumpingState JumpingState = new PlayerJumpingState();
    public PlayerFallingState FallingState = new PlayerFallingState();
    public PlayerAttackingState AttackingState = new PlayerAttackingState();

    public Animator anim;

    public Transform cam;

    public CharacterController character;

    public GravityBehaviour gravity;

    public InputHandler controls;

    // Start is called before the first frame update
    void Start()
    {
        currentState = IdlingState;

        currentState.EnterState(this);

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(PlayerBaseState state)
    {
        currentState.ExitState(this);

        currentState = state;
        state.EnterState(this);
    }
}
