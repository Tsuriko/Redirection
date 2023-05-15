using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingScript : MonoBehaviour
{
    public Transform user; // the user's transform
    public Transform targetObject; // the object to check if the user is facing
    public float angleThreshold = 30f; // the maximum angle in degrees between the user's forward vector and the vector from the user to the target object

    private void Update()
    {
        // get the vector from the user to the target object
        Vector3 directionToTarget = targetObject.position - user.position;

        // calculate the angle between the user's forward vector and the vector to the target object
        float angle = Vector3.Angle(user.forward, directionToTarget);

        // check if the angle is below the threshold
        if (angle <= angleThreshold)
        {
            // the user is facing the target object
            Debug.Log("User is facing target object");
        }
    }
}
