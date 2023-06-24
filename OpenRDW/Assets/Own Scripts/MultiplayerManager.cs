using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{

    private GameObject vrPlayerPrefab; // Reference to the VR player prefab

    private void Start()
    {
        ConnectToPhoton();
    }

    private void ConnectToPhoton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void Update()
    {
        // Check for VR player prefabs and rename them
        GameObject[] vrPlayerPrefabs = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject vrPlayerPrefab in vrPlayerPrefabs)
        {
            if (vrPlayerPrefab.name == "Multiplayer Player(Clone)")
            {
                vrPlayerPrefab.name = "VR Player (Guest)";
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("Connected to Photon Master Server");
        JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");

        // Instantiate VR player prefab for local player
        vrPlayerPrefab = PhotonNetwork.Instantiate("Multiplayer Player", Vector3.zero, Quaternion.identity);
        vrPlayerPrefab.name = "VR Player (Host)"; // Set the name of the local player's VR player prefab
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room. Creating a new room...");

        // Create a new room if no rooms are available
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2; // Adjust the maximum number of players as desired
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("A new player entered the room");

        // Instantiate VR player prefab for the newly joined player
        base.OnPlayerEnteredRoom(newPlayer);
        //ameObject newPlayerPrefab = PhotonNetwork.Instantiate("Multiplayer Player", Vector3.zero, Quaternion.identity);
        //newPlayerPrefab.name = newPlayer.NickName; // Set the name of the newly joined player's VR player prefab
    }

    private void JoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogWarning("Cannot join a random room. Not connected to Photon.");
        }
    }

    public void LeaveRoom()
    {
        Debug.Log("Leave Room");
        PhotonNetwork.LeaveRoom();
    }

    //onleftroom
    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(vrPlayerPrefab);
        Debug.Log("OnLeftRoom");
        base.OnLeftRoom();
    }
}
