using UnityEngine;

public class SimpleRDW: MonoBehaviour
{
    public Transform realWorldPlayerTransform;
    public Transform virtualWorldCameraTransform;
    public Transform realWorldGoalTransform;
    public Transform virtualWorldGoalTransform;
    public float redirectionGain = 1.0f;
    public float distanceThreshold = 0.1f;

    private Vector3 previousRealWorldPosition;

    void Start()
    {
        previousRealWorldPosition = realWorldPlayerTransform.position;
    }

    void Update()
    {
        // Calculate the desired redirection vector from real player to virtual goal
        Vector3 desiredRedirection = virtualWorldGoalTransform.position - realWorldPlayerTransform.position;

        // Calculate the distance between real player and real goal
        float distanceToRealGoal = Vector3.Distance(realWorldPlayerTransform.position, realWorldGoalTransform.position);

        // If the player is close enough to the real goal, apply redirection
        if (distanceToRealGoal < distanceThreshold)
        {
            // Calculate how much to adjust the user's movement
            Vector3 adjustment = desiredRedirection * redirectionGain;

            // Apply the adjustment to the real-world player position
            realWorldPlayerTransform.position += adjustment;

            // Update the virtual camera position and orientation to match the real player's position and orientation
            virtualWorldCameraTransform.position = realWorldPlayerTransform.position;
            virtualWorldCameraTransform.rotation = realWorldPlayerTransform.rotation;
        }

        // Store the current real-world position for the next frame
        previousRealWorldPosition = realWorldPlayerTransform.position;
    }
}