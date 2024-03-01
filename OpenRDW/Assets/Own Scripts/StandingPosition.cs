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
    private List<GameObject> virtualClones = new List<GameObject>();

    public void CallSavePositionAndRotation()
    {
        photonView.RPC("SavePositionAndRotationRPC", RpcTarget.All);
    }

    [PunRPC]
    private void SavePositionAndRotationRPC()
    {
        savedPosition = new Vector3(realAvatar.position.x, realAvatar.position.y, realAvatar.position.z);
        savedYRotation = realAvatar.eulerAngles.y;
    }

    [PunRPC]
    private void SpawnVirtualCloneWithOffsetRPC(float usedOffset)
    {
        SpawnVirtualCloneWithOffset(usedOffset);
    }

    private void SpawnVirtualCloneWithOffset(float usedOffset)
    {
        Vector3 direction = (usedOffset > 0) ? Quaternion.Euler(0, savedYRotation, 0) * Vector3.right : Quaternion.Euler(0, savedYRotation, 0) * Vector3.left;
        Vector3 spawnPosition = savedPosition + direction * Mathf.Abs(usedOffset);
        
        Vector3 relativePosition = Quaternion.Inverse(realAvatar.rotation) * (spawnPosition - realAvatar.position);
        Vector3 virtualClonePosition = virtualAvatar.position + (virtualAvatar.rotation * relativePosition);
        virtualClonePosition.y = 0;

        GameObject virtualClone = Instantiate(objectToSpawn, virtualClonePosition, Quaternion.Euler(0, savedYRotation, 0));
        virtualClone.name = objectToSpawn.name + "(virtual)";
        virtualClones.Add(virtualClone);
    }

    public void CallSpawnVirtualCloneWithOffset()
    {
        photonView.RPC("SpawnVirtualCloneWithOffsetRPC", photonView.Owner, offset);
        photonView.RPC("SpawnVirtualCloneWithOffsetRPC", RpcTarget.Others, offsetOther);
    }

    public void CallDeleteAllSpawnedVirtualClones()
    {
        photonView.RPC("DeleteAllSpawnedVirtualClonesRPC", RpcTarget.All);
    }

    [PunRPC]
    private void DeleteAllSpawnedVirtualClonesRPC()
    {
        foreach (GameObject clone in virtualClones)
        {
            Destroy(clone);
        }
        virtualClones.Clear();
    }
}