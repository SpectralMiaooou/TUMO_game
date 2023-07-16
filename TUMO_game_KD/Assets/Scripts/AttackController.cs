using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    public Attack currentAttack;
    [SerializeField]
    private GameObject prefabHurtbox;

    public abstract void Attack();

    public void HurtboxEnable()
    {
        GameObject hurtbox = Instantiate(prefabHurtbox, transform.position, Quaternion.identity);
        HitboxDamageManager h = hurtbox.GetComponent<HitboxDamageManager>();
        h.height = currentAttack.height;
        h.radius = currentAttack.radius;
        h.damage = currentAttack.attackDamage;
    }
}
