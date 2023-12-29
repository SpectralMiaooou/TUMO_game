using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public PlayerController player;
    public Camera cam;
    [SerializeField]
    private Transform cameraPivotTransform;

    [Header("Camera Settings")]
    private float cameraSmoothSpeed = 1;
    [SerializeField]
    private float RotationSpeedY = 220;
    [SerializeField]
    private float RotationSpeedX = 220;
    [SerializeField]
    private float minimumPivot = -30;
    [SerializeField]
    private float maximumPivot = 60;
    [SerializeField]
    private float cameraCollisionRadius = 0.2f;
    [SerializeField]
    private LayerMask collideWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField]
    private float LookAngleY;
    [SerializeField]
    private float LookAngleX;
    private float defaultCameraPosition;
    private float targetCameraPosition;
    private float cameraZPosition;
    private float targetCameraZPosition;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        cameraZPosition = cam.transform.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            HandleFollowTarget();
            HandleRotations();
            HandleCollisions();
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        LookAngleY += (player.controls.mouseInput.x * RotationSpeedX) * Time.deltaTime;
        LookAngleX -= (player.controls.mouseInput.y * RotationSpeedY) * Time.deltaTime;
        LookAngleX = Mathf.Clamp(LookAngleX, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;
        
        cameraRotation.y = LookAngleY;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraRotation = Vector3.zero;
        cameraRotation.x = LookAngleX;
        targetRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = targetRotation;
    }

    private void HandleCollisions()
    {
        targetCameraZPosition = cameraZPosition;
        RaycastHit hit;
        Vector3 direction = cam.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
        {
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
        }

        if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
        {
            targetCameraZPosition = -cameraCollisionRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cam.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cam.transform.localPosition = cameraObjectPosition;
    }
}
