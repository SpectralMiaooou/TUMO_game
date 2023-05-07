using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthLife : MonoBehaviour
{
    //HealthLife variables
    private float maxHealthLife = 100f;
    public float healthLife;

    public void TakeDamage(float _damage)
    {
        healthLife -= _damage;
    }

    public float GetHealthPercent()
    {
        return healthLife / maxHealthLife;
    }
}
