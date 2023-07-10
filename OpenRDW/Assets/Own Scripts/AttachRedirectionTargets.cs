using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR_Toolkit.Redirection;

public class AttachRedirectionTargets : MonoBehaviour
{ 

    public enum AttachMethod
    {
        midpoint, otherHand
    }

    public AttachMethod attachMethod;

    private Transform realHandOfOtherPlayer;
    private Transform virtualHandOfOtherPlayer;
    private Transform ownRealHand;
    private bool attachObjects = false;
    private GameObject redirectionTarget;
    private GameObject redirectedTarget;
    // Start is called before the first frame update
    void Start()
    {
        redirectedTarget = GameObject.Find("Redirected Virtual Object");
        redirectionTarget = GameObject.Find("Redirected Real Target");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Redirection Targets Attached");
            GameObject vrPlayerGuest = GameObject.Find("VR Player (Guest)");
            realHandOfOtherPlayer = vrPlayerGuest.transform.Find("Real/Right Hand"); ;
            virtualHandOfOtherPlayer = vrPlayerGuest.transform.Find("Virtual/Right Hand");
            ownRealHand = GameObject.Find("VR Player (Host)/Real/Right Hand").transform;
            redirectedTarget.transform.Find("VirtualToRealConnection").GetComponent<VirtualToRealConnection>().enabled = true;
            redirectedTarget.transform.Find("VirtualToRealConnection").GetComponent<VirtualToRealConnection>().realPosition = GameObject.Find("Real hand of Other Player").transform;
            attachObjects = true;
            if(attachMethod == AttachMethod.midpoint)
            {
                Vector3 midpoint = (realHandOfOtherPlayer.position + ownRealHand.position) * 0.5f;

                // Set the position of the objectToMove to the calculated midpoint position
                redirectionTarget.transform.position = midpoint;
            }
        }
        if (attachObjects)
        {
            redirectedTarget.transform.position = virtualHandOfOtherPlayer.position;
            if(attachMethod == AttachMethod.otherHand)
            {
                redirectionTarget.transform.position = realHandOfOtherPlayer.position;
            }
        }
    }

}
