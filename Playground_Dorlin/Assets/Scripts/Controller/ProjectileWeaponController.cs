using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponController : WeaponController, IProjectile, IItem
{
    private Transform target;
    private ProjectileWeaponItem weapon;

    public Item GetItem()
    {
        return (weapon);
    }
    public void Throw()
    {
        GameObject projectile = Instantiate(weapon.weaponProjectileGO, target.position, target.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.AddForce(projectile.transform.forward, ForceMode.Impulse);
    }
}
