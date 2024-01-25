using UnityEngine;

public class CustomRDWTake3 : MonoBehaviour
{
    public Transform vrCamera;
    public Transform vrCameraParent;
    public Transform virtualObject;
    public Transform realObject;

    public float alignmentThresholdAngle = 5.0f;

    public float movementThreshold = 0.01f; // in meters
    public float rotationThreshold = 0.5f; // in degrees

    private Quaternion previousCameraRotation;
    private bool isAligned = false;

    private Vector3 previousPosition;
    private float initialDistanceToVirtualObject;
    private float initialAngleDifference;
    private Quaternion initialParentRotation;

    private void Start()
    {
        if (!vrCamera || !vrCameraParent || !virtualObject || !realObject)
        {
            Debug.LogError("Ensure all transforms are set in the script!");
            return;
        }

        previousCameraRotation = vrCamera.rotation;
        previousPosition = vrCamera.position;
        initialDistanceToVirtualObject = HorizontalDistance(vrCamera.position, virtualObject.position);
        initialAngleDifference = CalculateInitialAngleDifference();
        initialParentRotation = vrCameraParent.rotation;

        Debug.Log("Initial distance to virtual object: " + initialDistanceToVirtualObject);
        Debug.Log("Initial angle difference: " + initialAngleDifference);
    }

    private float CalculateInitialAngleDifference()
    {
        Vector3 directionToVirtual = (virtualObject.position - vrCamera.position).normalized;
        Vector3 directionToReal = (realObject.position - vrCamera.position).normalized;
        return Vector3.SignedAngle(directionToReal, directionToVirtual, Vector3.up);
    }

    private void Update()
    {
        if (!isAligned)
        {
            AlignVirtualAndRealObjects();
        }
        else if (isAligned)
        {
            AdjustPlayerMovement();
        }

        previousCameraRotation = vrCamera.rotation;
        previousPosition = vrCamera.position;
    }

    private float HorizontalDistance(Vector3 pointA, Vector3 pointB)
    {
        Vector3 horizontalPointA = new Vector3(pointA.x, 0, pointA.z);
        Vector3 horizontalPointB = new Vector3(pointB.x, 0, pointB.z);
        return Vector3.Distance(horizontalPointA, horizontalPointB);
    }

    private void AlignVirtualAndRealObjects()
    {
        float currentDistanceToVirtualObject = HorizontalDistance(vrCamera.position, virtualObject.position);
        float distanceRatio = currentDistanceToVirtualObject / initialDistanceToVirtualObject;
        float adjustedAngleDifference = initialAngleDifference * (1 - distanceRatio);

        if (Mathf.Abs(adjustedAngleDifference) > alignmentThresholdAngle)
        {
            Quaternion targetRotation = Quaternion.Euler(0, adjustedAngleDifference, 0);
            vrCameraParent.rotation = initialParentRotation * targetRotation;

            Debug.Log("Setting VR Camera Parent rotation. Target rotation: " + targetRotation);
        }
        else
        {
            //isAligned = true;
            Debug.Log("Alignment achieved");
        }
    }

    private void AdjustPlayerMovement()
    {
        Vector3 realWorldMovement = vrCamera.position - previousPosition;
        float distanceToVirtual = HorizontalDistance(vrCamera.position, virtualObject.position);
        float distanceToReal = HorizontalDistance(vrCamera.position, realObject.position);

        float scalingFactor = distanceToVirtual / distanceToReal;
        Vector3 scaledMovement = realWorldMovement * scalingFactor;

        vrCameraParent.position += scaledMovement;

        Debug.Log("Adjusting player movement. Scaled movement: " + scaledMovement);
    }
}