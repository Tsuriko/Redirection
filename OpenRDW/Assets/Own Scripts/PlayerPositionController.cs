using UnityEngine;
using Photon.Pun;

public class PlayerPositionController : MonoBehaviourPun
{
    public KeyCode triggerKey = KeyCode.G; // Replace with your desired button

    public GameObject headMaster;
    public GameObject headOther;

    void Start()
    {
        // Find the head GameObjects of both players during initialization
        headMaster = GameObject.Find("VR Player (Host)/Virtual/Head");
        headOther = GameObject.Find("VR Player (Guest)/Virtual/Head");

        if (headMaster == null || headOther == null)
        {
            Debug.LogError("Head GameObjects not found. Make sure they have the correct names.");
        }
        else
        {
            Debug.Log("Head GameObjects successfully found.");
        }
    }

    public void ActivatePlayerPositioning()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Activating Player Positioning.");

            if (headMaster != null && headOther != null)
            {
                // Calculate and apply rotation first
                Quaternion rotationMaster = CalculateRotation(true);
                Quaternion rotationOther = CalculateRotation(false);

                // Apply only the rotation for both players
                MoveOwnPlayerLocallyOnlyRotation(rotationMaster);
                photonView.RPC("MoveOwnPlayerLocallyOnlyRotation", RpcTarget.Others, rotationOther);

                // Recalculate position based on the new rotation
                Vector3 newPositionMaster = CalculateNewPosition(true);
                Vector3 newPositionOther = CalculateNewPosition(false);

                // Apply the recalculated position
                MoveOwnPlayerLocally(newPositionMaster, rotationMaster); // Reuses existing method to apply position
                photonView.RPC("MoveOwnPlayerLocally", RpcTarget.Others, newPositionOther, rotationOther);
            }
            else
            {
                Debug.LogError("One or both head GameObjects are null.");
            }
        }
    }

    private Quaternion CalculateRotation(bool isMaster)
    {
        // Assuming 'head' is the GameObject you're trying to align but can't modify directly,
        // and 'ownPlayer' is the parent whose rotation you can modify.
        GameObject head = isMaster ? headMaster : headOther;
        GameObject ownPlayer = isMaster ? GameObject.Find("VR Player (Host)/Virtual") : GameObject.Find("VR Player (Guest)/Virtual");

        // Desired direction facing along the X-axis, based on whether they're master or not.
        Vector3 desiredDirection = isMaster ? Vector3.left : Vector3.right;

        // Calculate the current forward direction of the head in world space
        Vector3 currentHeadForward = head.transform.forward;

        // Calculate the rotation needed to align the head's forward direction with the desired direction
        Quaternion fromCurrentToDesired = Quaternion.FromToRotation(currentHeadForward, desiredDirection);

        // Apply this rotation to the parent, considering the current rotation of the head relative to the parent
        Quaternion parentTargetRotation = fromCurrentToDesired * ownPlayer.transform.rotation;

        return parentTargetRotation;
    }

    private Vector3 CalculateNewPosition(bool isMaster)
    {
        GameObject head = isMaster ? headMaster : headOther;
        GameObject ownPlayer = isMaster ? GameObject.Find("VR Player (Host)/Virtual") : GameObject.Find("VR Player (Guest)/Virtual");

        // Calculate horizontal distance between the virtual representations of the master and other player
        GameObject masterPlayerVirtual = GameObject.Find("VR Player (Host)/Real/Head");
        GameObject otherPlayerVirtual = GameObject.Find("VR Player (Guest)/Real/Head");
        float horizontalDistanceBetweenPlayers = (masterPlayerVirtual.transform.position - otherPlayerVirtual.transform.position).magnitude;

        // Position players on opposite sides of the origin (0, 0, 0) based on the horizontal distance
        float halfDistance = horizontalDistanceBetweenPlayers / 2;
        Vector3 newHeadPosition = isMaster ? new Vector3(halfDistance, 0, 0) : new Vector3(-halfDistance, 0, 0);
        Debug.Log(newHeadPosition);

        // Calculate the offset from the OwnPlayer to the head
        Vector3 offsetToHead = head.transform.position - ownPlayer.transform.position;

        // Calculate and return the new position for the OwnPlayer, adjusting for the offset
        return newHeadPosition - offsetToHead;
    }

    [PunRPC]
    private void MoveOwnPlayerLocally(Vector3 newPosition, Quaternion newRotation)
    {
        GameObject ownPlayer = GameObject.Find("OwnPlayer");


        if (ownPlayer != null)
        {
            Debug.Log("Moving own player locally.");
            newPosition.y = 0;
            ownPlayer.transform.position = newPosition;
            ownPlayer.transform.rotation = newRotation;
        }
        else
        {
            Debug.LogError("OwnPlayer GameObject not found.");
        }
    }

    [PunRPC]
    private void MoveOtherPlayer(Vector3 newPosition, Quaternion newRotation)
    {
        GameObject otherPlayer = GameObject.Find("OwnPlayer"); // Replace with the name of the other client's "OwnPlayer" object

        if (otherPlayer != null)
        {
            newPosition.y = 0;
            otherPlayer.transform.position = newPosition;
            otherPlayer.transform.rotation = newRotation;
        }
        else
        {
            Debug.LogError("OtherPlayer GameObject not found.");
        }
    }
    [PunRPC]
    private void MoveOwnPlayerLocallyOnlyRotation(Quaternion newRotation)
    {
        GameObject ownPlayer = GameObject.Find("OwnPlayer");
        if (ownPlayer != null)
        {
            Debug.Log("Applying rotation to own player locally.");
            ownPlayer.transform.rotation = newRotation;
        }
        else
        {
            Debug.LogError("OwnPlayer GameObject not found.");
        }
    }
}