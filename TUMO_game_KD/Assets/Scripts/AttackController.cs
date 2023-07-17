using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackController : MonoBehaviour
{
    public Attack currentAttack;
    [SerializeField]
    private GameObject prefabHurtbox;
    public CapsuleCollider capsuleCollider;

    public string user;
    public float damage;
    public float height;
    public float radius;
    public float duration;

    public abstract void Start();

    public void DrawCapsule(Vector3 _pos, float _radius, Color _color = default(Color))
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(_pos, _radius);
        Gizmos.DrawSphere(_pos + transform.forward * radius, _radius);
    }
}
