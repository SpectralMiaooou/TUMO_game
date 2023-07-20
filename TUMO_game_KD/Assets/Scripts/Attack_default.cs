using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_default : AttackController
{
    public override void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.center = new Vector3(0f, 0f, maxRange / 2);
        capsuleCollider.radius = radius;
        capsuleCollider.height = maxRange;
        capsuleCollider.direction = 2;
        //DrawCapsule(transform.position, radius, Color.green);
        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {

        // Draw the box gizmo
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.matrix = this.transform.localToWorldMatrix;/*
        UnityEditor.Handles.DrawWireCube(new Vector3( 0f, 0f, height/2), new Vector3(radius*2, radius*2, height));*/

        UnityEditor.Handles.DrawWireArc(Vector3.forward * (maxRange - radius), Vector3.up, Vector3.left, 180, radius);
        UnityEditor.Handles.DrawLine(new Vector3(-radius, 0f, radius), new Vector3(-radius, 0f, maxRange - radius));
        UnityEditor.Handles.DrawLine(new Vector3(radius, 0f, radius), new Vector3(radius, 0f, maxRange - radius));
        UnityEditor.Handles.DrawWireArc(Vector3.forward * (maxRange - radius), Vector3.left, Vector3.up, -180, radius);
        //draw frontways
        UnityEditor.Handles.DrawWireArc(Vector3.forward * radius, Vector3.up, Vector3.left, -180, radius);
        UnityEditor.Handles.DrawLine(new Vector3(0f, -radius, radius), new Vector3(0f, -radius, maxRange - radius));
        UnityEditor.Handles.DrawLine(new Vector3(0f, radius, radius), new Vector3(0f, radius, maxRange - radius));
        UnityEditor.Handles.DrawWireArc(Vector3.forward * radius, Vector3.left, Vector3.up, 180, radius);
        //draw center
        UnityEditor.Handles.DrawWireDisc(Vector3.forward * radius, Vector3.forward, radius);
        UnityEditor.Handles.DrawWireDisc(Vector3.forward * (maxRange - radius), Vector3.forward, radius);
    }
}
