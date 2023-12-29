using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : WeaponController, ISwingable, IItem
{
    public MeleeWeaponItem weapon;

    //private float comboCooldown = 0.5f;

    //public float lastComboEnd;

    public int comboCounter = 0;
    public bool canCombo = false;
    public Item GetItem()
    {
        return (weapon);
    }

    void Update()
    {

    }

    public void Swing(UserProfile profile)
    {
        Animator anim = profile.anim;

        if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            comboCounter = 0;
        }
        if ((anim.GetInteger("comboCounter") == 0 && !anim.GetBool("isAttacking")) || anim.GetBool("canDoCombo"))
        {
            anim.SetInteger("comboCounter", comboCounter++);

            anim.runtimeAnimatorController = weapon.weaponPrimaryAnimOV;
            anim.SetBool("isAttacking", true);
            anim.SetBool("canDoCombo", false);
            anim.SetBool("canMove", false);
            anim.Play("Attack_" + comboCounter);
            //profile.anim.SetTrigger("Attack");

            RaycastHit hit;
            if (Physics.SphereCast(transform.position, weapon.weaponRadius, transform.forward, out hit, weapon.weaponRange))
            {
                HealthBehaviour life = hit.transform.GetComponent<HealthBehaviour>();
                if (life != null)
                {
                    life.TakeDamage(weapon.weaponDamage);
                }
            }
            if (comboCounter >= 3)
            {
                comboCounter = 0;
            }
        }
    }
}
