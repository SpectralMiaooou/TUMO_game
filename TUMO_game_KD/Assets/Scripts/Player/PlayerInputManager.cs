using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerController controller;

    private void Awake()
    {
        controls = new PlayerControls();
        controller = GetComponent<PlayerController>();

        controls.Gameplay.PrimaryAttack.started += ctx => controller.StartPrimaryAttack();
        controls.Gameplay.PrimaryAttack.canceled += ctx => controller.StopPrimaryAttack();

        controls.Gameplay.SecondaryAttack.started += ctx => controller.StartSecondaryAttack();
        controls.Gameplay.SecondaryAttack.canceled += ctx => controller.StopSecondaryAttack();

        controls.Gameplay.UltimateAttack.started += ctx => controller.StartUltimateAttack();
        controls.Gameplay.UltimateAttack.canceled += ctx => controller.StopUltimateAttack();

        controls.Gameplay.Jump.started += ctx => controller.StartJump();
        controls.Gameplay.Jump.canceled += ctx => controller.StopJump();

        controls.Gameplay.Run.started += ctx => controller.StartRun();
        controls.Gameplay.Run.canceled += ctx => controller.StopRun();

        controls.Gameplay.Movement.performed += ctx => controller.SetMovement(ctx.ReadValue<Vector2>());
        controls.Gameplay.Movement.canceled += ctx => controller.SetMovement(Vector2.zero);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
