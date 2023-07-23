using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBehaviour : MonoBehaviour
{
    //Camera variables
    Transform cam;

    //Animator variables
    Animator anim;
    private void Start()
    {
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
    }
    public void handleRotation(Vector2 direction)
    {
        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * direction.y + right * direction.x;

        if (direction.magnitude > 0.2f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), 10f * Time.deltaTime);
        }
    }
}
