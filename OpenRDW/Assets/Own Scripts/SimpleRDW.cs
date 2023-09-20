using UnityEngine;

public class SimpleRDW : MonoBehaviour
{
    public float movementThreshold = 0.1f;  // Movement threshold for applying rotation
    public Transform realWorldPlayerTransform;
    public Transform virtualWorldPlayerTransform;
    public Transform realWorldGoalTransform;
    public Transform virtualWorldGoalTransform;
    public float rotationGainFactor = 0.1f;
    public float distanceToTriggerGoal = 1.0f;

    private Vector3 previousRealWorldPosition;
    private bool enableRedirection = false;

    void Start()
    {
        previousRealWorldPosition = new Vector3(realWorldPlayerTransform.position.x, 0, realWorldPlayerTransform.position.z);
    }

    void Update()
    {
        // Toggle redirection with the Enter key
        if (Input.GetKeyDown(KeyCode.Return))
        {
            enableRedirection = !enableRedirection;
            Debug.Log("Redirection toggled: " + enableRedirection);
        }

        if (!enableRedirection)
        {
            return;
        }

        Vector3 realWorldPosition = new Vector3(realWorldPlayerTransform.position.x, 0, realWorldPlayerTransform.position.z);
        Vector3 virtualWorldPosition = new Vector3(virtualWorldPlayerTransform.position.x, 0, virtualWorldPlayerTransform.position.z);
        Quaternion virtualWorldRotation = virtualWorldPlayerTransform.rotation;

        // Check if the player has moved beyond the threshold
        if (Vector3.Distance(realWorldPosition, previousRealWorldPosition) > movementThreshold)
        {
            Debug.Log("Player has moved beyond the threshold.");

            if (HasReachedGoal(realWorldPosition, realWorldGoalTransform.position, virtualWorldPosition, virtualWorldGoalTransform.position))
            {
                Debug.Log("Goal reached!");
                return;
            }

            // Calculate paths to goals
            Vector3 realWorldPath = CalculatePath(realWorldPosition, realWorldGoalTransform.position);
            Vector3 virtualWorldPath = CalculatePath(virtualWorldPosition, virtualWorldGoalTransform.position);

            // Calculate rotational gain and apply
            float rotationGain = CalculateAlignedRotationGain(virtualWorldRotation, realWorldPath, virtualWorldPath);
            Debug.Log("Calculated rotational gain: " + rotationGain);

            Quaternion manipulatedRotation = Quaternion.Euler(0, rotationGain, 0) * virtualWorldRotation;
            virtualWorldPlayerTransform.rotation = manipulatedRotation;

            // Calculate distance gain and apply
            Vector3 realWorldMovement = realWorldPosition - previousRealWorldPosition;
            float distanceGain = CalculateDistanceGain(realWorldPosition, realWorldGoalTransform.position, virtualWorldPosition, virtualWorldGoalTransform.position);
            Debug.Log("Calculated distance gain: " + distanceGain);

            Vector3 adjustedVirtualWorldMovement = realWorldMovement * distanceGain;
            virtualWorldPlayerTransform.position += adjustedVirtualWorldMovement;
        }

        previousRealWorldPosition = realWorldPosition;
    }

    Vector3 CalculatePath(Vector3 currentPos, Vector3 goalPos)
    {
        return new Vector3(goalPos.x - currentPos.x, 0, goalPos.z - currentPos.z);
    }

    float CalculateAlignedRotationGain(Quaternion currentRotation, Vector3 realWorldPath, Vector3 virtualWorldPath)
    {
        float angleDifference = Vector3.Angle(realWorldPath, virtualWorldPath);
        return angleDifference * rotationGainFactor;
    }

    float CalculateDistanceGain(Vector3 realWorldPosition, Vector3 realWorldGoal, Vector3 virtualWorldPosition, Vector3 virtualWorldGoal)
    {
        float realWorldDistance = Vector2.Distance(new Vector2(realWorldPosition.x, realWorldPosition.z), new Vector2(realWorldGoal.x, realWorldGoal.z));
        float virtualWorldDistance = Vector2.Distance(new Vector2(virtualWorldPosition.x, virtualWorldPosition.z), new Vector2(virtualWorldGoal.x, virtualWorldGoal.z));
        return realWorldDistance / virtualWorldDistance;
    }

    bool HasReachedGoal(Vector3 realWorldPosition, Vector3 realWorldGoal, Vector3 virtualWorldPosition, Vector3 virtualWorldGoal)
    {
        return Vector2.Distance(new Vector2(realWorldPosition.x, realWorldPosition.z), new Vector2(realWorldGoal.x, realWorldGoal.z)) < distanceToTriggerGoal &&
               Vector2.Distance(new Vector2(virtualWorldPosition.x, virtualWorldPosition.z), new Vector2(virtualWorldGoal.x, virtualWorldGoal.z)) < distanceToTriggerGoal;
    }
}