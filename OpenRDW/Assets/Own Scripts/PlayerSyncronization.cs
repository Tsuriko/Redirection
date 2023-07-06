using UnityEngine;
using Valve.VR;

public class PlayerSynchronization : MonoBehaviour
{
    public Transform targetLocation; // The target location where you want the player to be
    public Transform playerHead; // Reference to the player's head object (e.g., the camera)

    private bool movePlayer = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("Player Moved");
            playerHead = GameObject.Find("VR Player (Host)/Real/Head").transform;
            movePlayer = true;
        }

        if (movePlayer)
        {
            // Calculate the offset between the current position and the target position
            Vector3 offset = targetLocation.position - playerHead.position;

            // Apply the offset to the player's position
            Transform realParent = GameObject.Find("VR Player (Host)/Real").transform;
            realParent.transform.position += offset;

            // Set the player's rotation to match the target rotation
            Quaternion headRotationDifference = Quaternion.Inverse(playerHead.rotation) * targetLocation.rotation;
            realParent.transform.rotation *= headRotationDifference;

            movePlayer = false;
        }
    }
}