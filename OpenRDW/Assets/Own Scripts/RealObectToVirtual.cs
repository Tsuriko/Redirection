using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealObectToVirtual : MonoBehaviour
{
    public Transform realAvatar;
    public Transform virtualAvatar;
    public GameObject objectToClone;

    private GameObject clone;
    private Vector3 initialOffset;
    private Quaternion initialRotation;

    void Start()
    {
        // Spawn the clone at the same position as the object to clone
        clone = Instantiate(objectToClone, objectToClone.transform.position, objectToClone.transform.rotation);
        initialOffset = realAvatar.position - objectToClone.transform.position;
    }

    void Update()
    {
        // Calculate the relative position of the real avatar's facing direction to the object
        Vector3 relativePosition = Quaternion.Inverse(realAvatar.rotation) * (objectToClone.transform.position - realAvatar.position);

        // Set the position of the clone based on the virtual avatar's position and the relative position
        clone.transform.position = virtualAvatar.position + (virtualAvatar.rotation * relativePosition);
    }
}