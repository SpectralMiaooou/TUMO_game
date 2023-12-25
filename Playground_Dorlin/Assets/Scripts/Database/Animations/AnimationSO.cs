using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animation", menuName = "Asset/Animation")]
public class AnimationSO : ScriptableObject
{
    [Header("Animation Settings")]
    public string animationID;
    public string animationName;
    public AnimatorOverrideController animatorOV;
    [TextArea]
    public string animationDescription;
}
