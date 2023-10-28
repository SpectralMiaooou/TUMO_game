using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MeleeWeapon", menuName = "Asset/Behaviour/MeleeWeapon")]

public class MeleeWeaponData : WeaponBehaviour
{
    public List<Attack> combo;
    private float lastComboEnd;
    private float lastClickedTime;
    private int comboCounter = 0;
    private bool canCombo = false;

    public override void Attack(int type)
    {
        if (type == 1)
        {
            combo = weapon.primaryAttack;
        }
        else if (type == 2)
        {
            combo = weapon.secondaryAttack;
        }
        else if (type == 3)
        {
            combo = weapon.ultimateAttack;
        }
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if (comboCounter >= combo.Count)
            {
                comboCounter = 0;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            {
                canCombo = true;
                anim.SetBool("isAttacking", true);
                anim.SetBool("canMove", false);
            }
        }
        else
        {
            if(Time.time - lastComboEnd >= 1f)
            {
                anim.runtimeAnimatorController = combo[comboCounter].attackAnimation;
                anim.Play("Attack", 0, 0);
                comboCounter++;
                lastClickedTime = Time.time;
                anim.SetBool("isAttacking", true);
                anim.SetBool("canMove", false);
            }
        }
    }

    public override void LaunchAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            if(canCombo)
            {
                anim.runtimeAnimatorController = combo[comboCounter].attackAnimation;
                anim.Play("Attack", 0, 0);
                comboCounter++;
                lastClickedTime = Time.time;
                canCombo = false;
            }
            else
            {
                comboCounter = 0;
                lastComboEnd = Time.time;
                anim.SetBool("isAttacking", false);
                anim.SetBool("isRunning", false);
                anim.SetBool("canMove", true);
            }
        }
    }
}
