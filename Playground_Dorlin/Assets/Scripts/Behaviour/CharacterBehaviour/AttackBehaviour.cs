using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //behaviour.LaunchAttack();
    }

    public void handleAttack(InventoryBehaviour slot, InputHandler input)
    {
        GameObject weapon = slot.currentWeapon.item;
        if (input.isPrimaryAttackPressed)
        {
            if (weapon.TryGetComponent<ISwingable>(out ISwingable swingable))
            {
                swingable?.Swing();
            }
            else if (weapon.TryGetComponent<IHitscan>(out IHitscan hitscan))
            {
                hitscan?.Shoot();
            }
            else if (weapon.TryGetComponent<IProjectile>(out IProjectile projectile))
            {
                projectile?.Throw();
            }
            anim.SetBool("isAttacking", true);
        }
        else if (input.isSecondaryAttackPressed)
        {
            //attack.handleAttack(2);
        }
        else if (input.isUltimateAttackPressed)
        {
            //attack.handleAttack(3);
        }
    }
}
