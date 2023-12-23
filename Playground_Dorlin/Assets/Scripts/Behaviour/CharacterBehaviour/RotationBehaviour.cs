using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBehaviour : MonoBehaviour
{
    //Animator variables
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void handleRotation(Vector2 direction, Transform offset)
    {
        Vector3 forward;
        Vector3 right;
        if (offset == null)
        {
            forward = Vector3.forward;
            right = Vector3.right;
        }
        else
        {
            forward = offset.transform.forward;
            right = offset.transform.right;
        }

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
