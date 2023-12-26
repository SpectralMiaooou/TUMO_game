using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : WeaponController, ISwingable, IItem
{
    public MeleeWeaponItem weapon;

    private float clickCooldown = 0.15f; //Cooldown to not spam click
    private float comboCooldown = 0.5f; //Cooldown to use again combo

    public float lastClickedTime;
    public float lastComboEnd;

    public int comboCounter = 0;
    public bool canCombo = false;
    private Animator anim;
    public Item GetItem()
    {
        return (weapon);
    }

    void Update()
    {
        if(anim != null)
        {
            ExitAttack();
        }
    }

    public void Swing(UserProfile profile)
    {
        if (Time.time - lastComboEnd >= comboCooldown && comboCounter < weapon.weaponPrimaryAnimOV.Length)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickedTime >= clickCooldown)
            {
                anim = profile.anim;

                anim.runtimeAnimatorController = weapon.weaponPrimaryAnimOV[comboCounter].animatorOV;
                anim.Play("Attack");
                anim.SetBool("isAttacking", true);

                RaycastHit hit;
                if (Physics.SphereCast(transform.position, weapon.weaponRadius, transform.forward, out hit, weapon.weaponRange))
                {
                    HealthBehaviour life = hit.transform.GetComponent<HealthBehaviour>();
                    if (life != null)
                    {
                        life.TakeDamage(weapon.weaponDamage);
                    }
                }
                lastClickedTime = Time.time;
                comboCounter++;

                if (comboCounter >= weapon.weaponPrimaryAnimOV.Length)
                {
                    comboCounter = 0;
                }
            }
        }
    }
 

    void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            anim.SetBool("canCombo", false);
        }
    }

    void EndCombo()
    {
        comboCounter = 0;
        lastComboEnd = Time.time;
        if (anim != null)
        {
            anim.SetBool("isAttacking", false);
        }
        anim = null;
    }
}
