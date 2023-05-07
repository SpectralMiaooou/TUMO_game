using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Asset/Attack")]

public class Attack : ScriptableObject
{
    public string attackID;
    public string attackName;
    public string attackAnimation;
    public int attackType;
    public int attackCost;
    public float attackDamage;
    [TextArea]
    public string attackDescription;
}