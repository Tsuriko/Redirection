using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceMeasure : MonoBehaviour
{
    public Transform user1VR; // the first user's transform
    public Transform user1RS; // the first user's transform
    public Transform user2VR; // the second user's transform
    public Transform user2RS; // the second user's transform

    private void Update()
    {
        // calculate the distance between the two users
        float distanceVR = Vector3.Distance(user1VR.position, user2VR.position);
        float distanceRS = Vector3.Distance(user1RS.position, user2RS.position);

        // output the distance to the console
        Debug.Log("Distance between users: in VR" + distanceVR.ToString("F2"));
        Debug.Log("Distance between users: REAL SPACE" + distanceRS.ToString("F2"));
    }
}
