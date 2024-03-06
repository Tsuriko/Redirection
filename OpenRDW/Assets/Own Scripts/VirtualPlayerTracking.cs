using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class VirtualPlayerTracking : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;
    public Transform parentOfHead;

    private PhotonView photonView;
    private Transform trackedHead;
    private Transform trackedLeftHand;
    private Transform trackedRightHand;
    private Transform trackedCameraRig;

    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();

        // Find the Camera, Controller (left), and Controller (right) objects in the scene
        trackedHead = GameObject.Find("Camera").transform;
        trackedLeftHand = GameObject.Find("Controller (left)").transform;
        trackedRightHand = GameObject.Find("Controller (right)").transform;
        trackedCameraRig = GameObject.Find("OwnPlayer").transform;
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
            MapPosition(parentOfHead, trackedCameraRig);
        }

        //Debug.Log("Right Hand Position: " + rightHand.localPosition);
    }

    void MapPosition(Transform target, Transform source)
    {
        target.transform.position = source.transform.position;
        target.transform.rotation = source.transform.rotation;
    }
}
