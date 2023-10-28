using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    public Weapon weapon;
    Animator anim;

    private Attack currentAttack;

    public List<Attack> combo;
    private float lastComboEnd;
    private float lastClickedTime;
    private int comboCounter = 0;
    private bool canCombo = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Attack()
    {
        //Demander au scriptableobjet Attack behaviour comment attaquer
        //Ex: Tirer avec armes, coup de hache, tir � l'arc, ou 



        /*
        GameObject attackObject = currentAttack.attackManager;
        GameObject _object = Instantiate(attackObject, transform.position + currentAttack.offset, transform.rotation);
        AttackController h = _object.GetComponent<AttackController>();
        h.user = gameObject;
        h.maxRange = currentAttack.maxRange;
        h.radius = currentAttack.radius;
        h.damage = currentAttack.attackDamage;*/
    }

    void Update()
    {
        LauchAttack();
    }

    public void handleAttack(int type)
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
            if(Time.time - lastClickedTime >= 1f)
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

    void LauchAttack()
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
