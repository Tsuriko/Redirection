using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealObjectToVirtual : MonoBehaviour
{
    public Transform virtualAvatar;
    public List<GameObject> objectsToClone; // List of original objects to clone
    public Transform realAvatar;
    private List<GameObject> clones = new List<GameObject>(); // List of clones created from the objects
    public bool scriptEnabled = false;

    void Start()
    {
        //ConfigurationScript.Instance.OnXKeyPressed += HandleXKeyPress;
    }

    void OnDestroy()
    {
        //ConfigurationScript.Instance.OnXKeyPressed -= HandleXKeyPress;
    }

    void HandleXKeyPress()
    {
        if (!scriptEnabled && objectsToClone.Count > 0)
        {
            Debug.Log("Real Object now attached to virtual avatar");
            realAvatar = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head");

            foreach (var objectToClone in objectsToClone)
            {
                var clone = Instantiate(objectToClone, objectToClone.transform.position, objectToClone.transform.rotation);
                clone.name = objectToClone.name + "(virtual)"; // Change clone name here
                clones.Add(clone);
            }

            scriptEnabled = true;
        }
    }

    void Update()
    {
        if (scriptEnabled)
        {
            for (int i = 0; i < clones.Count; i++)
            {
                if (i < objectsToClone.Count) // Ensure there's a corresponding original object for each clone
                {
                    // Directly calculate the relative position for each clone based on its original object's current position
                    var relativePosition = Quaternion.Inverse(realAvatar.rotation) * (objectsToClone[i].transform.position - realAvatar.position);

                    // Set each clone's position based on the virtual avatar's position and the calculated relative position
                    clones[i].transform.position = virtualAvatar.position + (virtualAvatar.rotation * relativePosition);
                }
            }
        }
    }
    public void EnableScript()
    {
        if (!scriptEnabled && objectsToClone.Count > 0)
        {
            Debug.Log("Real Object now attached to virtual avatar");
            // Assuming ConfigurationScript.Instance.vrPlayerHost is already set
            realAvatar = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head");

            foreach (var objectToClone in objectsToClone)
            {
                var clone = Instantiate(objectToClone, objectToClone.transform.position, objectToClone.transform.rotation);
                clone.name = objectToClone.name + "(virtual)"; // Change clone name here
                clones.Add(clone);
            }

            scriptEnabled = true;
        }
    }
}