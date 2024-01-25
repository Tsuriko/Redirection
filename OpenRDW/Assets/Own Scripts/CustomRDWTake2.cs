using UnityEngine;

public class CustomRDWTake2 : MonoBehaviour
{
    public Transform vrCamera;
    public Transform vrCameraParent; // New variable for the parent object
    public Transform virtualObject;
    public Transform realObject;

    public float redirectionStrength = 0.5f;
    public float alignmentThresholdAngle = 5.0f;
    public float minRotationSpeed = 1.0f;

    // Thresholds for movement and rotation
    public float movementThreshold = 0.01f; // in meters
    public float rotationThreshold = 0.5f; // in degrees

    private Quaternion previousCameraRotation;
    private bool isAligned = false;

    // Added for tracking and redirection
    private Vector3 previousPosition;
    private Vector3 previousDirection;
    private float accumulatedRotation = 0.0f;

    private void Start()
    {
        if (!vrCamera || !vrCameraParent || !virtualObject || !realObject)
        {
            Debug.LogError("Ensure all transforms are set in the script!");
            return;
        }
        previousCameraRotation = vrCamera.rotation;
        previousPosition = vrCamera.position;
        previousDirection = vrCamera.forward;
    }

    private void Update()
    {
        if (!isAligned && HasPlayerMovedOrRotated())
        {
            AlignVirtualAndRealObjects();
        }
        else if (isAligned)
        {
            AdjustPlayerMovement();
        }

        previousCameraRotation = vrCamera.rotation;
        previousPosition = vrCamera.position;
        previousDirection = vrCamera.forward;
    }

    private bool HasPlayerMovedOrRotated()
    {
        bool hasMoved = Vector3.Distance(vrCamera.position, previousPosition) > movementThreshold;
        bool hasRotated = Quaternion.Angle(vrCamera.rotation, previousCameraRotation) > rotationThreshold;

        return hasMoved || hasRotated;
    }

    private void AlignVirtualAndRealObjects()
    {
        Vector3 directionToVirtual = (virtualObject.position - vrCamera.position).normalized;
        Vector3 directionToReal = (realObject.position - vrCamera.position).normalized;

        float angleToVirtual = Vector3.SignedAngle(vrCamera.forward, directionToVirtual, Vector3.up);
        float angleToReal = Vector3.SignedAngle(vrCamera.forward, directionToReal, Vector3.up);

        float angleDifference = Mathf.DeltaAngle(angleToVirtual, angleToReal);

        if (Mathf.Abs(angleDifference) > alignmentThresholdAngle)
        {
            float dynamicRotationSpeed = redirectionStrength * Mathf.Abs(angleDifference) * Time.deltaTime;
            float rotationSpeed = Mathf.Max(dynamicRotationSpeed, minRotationSpeed * Time.deltaTime);
            float rotationDirection = angleDifference > 0 ? -1 : 1;

            // Rotate the parent of the camera instead of the camera itself
            vrCameraParent.Rotate(Vector3.up, rotationSpeed * rotationDirection);
        }
        else
        {
            isAligned = true;
        }
    }

    private void AdjustPlayerMovement()
    {
        Vector3 realWorldMovement = vrCamera.position - previousPosition;
        float distanceToVirtual = Vector3.Distance(vrCamera.position, virtualObject.position);
        float distanceToReal = Vector3.Distance(vrCamera.position, realObject.position);

        float scalingFactor = distanceToVirtual / distanceToReal;
        Vector3 scaledMovement = realWorldMovement * scalingFactor;

        // Apply the scaled movement to the parent of the VR camera
        vrCameraParent.position += scaledMovement;
    }
}