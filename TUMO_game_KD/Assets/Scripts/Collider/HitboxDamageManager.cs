using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDamageManager : MonoBehaviour
{
    public float damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void handleAttackDamage(float _damage)
    {
        damage = _damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HealthLife>() != null)
        {
            //
        }
    }
}
