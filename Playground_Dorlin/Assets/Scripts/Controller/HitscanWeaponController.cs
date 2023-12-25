using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeaponController : WeaponController, IHitscan, IItem
{
    private Transform target;
    private HitscanWeaponItem weapon;
    public Item GetItem()
    {
        return (weapon);
    }
    public void Shoot(UserProfile profile)
    {
        RaycastHit hit;
        if (Physics.Raycast(target.position, target.forward, out hit, weapon.weaponRange))
        {
            HealthBehaviour life = hit.transform.GetComponent<HealthBehaviour>();
            if (life != null)
            {
                life.TakeDamage(weapon.weaponDamage);
            }

        }
    }
}
