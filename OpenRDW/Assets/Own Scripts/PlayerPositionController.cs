using UnityEngine;
using Photon.Pun;

public class PlayerPositionController : MonoBehaviourPun
{
    public Vector3 targetPosition; // The target position
    public float horizontalDistanceBetweenPlayers; // Horizontal distance between players
    public float rotationOffset;
    public KeyCode triggerKey = KeyCode.G; // Replace with your desired button

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && PhotonNetwork.IsMasterClient)
        {
            // Calculate the new horizontal positions for both players
            Vector3 newPositionMaster = CalculateNewHorizontalPosition(true, targetPosition);
            Vector3 newPositionOther = CalculateNewHorizontalPosition(false, targetPosition);
            Quaternion newRotationMaster = Quaternion.Euler(0, rotationOffset, 0);
            Quaternion newRotationOther = Quaternion.Euler(0, -rotationOffset, 0);

            // Move the master client's "OwnPlayer" horizontally
            MoveOwnPlayerHorizontally(newPositionMaster, newRotationMaster);

            // Send an RPC to instruct the other client to move their "OwnPlayer" horizontally
            photonView.RPC("MoveOtherPlayerHorizontally", RpcTarget.Others, newPositionOther, newRotationOther);
        }
    }

    private Vector3 CalculateNewHorizontalPosition(bool isMaster, Vector3 targetPosition)
    {
        GameObject ownPlayer = GameObject.Find(isMaster ? "VR Player (Host)/Virtual/Head" : "VR Player (Guest)/Virtual/Head");

        if (ownPlayer != null)
        {
            Vector3 newPosition = targetPosition + ownPlayer.transform.forward * horizontalDistanceBetweenPlayers;
            newPosition.y = ownPlayer.transform.position.y; // Maintain the same vertical position
            return newPosition;
        }

        return Vector3.zero;
    }

    private void MoveOwnPlayerHorizontally(Vector3 newPosition, Quaternion newRotation)
    {
        GameObject ownPlayer = GameObject.Find("OwnPlayer");

        if (ownPlayer != null)
        {
            // Move the local "OwnPlayer" horizontally and apply rotation
            ownPlayer.transform.position = newPosition;
            ownPlayer.transform.rotation = newRotation;
        }
    }

    [PunRPC]
    private void MoveOtherPlayerHorizontally(Vector3 newPosition, Quaternion newRotation)
    {
        GameObject ownPlayer = GameObject.Find("OwnPlayer");

        if (ownPlayer != null)
        {
            // Move the other client's "OwnPlayer" horizontally and apply rotation
            ownPlayer.transform.position = newPosition;
            ownPlayer.transform.rotation = newRotation;
        }
    }
}