using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLife
{
    //HealthLife variables
    public float maxHealthLife = 1000f;
    public float healthLife;
    public float shieldLife;

    void Start()
    {
        healthLife = maxHealthLife;
        shieldLife = 0f;
    }

    public void TakeDamage(float _damage)
    {
        healthLife -= _damage;
    }

    public float GetHealthPercent()
    {
        return healthLife / maxHealthLife;
    }
}
