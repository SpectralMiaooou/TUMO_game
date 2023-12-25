using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Weapon Item", menuName = "Asset/Projectile Weapon Item")]
public class ProjectileWeaponItem : WeaponItem
{
    [Header("Projectile Weapon Settings")]
    public float weaponReloadCooldown;
    public float weaponDamage;
    public float weaponProjectileSpeed;
    public float weaponProjectileGravity;
    public float weaponProjectileLifeTime;
    public GameObject weaponProjectileGO;
    public int weaponCost;
    public AnimatorOverrideController shootAction;
    public AnimatorOverrideController scopeAction;
    public AnimatorOverrideController reloadAction;
}
