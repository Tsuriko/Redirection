using UnityEngine;


public class CustomRDWTake3 : MonoBehaviour
{
    public Transform vrCamera;
    public Transform vrCameraParent;
    public Transform virtualObject;
    public Transform realObject;

    public float alignmentThresholdDistance = 0.1f;
    public bool redirectTurnsOffAfterAlignment = true;
    public bool adjustPlayerMovement;
    public float redirectIntensity = 1.0f;
    public float adjustedAngleDifference;

    private Quaternion previousCameraRotation;
    private Vector3 previousPosition;
    public float initialDistanceToVirtualObject;
    public float currentDistanceToVirtualObject;
    public float initialAngleDifference;
    private Quaternion initialParentRotation;
    private float translativeGainFactor;
    public bool alignmentAchieved;

    private void Start()
    {
        if (!vrCamera || !vrCameraParent || !virtualObject || !realObject)
        {
            Debug.LogError("Ensure all transforms are set in the script!");
            return;
        }
        InitializeRedirection();
    }

    public void InitializeRedirection()
    {
        previousCameraRotation = vrCamera.rotation;
        previousPosition = vrCamera.position;
        initialDistanceToVirtualObject = HorizontalDistance(vrCamera.position, virtualObject.position);
        initialAngleDifference = CalculateInitialAngleDifference();
        initialParentRotation = vrCameraParent.rotation;
        float distanceToVirtual = HorizontalDistance(vrCamera.position, virtualObject.position);
        float distanceToReal = HorizontalDistance(vrCamera.position, realObject.position);
        translativeGainFactor = distanceToVirtual / distanceToReal;
        alignmentAchieved = false;
        adjustPlayerMovement = true;
    }

    private float CalculateInitialAngleDifference()
    {
        Vector3 directionToVirtual = (virtualObject.position - vrCamera.position).normalized;
        Vector3 directionToReal = (realObject.position - vrCamera.position).normalized;
        return Vector3.SignedAngle(directionToReal, directionToVirtual, Vector3.up);
    }

    private void Update()
    {
        if (!alignmentAchieved || redirectTurnsOffAfterAlignment)
        {
            AlignVirtualAndRealObjects();
        }
        if (adjustPlayerMovement)
        {
            //AdjustPlayerMovement();
        }

        previousCameraRotation = vrCamera.rotation;
        previousPosition = vrCamera.position;
    }



    private void AlignVirtualAndRealObjects()
    {
        if (alignmentAchieved && redirectTurnsOffAfterAlignment) return;

        currentDistanceToVirtualObject = HorizontalDistance(vrCamera.position, virtualObject.position);

        if (currentDistanceToVirtualObject > alignmentThresholdDistance)
        {
            // Calculate the desired rotation based on distance ratio and adjusted angle difference
            float distanceRatio = currentDistanceToVirtualObject / initialDistanceToVirtualObject;
            adjustedAngleDifference = initialAngleDifference * (1 - distanceRatio) * redirectIntensity;

            // Determine the target rotation as an adjustment from the initial parent rotation
            Quaternion targetRotation = Quaternion.Euler(0, adjustedAngleDifference, 0) * initialParentRotation;

            // Calculate the rotation difference between the current and target orientations
            Quaternion rotationDifference = targetRotation * Quaternion.Inverse(vrCameraParent.rotation);
            rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);

            // Ensure there's a significant rotation to apply
             // A small threshold to avoid floating point imprecision issues
            
                // Apply the rotation difference around the VR camera's position
                vrCameraParent.RotateAround(vrCamera.position, axis, angle);
            
        }
        else
        {
            Debug.Log("Alignment achieved");
            alignmentAchieved = true;
        }
    }
    private float HorizontalDistance(Vector3 pointA, Vector3 pointB)
    {
        Vector3 horizontalPointA = new Vector3(pointA.x, 0, pointA.z);
        Vector3 horizontalPointB = new Vector3(pointB.x, 0, pointB.z);
        return Vector3.Distance(horizontalPointA, horizontalPointB);
    }
    private void AdjustPlayerMovement()
    {
        Vector3 realWorldMovement = vrCamera.position - previousPosition;

        Vector3 scaledMovement = realWorldMovement * translativeGainFactor;

        vrCameraParent.position += scaledMovement;
    }
}