using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealObectToVirtual : MonoBehaviour
{
    public Transform virtualAvatar;
    public GameObject objectToClone;
    public string cloneName;
    private Transform realAvatar;
    private GameObject clone;
    private Vector3 initialOffset;
    private Quaternion initialRotation;
    private bool scriptEnabled = false;

    void Start()
    {
        // Subscribe to the X key press event from ConfigurationScript
        ConfigurationScript.Instance.OnXKeyPressed += HandleXKeyPress;
    }

    void OnDestroy() // Unsubscribe when the object is destroyed
    {
        ConfigurationScript.Instance.OnXKeyPressed -= HandleXKeyPress;
    }

    void HandleXKeyPress()
    {
        Debug.Log("Real Object now attached to virtual avatar");
        // Spawn the clone at the same position as the object to clone
        clone = Instantiate(objectToClone, objectToClone.transform.position, objectToClone.transform.rotation);
        clone.name = cloneName;
        realAvatar = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head");
        initialOffset = realAvatar.position - objectToClone.transform.position;
        scriptEnabled = true;
    }

    void Update()
    {
        if (scriptEnabled)
        {
            // Calculate the relative position of the real avatar's facing direction to the object
            Vector3 relativePosition = Quaternion.Inverse(realAvatar.rotation) * (objectToClone.transform.position - realAvatar.position);

            // Set the position of the clone based on the virtual avatar's position and the relative position
            clone.transform.position = virtualAvatar.position + (virtualAvatar.rotation * relativePosition);
        }
    }
}