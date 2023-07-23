using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    public Weapon weapon;
    Animator anim;

    private Attack currentAttack;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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

    public void handleAttack(int type)
    {
        if (type == 1)
        {
            currentAttack = weapon.primaryAttack;
        }
        else if (type == 2)
        {
            currentAttack = weapon.secondaryAttack;
        }
        else if (type == 3)
        {
            currentAttack = weapon.ultimateAttack;
        }

        anim.SetBool("isAttacking", true);
        anim.SetBool("canMove", false);

        //anim.Play("Attacks");
        anim.Play(currentAttack.attackAnimation);
    }
    public void disableAttack()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("canMove", true);
    }

}
