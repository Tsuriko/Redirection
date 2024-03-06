using UnityEngine;


public class CustomRDWTake3 : MonoBehaviour
{
    public Transform vrCamera;
    public Transform vrCameraParent;
    public Transform virtualObject;
    public Transform realObject;

    public float alignmentThresholdDistance = 0.2f;
    public bool redirectTurnsOffAfterAlignment = true;
    public bool adjustPlayerMovement;
    public float redirectIntensity = 1.0f; // Intensity of the redirection, range [0, 1]

    private Quaternion previousCameraRotation;
    private Vector3 previousPosition;
    private float initialDistanceToVirtualObject;
    private float initialAngleDifference;
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

        float currentDistanceToVirtualObject = HorizontalDistance(vrCamera.position, virtualObject.position);
        float distanceRatio = currentDistanceToVirtualObject / initialDistanceToVirtualObject;
        float adjustedAngleDifference = initialAngleDifference * (1 - distanceRatio) * redirectIntensity;

        if (currentDistanceToVirtualObject > alignmentThresholdDistance)
        {
            Quaternion targetRotation = Quaternion.Euler(0, adjustedAngleDifference, 0);
            vrCameraParent.rotation = initialParentRotation * targetRotation;
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