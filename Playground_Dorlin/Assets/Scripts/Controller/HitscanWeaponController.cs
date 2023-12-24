using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeaponController : MonoBehaviour, IHitscan
{
    private Transform target;
    private HitscanWeaponItem weaponData;

    public void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(target.position, target.forward, out hit, weaponData.maxRange))
        {

        }
    }
}
