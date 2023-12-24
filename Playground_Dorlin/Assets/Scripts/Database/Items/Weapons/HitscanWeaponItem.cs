using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hitscan Weapon Item", menuName = "Asset/Hitscan Weapon Item")]
public class HitscanWeaponItem : WeaponItem
{
    [Header("Hitscan Weapon Settings")]
    public float weaponFireRate;
    public float weaponReloadCooldown;
    public float weaponDamage;
    public float weaponRange;
    public int weaponCost;
    public AnimatorOverrideController shootAction;
    public AnimatorOverrideController scopeAction;
    public AnimatorOverrideController reloadAction;
}
