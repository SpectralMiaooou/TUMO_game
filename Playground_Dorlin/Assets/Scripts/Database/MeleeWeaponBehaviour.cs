using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon Behaviour", menuName = "Asset/Melee Weapon Behaviour")]

public class MeleeWeaponBehaviour : WeaponBehaviour
{
    public override void Attack()
    {
        Debug.Log("Melee Attack");
    }
}
