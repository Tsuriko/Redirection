using UnityEngine;
using Photon.Pun;
public class MidpointSynchronization : MonoBehaviourPunCallbacks
{
    private Vector3 realMidpoint;
    private Vector3 virtualMidpoint;

    private AttachRedirectionTargets attachRedirectionTargets;

    private void Start()
    {
        attachRedirectionTargets = GetComponent<AttachRedirectionTargets>();
    }

    public void UpdateMidpoints(Vector3 real, Vector3 virtualPoint)
    {
        realMidpoint = real;
        virtualMidpoint = virtualPoint;

        // Apply the midpoints locally
        ApplyMidpoints();

        // Send the updated midpoints to the other player
        photonView.RPC("UpdateMidpointsRPC", RpcTarget.Others, realMidpoint, virtualMidpoint);
    }

    [PunRPC]
    private void UpdateMidpointsRPC(Vector3 real, Vector3 virtualPoint)
    {
        realMidpoint = real;
        virtualMidpoint = virtualPoint;

        // Apply the midpoints for the other player
        ApplyMidpoints();
    }

    private void ApplyMidpoints()
    {
        // Update the midpoints in the AttachRedirectionTargets script
        attachRedirectionTargets.SetMidpoints(realMidpoint, virtualMidpoint);
    }
}