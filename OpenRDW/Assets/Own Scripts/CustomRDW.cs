using UnityEngine;

public class CustomRDW : MonoBehaviour
{
    public Transform vrCamera;
    public Transform virtualObject;
    public Transform realObject;

    public float redirectionRadius = 200.0f;
    public float gazeThresholdAngle = 30.0f;
    public float speedThreshold = 1.0f;
    public float maxVirtualObjectMoveSpeed = 0.5f;
    public float nearAlignmentDistance = 5f; // Distance threshold for rapid alignment

    private Vector3 previousCameraPosition;

    private void Start()
    {
        if (!vrCamera || !virtualObject || !realObject)
        {
            Debug.LogError("Ensure all transforms are set in the script!");
            return;
        }
        previousCameraPosition = vrCamera.position;
    }

    private void Update()
    {
        AlignVirtualObject();
        previousCameraPosition = vrCamera.position;
    }

    private void AlignVirtualObject()
    {
        float distanceToVirtual = Vector3.Distance(vrCamera.position, virtualObject.position);
        float distanceToReal = Vector3.Distance(vrCamera.position, realObject.position);

        Vector3 gazeDirection = vrCamera.forward;
        Vector3 directionToVirtual = (virtualObject.position - vrCamera.position).normalized;
        float angleToVirtual = Vector3.Angle(gazeDirection, directionToVirtual);

        float cameraMovementSpeed = (vrCamera.position - previousCameraPosition).magnitude / Time.deltaTime;

        float dynamicMoveSpeed = Mathf.Lerp(0, maxVirtualObjectMoveSpeed, cameraMovementSpeed);
        dynamicMoveSpeed += maxVirtualObjectMoveSpeed * (1.0f - (distanceToVirtual / redirectionRadius));

        if (distanceToReal <= nearAlignmentDistance)
        {
            dynamicMoveSpeed = Mathf.Max(dynamicMoveSpeed, 5.0f); // Rapid alignment speed
            Debug.Log("Rapid alignment speed");
        }

        if (angleToVirtual > gazeThresholdAngle || cameraMovementSpeed > speedThreshold || distanceToReal <= nearAlignmentDistance)
        {
            virtualObject.position = Vector3.Lerp(virtualObject.position, realObject.position, dynamicMoveSpeed * Time.deltaTime);
        }
    }
}