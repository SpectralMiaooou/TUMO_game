using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Asset/Attack")]

public class Attack : ScriptableObject
{
    public int attackID;
    public string attackName;
    public int attackType;
    [TextArea]
    public string attackDescription;
    public int attackCost;
}
