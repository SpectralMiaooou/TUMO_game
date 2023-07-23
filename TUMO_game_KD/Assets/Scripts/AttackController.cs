using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    public Attack currentAttack;
    [SerializeField]
    private GameObject prefabHurtbox;
    public CapsuleCollider capsuleCollider;

    public GameObject user;
    public float damage;
    public float maxRange;
    public float radius;
    public float duration;

    public abstract void Start();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthBehaviour>() != null && other.gameObject != user)
        {
            HealthBehaviour health = other.GetComponent<HealthBehaviour>();
            health.TakeDamage(damage);
            Debug.Log(other.name + ": " + health.healthLife.ToString());
        }
    }
}
