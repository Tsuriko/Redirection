using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToObject : MonoBehaviour
{
    public Transform userRealSpace;
    public Transform userVrSpace;
    public Transform objectRealSpace;
    public Transform objectVrSpace;

    public float maxRotationSpeed = 180f;

    private bool isSynchronizing = false;
    private float targetAngle;
    private float currentAngle;

    void Update()
    {
        // Calculate the angle between the user and the object in both spaces
        Vector3 userToObjectReal = objectRealSpace.position - userRealSpace.position;
        Vector3 userToObjectVr = objectVrSpace.position - userVrSpace.position;
        float targetAngle = Vector3.SignedAngle(userToObjectVr, userToObjectReal, Vector3.up);

        if (!isSynchronizing && Mathf.Abs(targetAngle - currentAngle) > 0.1f)
        {
            // Start synchronizing if there's a significant difference in angle
            isSynchronizing = true;
            this.targetAngle = targetAngle;
        }

        if (isSynchronizing)
        {
            // Synchronize the angle gradually
            float angleDifference = targetAngle - currentAngle;
            float rotationSpeed = Mathf.Clamp(angleDifference / Time.deltaTime, -maxRotationSpeed, maxRotationSpeed);
            float rotationAngle = rotationSpeed * Time.deltaTime;

            userVrSpace.Rotate(Vector3.up, rotationAngle);
            currentAngle += rotationAngle;

            if (Mathf.Abs(currentAngle - targetAngle) < 0.1f)
            {
                // Finish synchronizing when the angle is close enough
                isSynchronizing = false;
            }
        }
    }
}