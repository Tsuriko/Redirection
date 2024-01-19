using UnityEngine;
using Photon.Pun;

public class PlayerPositionController : MonoBehaviourPun
{
    public Vector3 targetPosition;
    public float distanceBetweenPlayers;
    public float rotationOffset;
    public KeyCode triggerKey = KeyCode.G; // Replace with your desired button

    void Update()
    {
        if (Input.GetKeyDown(triggerKey) && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("MoveAndRotatePlayers", RpcTarget.All);
        }
    }

    [PunRPC]
    private void MoveAndRotatePlayers()
    {
        GameObject masterPlayer = null;
        GameObject otherPlayer = null;

        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.TagObject is GameObject playerGameObject)
            {
                if (player.IsMasterClient)
                {
                    masterPlayer = playerGameObject;
                }
                else
                {
                    otherPlayer = playerGameObject;
                }
            }
        }

        if (masterPlayer == null || otherPlayer == null)
        {
            Debug.LogError("PlayerPositionController: One or both players not found.");
            return; // Exit if one of the players is not found
        }

        // Calculate the midpoint
        Vector3 midpoint = targetPosition;

        // Calculate positions
        Vector3 positionMaster = midpoint + (masterPlayer.transform.forward * distanceBetweenPlayers / 2.0f);
        Vector3 positionOther = midpoint - (otherPlayer.transform.forward * distanceBetweenPlayers / 2.0f);

        // Apply positions
        masterPlayer.transform.position = positionMaster;
        otherPlayer.transform.position = positionOther;

        // Calculate and apply rotations
        masterPlayer.transform.rotation = Quaternion.Euler(0, rotationOffset, 0);
        otherPlayer.transform.rotation = Quaternion.Euler(0, -rotationOffset, 0);
    }
}