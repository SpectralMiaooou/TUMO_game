using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Attack currentAttack;
    [SerializeField]
    private GameObject prefabHurtbox; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HurtboxEnable()
    {
        GameObject hurtbox = Instantiate(prefabHurtbox, transform.position, Quaternion.identity);
        HitboxDamageManager h = hurtbox.GetComponent<HitboxDamageManager>();
        h.height = currentAttack.height;
        h.radius = currentAttack.radius;
        h.damage = currentAttack.attackDamage;
    }
}
