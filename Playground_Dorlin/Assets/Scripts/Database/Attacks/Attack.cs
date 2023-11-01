using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : ScriptableObject
{
    public string attackID;
    public string attackName;
    [TextArea]
    public string attackDescription;
    public float attackDamage;
    public int attackCost;
    public List<AnimatorOverrideController> attackAnimations;
}