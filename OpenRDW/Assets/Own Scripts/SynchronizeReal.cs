using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeReal : MonoBehaviour
{
    private bool moveToZero = false;
    private Quaternion targetRotation = Quaternion.identity;
    private Transform realObject;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            // Set the target rotation
            targetRotation = Quaternion.Euler(0f, 0f, 0f);
            // Start moving the object to (0, 0, 0)
            moveToZero = true;
        }

        if (moveToZero)
        {
            GameObject vrPlayerHost = GameObject.Find("VR Player (Host)");
            realObject = vrPlayerHost.transform.Find("Real");
            // Move the object towards (0, 0, 0)
            realObject.position = Vector3.MoveTowards(realObject.position, Vector3.zero, Time.deltaTime * 5f);
            // Rotate the object towards the target rotation
            realObject.rotation = Quaternion.RotateTowards(realObject.rotation, targetRotation, Time.deltaTime * 50f);

            // Check if the object has reached the target position and rotation
            if (realObject.position == Vector3.zero && realObject.rotation == targetRotation)
            {
                // Stop moving and rotating the object
                moveToZero = false;
            }
        }
    }
}
