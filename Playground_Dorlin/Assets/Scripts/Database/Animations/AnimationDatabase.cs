using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aniamtion Database", menuName = "Asset/Databases/Animation Database")]
public class AnimationDatabase : ScriptableObject
{
    public List<AnimationSO> allAnimations;
}
