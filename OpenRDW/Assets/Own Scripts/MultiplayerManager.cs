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

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon Master Server");
        JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room");

        // Instantiate VR player prefab for local player
        Vector3 spawnPosition = new Vector3(0f, 0f, 0f); // Define the spawn position
        vrPlayerPrefab = PhotonNetwork.Instantiate("Multiplayer Player", spawnPosition, Quaternion.identity);
        vrPlayerPrefab.name = "VR Player (Host)";
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
        Vector3 spawnPosition = new Vector3(0f, 0f, 0f); // Define the spawn position
        GameObject newPlayerPrefab = PhotonNetwork.Instantiate("Multiplayer Player", spawnPosition, Quaternion.identity);
        newPlayerPrefab.name = "VR Player (Guest)";
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
}