using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class PlayerTracking : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Vector3 additionalPositionOffset;

    private PhotonView photonView;
    private Transform trackedHead;
    private Transform trackedLeftHand;
    private Transform trackedRightHand;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        // Find the Camera, Controller (left), and Controller (right) objects in the scene
        trackedHead = GameObject.Find("Camera").transform;
        trackedLeftHand = GameObject.Find("Controller (left)").transform;
        trackedRightHand = GameObject.Find("VirtualHand").transform;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            head.gameObject.SetActive(false);
            leftHand.gameObject.SetActive(false);
            rightHand.gameObject.SetActive(false);
            MapPosition(head, trackedHead);
            MapPosition(leftHand, trackedLeftHand);
            MapPosition(rightHand, trackedRightHand);
        }

        //Debug.Log("Right Hand Position: " + rightHand.localPosition);
    }

    void MapPosition(Transform target, Transform source)
    {
        target.position = source.position + additionalPositionOffset;
        target.rotation = source.rotation;
    }
}
