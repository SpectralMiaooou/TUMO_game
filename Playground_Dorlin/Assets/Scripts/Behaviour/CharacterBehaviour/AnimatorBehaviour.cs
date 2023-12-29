using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorBehaviour : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ResetAttack()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("canMove", true);
        anim.SetInteger("comboCounter", 0);
    }
    public void EnableCombo()
    {
        anim.SetBool("canDoCombo", true);
    }
    public void DisableCombo()
    {
        anim.SetBool("canDoCombo", false);
    }
}
