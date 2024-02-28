using UnityEngine;
using System.Collections.Generic;

public class StandingPosition : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform virtualAvatar; // Virtual avatar to mirror the object on
    public Transform realAvatar; // Real avatar's position to mirror from

    public float offset = 0f; // Offset to spawn the virtual clone
    public Vector3 savedPosition;
    private float savedYRotation;
    private List<GameObject> virtualClones = new List<GameObject>(); // List to keep track of virtual clones


    // Method to save the horizontal position and Y rotation
    public void SavePositionAndRotation()
    {
        savedPosition = new Vector3(realAvatar.transform.position.x, realAvatar.transform.position.y, realAvatar.transform.position.z);
        savedYRotation = realAvatar.transform.eulerAngles.y;
    }

    // Method to spawn a virtual clone with an offset, without creating a real-world object
    public void SpawnVirtualCloneWithOffset()
    {
        Vector3 direction = (offset > 0) ? Quaternion.Euler(0, savedYRotation, 0) * Vector3.right : Quaternion.Euler(0, savedYRotation, 0) * Vector3.left;
        Vector3 spawnPosition = savedPosition + direction * Mathf.Abs(offset);
        
        // Calculate the virtual clone's position relative to the virtual avatar, mirroring the intended position
        Vector3 relativePosition = Quaternion.Inverse(realAvatar.rotation) * (spawnPosition - realAvatar.position);
        Vector3 virtualClonePosition = virtualAvatar.position + (virtualAvatar.rotation * relativePosition);
        virtualClonePosition.y = 0;

        GameObject virtualClone = Instantiate(objectToSpawn, virtualClonePosition, Quaternion.Euler(0, savedYRotation, 0));
        virtualClone.name = objectToSpawn.name + "(virtual)"; // Naming the clone to indicate it's virtual
        virtualClones.Add(virtualClone); // Add the clone to the list
    }

    // Method to delete the most recently spawned virtual clone
    public void DeleteLastSpawnedVirtualClone()
    {
        if (virtualClones.Count > 0)
        {
            GameObject cloneToDelete = virtualClones[virtualClones.Count - 1]; // Get the last virtual clone
            
            virtualClones.RemoveAt(virtualClones.Count - 1); // Remove the clone from its list
            
            Destroy(cloneToDelete); // Delete the virtual clone
        }
    }

    // Method to delete all spawned virtual clones
    public void DeleteAllSpawnedVirtualClones()
    {
        foreach (GameObject clone in virtualClones)
        {
            Destroy(clone); // Delete each virtual clone
        }
        virtualClones.Clear(); // Clear the list of virtual clones
    }
}