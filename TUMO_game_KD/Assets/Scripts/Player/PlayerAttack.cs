using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    PlayerMovement player;

    //Animator variables
    Animator anim;

    //CharacterController variables
    CharacterController character;

    //Attack variables
    public bool isPrimaryAttackPressed = false;
    public bool isSecondaryAttackPressed = false;
    public bool isUltimateAttackPressed = false;

    //Attack variables
    public GameObject weaponObject;
    private AttackAbility weaponAbility;
    private Weapon weapon;
    private bool isAttacking;

    //Input variables
    public PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.PrimaryAttack.started += onPrimaryAttack;
        controls.Gameplay.PrimaryAttack.canceled += onPrimaryAttack;

        controls.Gameplay.SecondaryAttack.started += onSecondaryAttack;
        controls.Gameplay.SecondaryAttack.canceled += onSecondaryAttack;

        controls.Gameplay.UltimateAttack.started += onUltimateAttack;
        controls.Gameplay.UltimateAttack.canceled += onUltimateAttack;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerMovement>();
        weaponAbility = weaponObject.GetComponent<AttackAbility>();
        weapon = weaponAbility.weapon;
        character = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("isGrounded", character.isGrounded);
        handleAttack();
        handleAnimation();
    }
    void handleAnimation()
    {
        isAttacking = anim.GetBool("isAttacking");
    }

    void handleAttack()
    {
        if (character.isGrounded && !isAttacking)
        {
            if (isPrimaryAttackPressed)
            {
                player.canMove = false;
                anim.SetBool("isAttacking", true);
                anim.Play("Attacks");
                anim.Play(weapon.primaryAttack.attackAnimation);
                //activeAttackHit();
            }
            if (isSecondaryAttackPressed)
            {
                player.canMove = false;
                anim.SetBool("isAttacking", true);
                anim.Play("Attacks");
                anim.Play(weapon.secondaryAttack.attackAnimation);
                //
            }
            if (isUltimateAttackPressed)
            {
                player.canMove = false;
                anim.SetBool("isAttacking", true);
                //
            }
        }
    }

    void onPrimaryAttack(InputAction.CallbackContext context)
    {
        isPrimaryAttackPressed = context.ReadValueAsButton();
    }
    void onSecondaryAttack(InputAction.CallbackContext context)
    {
        isSecondaryAttackPressed = context.ReadValueAsButton();
    }
    void onUltimateAttack(InputAction.CallbackContext context)
    {
        isUltimateAttackPressed = context.ReadValueAsButton();
    }
}
