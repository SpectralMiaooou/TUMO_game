using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBehaviour : ScriptableObject
{
    public string weaponBehaviourID;
    public string weaponBehaviourType;
    public string weaponName;

    public abstract void Attack();
}
