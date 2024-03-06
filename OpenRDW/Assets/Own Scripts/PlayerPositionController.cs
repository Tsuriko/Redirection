using UnityEngine;
using Photon.Pun;

public class PlayerPositionController : MonoBehaviourPun
{
    public float targetDistanceBetweenPlayers; // Desired distance between players
    public float rotationOffsetMaster;
    public float rotationOffsetOther;
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

    public void ActivatePlayerPositioning()
    {
        headMaster = GameObject.Find("VR Player (Host)/Virtual/Head");
        headOther = GameObject.Find("VR Player (Guest)/Virtual/Head");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Activating Player Positioning.");

            if (headMaster != null && headOther != null)
            {
                Quaternion rotationMaster = CalculateRotation(true);
                Quaternion rotationOther = CalculateRotation(false);

                Vector3 newPositionMaster = CalculateNewPosition(true);
                Vector3 newPositionOther = CalculateNewPosition(false);


                MoveOwnPlayerLocally(newPositionMaster, rotationMaster);
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

        // Use the respective offset for master or other player
        float rotationOffset = isMaster ? rotationOffsetMaster : rotationOffsetOther;
        Quaternion offsetRotation = Quaternion.Euler(0f, rotationOffset, 0f);

        Quaternion finalRotation = targetRotation * offsetRotation;

        return finalRotation;
    }

    private Vector3 CalculateNewPosition(bool isMaster)
    {
        GameObject head = isMaster ? headMaster : headOther;
        GameObject ownPlayer = isMaster ? GameObject.Find("VR Player (Host)/Virtual") : GameObject.Find("VR Player (Guest)/Virtual");
        GameObject masterPlayer = GameObject.Find("VR Player (Host)/Real/Head");
        GameObject otherPlayer = GameObject.Find("VR Player (Guest)/Real/Head");

        // Calculate horizontal distance between master and other player
        Vector3 masterPosition = new Vector3(masterPlayer.transform.position.x, 0, masterPlayer.transform.position.z);
        Vector3 otherPosition = new Vector3(otherPlayer.transform.position.x, 0, otherPlayer.transform.position.z);
        float horizontalDistanceBetweenPlayers = (masterPosition - otherPosition).magnitude;

        Vector3 directionToHead;
        if (headMaster.transform.position == headOther.transform.position)
        {
            // Use a default direction if both heads are at the same position
            directionToHead = isMaster ? Vector3.right : Vector3.left;
        }
        else
        {
            // Calculate the direction from the midpoint to the head, ignoring y-axis differences
            Vector3 headPositionWithoutY = new Vector3(head.transform.position.x, 0, head.transform.position.z);
            Vector3 midpointWithoutY = new Vector3(midpoint.x, 0, midpoint.z);
            directionToHead = (headPositionWithoutY - midpointWithoutY).normalized;
        }

        // Adjust the new head position calculation using the horizontal distance
        Vector3 newHeadPosition = midpoint + directionToHead * (horizontalDistanceBetweenPlayers / 2);

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
}