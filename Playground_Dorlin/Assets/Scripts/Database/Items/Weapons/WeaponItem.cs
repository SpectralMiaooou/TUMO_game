using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "Asset/Weapon Item")]
public class WeaponItem : Item
{
    [Header("Weapon Options")]
    public GameObject weaponPrefab;
    public Attack primaryAttack;
    public Attack secondaryAttack;
    public Attack ultimateAttack;
    public float maxRange;
}
