using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Asset/Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponID;
    public string weaponName;
    public Attack primaryAttack;
    public Attack secondaryAttack;
    public Attack ultimateAttack;
}
