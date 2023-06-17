using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class RealPlayerTracking : MonoBehaviour
{
    public GameObject headObject;
    public GameObject leftHandObject;
    public GameObject rightHandObject;
    
    private PhotonView photonView;
    private SteamVR_Behaviour_Pose[] poses;

    private void Start()
    {
        photonView = GetComponentInParent<PhotonView>();

        poses = new SteamVR_Behaviour_Pose[2];
        poses[0] = leftHandObject.GetComponent<SteamVR_Behaviour_Pose>();
        poses[1] = rightHandObject.GetComponent<SteamVR_Behaviour_Pose>();
    }

    private void Update()
    {
        if (photonView.IsMine && SteamVR.instance != null && SteamVR.instance.hmd != null)
        {
            headObject.gameObject.SetActive(false);
            leftHandObject.gameObject.SetActive(false);
            rightHandObject.gameObject.SetActive(false);
            // Update head position and rotation
            Transform headTransform = SteamVR_Render.Top().head;
            headObject.transform.position = headTransform.position;
            headObject.transform.rotation = headTransform.rotation;
            // Update hand positions and rotations
            for (int i = 0; i < poses.Length; i++)
            {
                Transform handTransform = poses[i].transform;
                GameObject currentHandObject = i == 0 ? leftHandObject : rightHandObject;

                currentHandObject.transform.position = handTransform.position;
                currentHandObject.transform.rotation = handTransform.rotation;
            }
        }
        
    }
}