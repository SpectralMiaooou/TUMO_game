using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDamage : MonoBehaviour
{
    Camera cam;
    public LayerMask mask;
    Vector3 drawSpherePos;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Draw Ray
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 100f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.blue);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log("Click.");

            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                Debug.Log(hit.transform.name);
                drawSpherePos = hit.point;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(drawSpherePos, 0.1f);
    }
}
