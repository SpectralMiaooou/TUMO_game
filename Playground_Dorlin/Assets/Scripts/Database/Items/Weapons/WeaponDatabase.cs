using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Database", menuName = "Asset/Databases/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
public List<WeaponItem> allWeapons;
}
