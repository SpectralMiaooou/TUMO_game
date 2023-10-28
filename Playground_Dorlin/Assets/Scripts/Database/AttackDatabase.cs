using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Database", menuName = "Asset/Databases/Attack Database")]
public class AttackDatabase : ScriptableObject
{
    public List<Attack> allAttacks;
}
