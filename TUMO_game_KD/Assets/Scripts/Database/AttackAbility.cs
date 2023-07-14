using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility : MonoBehaviour
{
    public Weapon weapon;
    public int attackType;

    private Collider damageHitbox;


    // Start is called before the first frame update
    void Start()
    {
        damageHitbox = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        HealthLife life = other.GetComponent<HealthLife>();
        if(life != null)
        {
            if (attackType == 1)
            {
                life.TakeDamage(weapon.primaryAttack.attackDamage);
            }
            if (attackType == 2)
            {
                life.TakeDamage(weapon.secondaryAttack.attackDamage);
            }
            else
            {
                life.TakeDamage(weapon.ultimateAttack.attackDamage);
            }
            DisableAttack();
        }
    }

    public void EnableAttack(int type)
    {
        damageHitbox.enabled = true;
        attackType = type;
    }

    void DisableAttack()
    {
        damageHitbox.enabled = false;
    }
}
