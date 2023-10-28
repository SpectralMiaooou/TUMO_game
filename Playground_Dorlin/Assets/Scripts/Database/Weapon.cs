using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Asset/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponID;
    public string weaponName;
    public List<Attack> primaryAttack;
    public List<Attack> secondaryAttack;
    public List<Attack> ultimateAttack;
    public float maxRange;
}
