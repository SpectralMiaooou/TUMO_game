using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    public Weapon weapon;
    Animator anim;
    public WeaponBehaviour behaviour;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        behaviour.weapon = weapon;
        behaviour.anim = anim;
    }

    void Update()
    {
        behaviour.LaunchAttack();
    }

    public void handleAttack(int type)
    {
        behaviour.Attack(type);
    }
}
