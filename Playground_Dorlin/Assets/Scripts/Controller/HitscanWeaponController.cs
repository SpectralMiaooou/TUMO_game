using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeaponController : WeaponController, IHitscan, IItem
{
    private Transform target;
    private Transform handle;
    private HitscanWeaponItem weapon;
    private float lastTimeShot;

    public Item GetItem()
    {
        return (weapon);
    }

    public void Shoot(UserProfile profile)
    {
        if (Time.time - lastTimeShot > 1/weapon.weaponFireRate)
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
}
