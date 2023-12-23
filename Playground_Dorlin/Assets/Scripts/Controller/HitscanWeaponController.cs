using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitscanWeaponController : RangedWeaponController, IHitscanable
{
    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(target, target.forward, ))
    }
}
