using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HitscanWeaponController : WeaponController, IHitscan, IItem
{
    public Transform target;
    public Transform handle;
    public HitscanWeaponItem weapon;

    private PlayerController user;
    private float lastTimeShot;

    public Item GetItem()
    {
        return (weapon);
    }

    void OnEnable()
    {
        user.GetComponent<Rig>().weight = 1f;
    }
    void OnDisable()
    {
        user.GetComponent<Rig>().weight = 0f;
    }

    public void Shoot(UserProfile profile)
    {
        if (Time.time - lastTimeShot > 1/weapon.weaponFireRate)
        {
            lastTimeShot = Time.time;
            profile.anim.SetBool("isAttacking", true);
            profile.anim.Play("Attack_Shoot");
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
