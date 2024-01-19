using UnityEngine;

public class CustomRDWTake2 : MonoBehaviour
{
    public Transform vrCamera;
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
    private float accumulatedRotation = 0.0f; // Track applied rotation for redirection

    private void Start()
    {
        if (!vrCamera || !virtualObject || !realObject)
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
        // Check for significant movement
        bool hasMoved = Vector3.Distance(vrCamera.position, previousPosition) > movementThreshold;

        // Check for significant rotation
        bool hasRotated = Quaternion.Angle(vrCamera.rotation, previousCameraRotation) > rotationThreshold;

        return hasMoved || hasRotated;
    }

    // Aligns the angles to the virtual and real objects
private void AlignVirtualAndRealObjects()
{
    // Calculate direction vectors
    Vector3 directionToVirtual = (virtualObject.position - vrCamera.position).normalized;
    Vector3 directionToReal = (realObject.position - vrCamera.position).normalized;

    // Calculate the angles
    float angleToVirtual = Vector3.SignedAngle(vrCamera.forward, directionToVirtual, Vector3.up);
    float angleToReal = Vector3.SignedAngle(vrCamera.forward, directionToReal, Vector3.up);

    // Determine the angle difference and rotation direction
    float angleDifference = Mathf.DeltaAngle(angleToVirtual, angleToReal);


    // Check if alignment is needed
    if (Mathf.Abs(angleDifference) > alignmentThresholdAngle)
    {
        // Calculate dynamic rotation speed based on angle difference
        float dynamicRotationSpeed = redirectionStrength * Mathf.Abs(angleDifference) * Time.deltaTime;

        // Ensure rotation speed is at least the minimum
        float rotationSpeed = Mathf.Max(dynamicRotationSpeed, minRotationSpeed * Time.deltaTime);
        float rotationDirection = angleDifference > 0 ? -1 : 1; // Rotate clockwise or counterclockwise

        // Rotate the camera
        vrCamera.Rotate(Vector3.up, rotationSpeed * rotationDirection);
    }
    else
    {
        // Alignment achieved
        isAligned = true;
    }
}

    // Adjusts the player's movement based on the distance discrepancy
   private void AdjustPlayerMovement()
{
    // Get the real-world movement delta
    Vector3 realWorldMovement = vrCamera.position - previousPosition;

    // Calculate the distances to the virtual and real objects
    float distanceToVirtual = Vector3.Distance(vrCamera.position, virtualObject.position);
    float distanceToReal = Vector3.Distance(vrCamera.position, realObject.position);

    // Determine the scaling factor based on the distance discrepancy
    float scalingFactor = distanceToVirtual / distanceToReal;

    // Scale the real-world movement for the virtual environment
    Vector3 scaledMovement = realWorldMovement * scalingFactor;
    
    // Apply the scaled movement to the VR camera
    vrCamera.position += scaledMovement;
}
}