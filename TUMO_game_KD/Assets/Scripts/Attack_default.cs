using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_default : AttackController
{
    public override void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.center = new Vector3(0f, 0f, height / 2);
        capsuleCollider.radius = radius;
        capsuleCollider.height = height;
        capsuleCollider.direction = 2;
        //DrawCapsule(transform.position, radius, Color.green);
        Debug.Log("hey");
    }

    private void OnDrawGizmos()
    {/*
        // Calculate the center position of the box
        Vector3 center = transform.position + transform.forward * (2f / 2f);

        // Draw the box gizmo
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(radius, radius, height));*/
    }
}
