using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon Item", menuName = "Asset/Melee Weapon Item")]
public class MeleeWeaponItem : WeaponItem
{
    [Header("Melee Weapon Settings")]
    public float weaponRange;
    public float weaponRadius;
    public float weaponDamage;
    public int weaponCost;
    public List<AnimatorOverrideController> dataAnimations;
}
