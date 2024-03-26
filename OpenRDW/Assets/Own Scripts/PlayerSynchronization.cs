using UnityEngine;
using Valve.VR;

public class PlayerSynchronization : MonoBehaviour
{
    public Transform targetLocation; // The target location where you want the player to be

    private Transform playerHead; // Reference to the player's head object (e.g., the camera)
    private bool movePlayer = false;
    private Transform realParent;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //MovePlayer();
        }

        if (movePlayer)
        {
            realParent = GameObject.Find("VR Player (Host)/Real").transform;

            // Calculate the y-axis rotation difference between the player's head and the target
            float yRotationDiff = targetLocation.eulerAngles.y - playerHead.eulerAngles.y;

            // Apply the y-axis rotation difference to the real parent
            realParent.transform.Rotate(0, yRotationDiff, 0, Space.World);

            // Calculate the offset between the current position and the target position
            Vector3 offset = targetLocation.position - playerHead.position;

            // Apply the offset to the player's position
            realParent.transform.position += offset;
            moveVirtualPlayerToReal();
            movePlayer = false;
        }
    }
        public void MovePlayer()
    {
        Debug.Log("Player Moved");
        playerHead = GameObject.Find("VR Player (Host)/Real/Head").transform;
        movePlayer = true;
    }
    public void moveVirtualPlayerToReal()
    {
        realParent = GameObject.Find("VR Player (Host)/Real").transform;
        Transform ownPlayer  = ConfigurationScript.Instance.ownPlayer.transform;



        ownPlayer.position = realParent.position - new Vector3(0f, 6.0f, 0f);
        ownPlayer.rotation = realParent.rotation;

        movePlayer = false;
    }
}