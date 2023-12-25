using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : WeaponController, ISwingable, IItem
{
    public MeleeWeaponItem weapon;

    public Item GetItem()
    {
        return (weapon);
    }

    public void Swing()
    {
        RaycastHit hit;
        if(Physics.SphereCast(transform.position, weapon.weaponRadius, transform.forward, out hit, weapon.weaponRange))
        {
            HealthBehaviour life = hit.transform.GetComponent<HealthBehaviour>();
            if(life != null)
            {
                life.TakeDamage(weapon.weaponDamage);
            }
        }
    }
}
