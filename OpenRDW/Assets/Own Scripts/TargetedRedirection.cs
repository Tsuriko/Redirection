using UnityEngine;

public class TargetedWalkingRedirection : MonoBehaviour
{
    public float movementThreshold = 0.1f;
    public Transform virtualWorldCameraTransform; // Assign the camera's Transform here.
    public Transform virtualWorldCameraRigTransform; // Assign the camera rig's Transform here.
    public Transform realWorldGoalTransform;
    public Transform virtualWorldGoalTransform; // Assign the virtual goal's Transform here.
    public float rotationGainFactor = 0.1f;
    public float distanceToTriggerGoal = 1.0f;

    private Vector3 previousRealWorldPosition;
    private bool enableRedirection = false;

    private void Start()
    {
        previousRealWorldPosition = new Vector3(virtualWorldCameraRigTransform.position.x, 0, virtualWorldCameraRigTransform.position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            enableRedirection = !enableRedirection;
            Debug.Log("Redirection toggled: " + enableRedirection);
        }

        if (!enableRedirection)
        {
            return;
        }

        Vector3 realWorldPosition = new Vector3(virtualWorldCameraRigTransform.position.x, 0, virtualWorldCameraRigTransform.position.z);

        if (Vector3.Distance(realWorldPosition, previousRealWorldPosition) > movementThreshold)
        {
            Debug.Log("Player has moved beyond the threshold.");

            if (HasReachedGoal(realWorldPosition, realWorldGoalTransform.position, virtualWorldGoalTransform.position))
            {
                Debug.Log("Goal reached!");
                return;
            }

            Vector3 realWorldDesiredDirection = (realWorldGoalTransform.position - realWorldPosition).normalized;
            Vector3 virtualWorldDirection = virtualWorldCameraTransform.forward;

            float redirectionAngle = Vector3.SignedAngle(virtualWorldDirection, realWorldDesiredDirection, Vector3.up);
            float adjustedRedirectionAngle = redirectionAngle * rotationGainFactor;

            Quaternion rotation = Quaternion.Euler(0, adjustedRedirectionAngle, 0);
            virtualWorldCameraTransform.rotation *= rotation;

            Vector3 realWorldMovement = realWorldPosition - previousRealWorldPosition;
            float distanceGain = CalculateDistanceGain(realWorldPosition, realWorldGoalTransform.position, virtualWorldGoalTransform.position);

            Vector3 adjustedVirtualWorldMovement = realWorldMovement * distanceGain;
            virtualWorldCameraTransform.position += adjustedVirtualWorldMovement;
        }

        previousRealWorldPosition = realWorldPosition;
    }

    private float CalculateDistanceGain(Vector3 realWorldPosition, Vector3 realWorldGoal, Vector3 virtualWorldGoal)
    {
        float realWorldDistance = Vector2.Distance(new Vector2(realWorldPosition.x, realWorldPosition.z), new Vector2(realWorldGoal.x, realWorldGoal.z));
        float virtualWorldDistance = Vector2.Distance(new Vector2(virtualWorldCameraRigTransform.position.x, virtualWorldCameraRigTransform.position.z), new Vector2(virtualWorldGoal.x, virtualWorldGoal.z));
        return realWorldDistance / virtualWorldDistance;
    }

    private bool HasReachedGoal(Vector3 realWorldPosition, Vector3 realWorldGoal, Vector3 virtualWorldGoal)
    {
        return Vector2.Distance(new Vector2(realWorldPosition.x, realWorldPosition.z), new Vector2(realWorldGoal.x, realWorldGoal.z)) < distanceToTriggerGoal &&
               Vector2.Distance(new Vector2(virtualWorldCameraRigTransform.position.x, virtualWorldCameraRigTransform.position.z), new Vector2(virtualWorldGoal.x, virtualWorldGoal.z)) < distanceToTriggerGoal;
    }
}