using UnityEngine;
using Photon.Pun;

public class PlayerPositionController : MonoBehaviourPun
{
    public float targetDistanceBetweenPlayers; // Desired distance between players
    public float rotationOffset; // Desired rotation offset in degrees
    public KeyCode triggerKey = KeyCode.G; // Replace with your desired button
    public Vector3 midpoint; // Settable midpoint for player positioning

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

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Trigger key pressed by master client.");

            if (headMaster != null && headOther != null)
            {
                Debug.Log("Calculating new positions and rotations for players.");

                // Calculate the rotation separately
                Quaternion rotationMaster = CalculateRotation(true);
                Quaternion rotationOther = CalculateRotation(false);

                // Calculate the new positions for both heads
                Vector3 newPositionMaster = CalculateNewPosition(true);
                Vector3 newPositionOther = CalculateNewPosition(false);

                Debug.Log($"New position for master: {newPositionMaster}, New rotation for master: {rotationMaster}");
                Debug.Log($"New position for other player: {newPositionOther}, New rotation for other player: {rotationOther}");

                // Move the master client's "OwnPlayer" locally
                MoveOwnPlayerLocally(newPositionMaster, rotationMaster);

                // Send an RPC to instruct the other client to move their "OwnPlayer"
                photonView.RPC("MoveOtherPlayer", RpcTarget.Others, newPositionOther, rotationOther);
            }
            else
            {
                Debug.LogError("One or both head GameObjects are null.");
            }
        }
    }

    private Quaternion CalculateRotation(bool isMaster)
    {
        Vector3 directionToOther = isMaster ? (headOther.transform.position - headMaster.transform.position) : (headMaster.transform.position - headOther.transform.position);
        directionToOther.y = 0; // Ignore the y-axis to keep rotation only on the horizontal plane

        Quaternion targetRotation = Quaternion.LookRotation(directionToOther);

        // Apply the same offset to both players
        Quaternion offsetRotation = Quaternion.Euler(0f, rotationOffset, 0f);

        Quaternion finalRotation = targetRotation * offsetRotation;
        Debug.Log($"Calculated rotation for {(isMaster ? "master" : "other")} player: {finalRotation}");

        return finalRotation;
    }

    private Vector3 CalculateNewPosition(bool isMaster)
    {
        GameObject head = isMaster ? headMaster : headOther;
        GameObject ownPlayer = isMaster ? GameObject.Find("VR Player (Host)/Virtual") : GameObject.Find("VR Player (Guest)/Virtual");

        Vector3 directionToHead;
        if (headMaster.transform.position == headOther.transform.position)
        {
            // Use a default direction if both heads are at the same position
            directionToHead = isMaster ? Vector3.right : Vector3.left;
        }
        else
        {
            // Calculate the direction from the midpoint to the head
            directionToHead = (head.transform.position - midpoint).normalized;
        }

        // Calculate the new position for the head
        Vector3 newHeadPosition = midpoint + directionToHead * (targetDistanceBetweenPlayers / 2);

        // Calculate the offset from the OwnPlayer to the head
        Vector3 offsetToHead = head.transform.position - ownPlayer.transform.position;

        // Calculate and return the new position for the OwnPlayer, adjusting for the offset
        return newHeadPosition - offsetToHead;
    }

    private void MoveOwnPlayerLocally(Vector3 newPosition, Quaternion newRotation)
    {
        GameObject ownPlayer = GameObject.Find("OwnPlayer");

        if (ownPlayer != null)
        {
            Debug.Log("Moving own player locally.");
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
            Debug.Log("Moving other player.");
            otherPlayer.transform.position = newPosition;
            otherPlayer.transform.rotation = newRotation;
        }
        else
        {
            Debug.LogError("OtherPlayer GameObject not found.");
        }
    }
}