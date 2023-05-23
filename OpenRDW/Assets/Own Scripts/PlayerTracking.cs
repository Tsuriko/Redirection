using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;


public class PlayerTracking : MonoBehaviour
{
    public SteamVR_Action_Pose poseAction;
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            head.gameObject.SetActive(false);
            leftHand.gameObject.SetActive(false);
            rightHand.gameObject.SetActive(false);
            MapPosition(head, SteamVR_Input_Sources.Head);
            MapPosition(leftHand, SteamVR_Input_Sources.LeftHand);
            MapPosition(rightHand, SteamVR_Input_Sources.RightHand);
        
        }

        //Debug.Log("Right Hand Position: " + rightHand.localPosition);
    }
    void MapPosition(Transform target, SteamVR_Input_Sources source)
    {
        target.localPosition = poseAction.GetLocalPosition(source);
        target.localRotation = poseAction.GetLocalRotation(source);
    }
    
}
