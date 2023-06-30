using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealObectToVirtual : MonoBehaviour
{
    
    public Transform virtualAvatar;
    public GameObject objectToClone;
    private Transform realAvatar;
    private GameObject clone;
    private Vector3 initialOffset;
    private Quaternion initialRotation;
    private bool scriptEnabled = false;

    void Start()
    {

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Real Object now attached to virtual avatar");
                    // Spawn the clone at the same position as the object to clone
            clone = Instantiate(objectToClone, objectToClone.transform.position, objectToClone.transform.rotation);
            clone.name = "Real hand of Other Player";
            realAvatar = GameObject.Find("VR Player (Host)").transform.Find("Real").transform.Find("Head");
            initialOffset = realAvatar.position - objectToClone.transform.position;
            scriptEnabled = true;
        }
        if (scriptEnabled)
        {
            // Calculate the relative position of the real avatar's facing direction to the object
            Vector3 relativePosition = Quaternion.Inverse(realAvatar.rotation) * (objectToClone.transform.position - realAvatar.position);

            // Set the position of the clone based on the virtual avatar's position and the relative position
            clone.transform.position = virtualAvatar.position + (virtualAvatar.rotation * relativePosition);
        }

    }
}