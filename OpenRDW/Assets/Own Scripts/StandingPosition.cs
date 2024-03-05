using UnityEngine;
using Photon.Pun; 
using System.Collections.Generic;


public class StandingPosition : MonoBehaviourPunCallbacks
{
    public GameObject objectToSpawn;
    public Transform virtualAvatar;
    public Transform realAvatar;

    public float offset = 0f;
    public float offsetOther = 0f;
    public Vector3 savedPosition;
    private float savedYRotation;
    private GameObject virtualClone; // Changed from List to a single GameObject


    public void CallSavePositionAndRotation()
    {
        photonView.RPC("SavePositionAndRotationRPC", RpcTarget.All);
    }

    [PunRPC]
    public void SavePositionAndRotationRPC()
    {
        savedPosition = new Vector3(realAvatar.position.x, realAvatar.position.y, realAvatar.position.z);
        savedYRotation = realAvatar.eulerAngles.y;
    }

    [PunRPC]
    private void SpawnVirtualCloneWithOffsetRPC(float usedOffset)
    {
        Debug.Log("SpawnVirtualCloneWithOffsetRPC(float usedOffset)");
        SpawnVirtualCloneWithOffset(usedOffset);
    }

    public void SpawnVirtualCloneWithOffset(float usedOffset)
    {
        if (virtualClone != null) // Check if a clone already exists
        {
            Destroy(virtualClone); // Destroy the existing clone
        }

        Vector3 direction = (usedOffset > 0) ? Quaternion.Euler(0, savedYRotation, 0) * Vector3.right : Quaternion.Euler(0, savedYRotation, 0) * Vector3.left;
        Vector3 spawnPosition = savedPosition + direction * Mathf.Abs(usedOffset);

        Vector3 relativePosition = Quaternion.Inverse(realAvatar.rotation) * (spawnPosition - realAvatar.position);
        Vector3 virtualClonePosition = virtualAvatar.position + (virtualAvatar.rotation * relativePosition);
        virtualClonePosition.y = 0;

        virtualClone = Instantiate(objectToSpawn, virtualClonePosition, Quaternion.Euler(0, savedYRotation, 0)); // Instantiate and assign to virtualClone
        virtualClone.name = objectToSpawn.name + "(virtual)";
        Debug.Log("Spawn");
    }

    public void CallSpawnVirtualCloneWithOffset()
    {
        Debug.Log("CallSpawnVirtualCloneWithOffset()");
        photonView.RPC("SpawnVirtualCloneWithOffsetRPC", RpcTarget.MasterClient, offset);
        photonView.RPC("SpawnVirtualCloneWithOffsetRPC", RpcTarget.Others, offsetOther);
    }

    public void CallDeleteAllSpawnedVirtualClones()
    {
        photonView.RPC("DeleteAllSpawnedVirtualClonesRPC", RpcTarget.All);
    }

    [PunRPC]
    private void DeleteAllSpawnedVirtualClonesRPC()
    {
        if (virtualClone != null) // Check if the clone exists
        {
            Destroy(virtualClone); // Destroy the clone
            virtualClone = null; // Ensure the reference is cleared
        }
    }
}