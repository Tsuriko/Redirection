using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlaneScript : MonoBehaviour
{
    private Transform handTransform; // The hand's transform, set to "Controller (right)"
    private Transform referenceObject; // The object used to get the location
    private Transform movableObject; // The object that will be moved on the plane
    private Plane myPlane; // The plane

    void Start()
    {
        // Instantiate the plane once at the start using the reference object's position
        handTransform = GameObject.Find("Controller (right)").transform;
        referenceObject = GameObject.Find("Redirected Real Target").transform;
        movableObject = GameObject.Find("Redirected Virtual Object").transform;
        myPlane = new Plane(handTransform.forward, referenceObject.position);
    }

    void OnEnable()
    {
        // Move the plane to the reference object's position whenever the script is enabled
        MovePlaneToReferenceObjectLocation();
    }

    void Update()
    {
        // Create a ray from the hand's position in the direction of the hand's forward vector
        Ray ray = new Ray(handTransform.position, handTransform.forward);

        // Variable to store the distance along the ray where it intersects the plane
        float enter;

        // If the ray intersects the plane
        if (myPlane.Raycast(ray, out enter))
        {
            // Get the point of intersection
            Vector3 hitPoint = ray.GetPoint(enter);

            // Move the movable object to the nearest point on the plane
            movableObject.position = hitPoint;
        }
    }

    // Method to move the plane to the reference object's current location
    public void MovePlaneToReferenceObjectLocation()
    {
        // Update the plane's position to the reference object's position while keeping it perpendicular to the hand's direction
        myPlane.SetNormalAndPosition(handTransform.forward, referenceObject.position);
    }
}