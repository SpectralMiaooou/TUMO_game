using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility : MonoBehaviour
{
    public Weapon weapon;
    public int attackType;
    Animator anim;
    public PlayerController player;

    //Attack variables
    /*
    public Weapon weapon;
    private bool isAttacking;*/
    private Attack currentAttack;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        handleAttack();
    }

    public void Attack()
    {
        GameObject attackObject = currentAttack.attackManager;
        GameObject _object = Instantiate(attackObject, transform.position + currentAttack.offset, transform.rotation);
        AttackController h = _object.GetComponent<AttackController>();
        h.user = gameObject;
        h.maxRange = currentAttack.maxRange;
        h.radius = currentAttack.radius;
        h.damage = currentAttack.attackDamage;
    }

    public void handleAttack()
    {
        if(player.isPrimaryAttackPressed)
        {
            currentAttack = weapon.primaryAttack;
        }
        else if(player.isSecondaryAttackPressed)
        {
            currentAttack = weapon.secondaryAttack;
        }
        else if(player.isUltimateAttackPressed)
        {
            currentAttack = weapon.ultimateAttack;
        }
        else
        {
            return;
        }

        anim.SetBool("isAttacking", true);
        anim.SetBool("canMove", false);
        anim.Play("Attacks");
        anim.Play(currentAttack.attackAnimation);
    }
    public void disableAttack()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("canMove", true);
    }
}
