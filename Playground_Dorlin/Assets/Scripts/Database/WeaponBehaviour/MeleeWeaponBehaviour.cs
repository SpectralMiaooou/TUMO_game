using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponBehaviour : IWeapon
{
    public void PrimaryAction()
    {
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
            if (Time.time - lastComboEnd >= 1f)
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

    public void SecondaryAction()
    {
        // aim down the sights
    }

    public void TertiaryAction()
    {
        // reload
    }
}

