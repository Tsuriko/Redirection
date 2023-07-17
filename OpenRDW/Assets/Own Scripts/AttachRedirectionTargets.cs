using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR_Toolkit.Redirection;

public class AttachRedirectionTargets : MonoBehaviour
{ 
    public enum AttachMethod
    {
        midpoint,
        otherHand
    }

    public AttachMethod attachMethod;

    private Transform realHandOfOtherPlayer;
    private Transform virtualHandOfOtherPlayer;
    private Transform ownRealHand;
    private Transform realHandOfOtherPlayerVirtual;

    private bool attachObjects = false;
    private bool isMidPointSet = false;


    private GameObject otherPlayerHandObject;
    private GameObject redirectionTarget;
    private GameObject redirectedTarget;

    private Vector3 initialMidpoint;
    
    void Start()
    {
        FindObjects();
    }

    void Update()
    {
        HandleKeyPress();
        HandleObjectAttachments();
    }

    void FindObjects()
    {
        redirectedTarget = GameObject.Find("Redirected Virtual Object");
        redirectionTarget = GameObject.Find("Redirected Real Target");
        otherPlayerHandObject = GameObject.Find("OtherPlayerHandObject");
        
    }

    void HandleKeyPress()
    {
        if (!Input.GetKeyDown(KeyCode.C)) return;
        
        Debug.Log("Redirection Targets Attached");
        realHandOfOtherPlayerVirtual = GameObject.Find("Real hand of Other Player").transform;
        GameObject vrPlayerGuest = GameObject.Find("VR Player (Guest)");
        realHandOfOtherPlayer = vrPlayerGuest.transform.Find("Real/Right Hand"); ;
        virtualHandOfOtherPlayer = vrPlayerGuest.transform.Find("Virtual/Right Hand");
        ownRealHand = GameObject.Find("Controller (right)").transform;

        EnableVirtualToRealConnection();

        attachObjects = true;

        if (attachMethod != AttachMethod.midpoint) return;

        if (!isMidPointSet)
        {
            SetMidpoint();
        }
        else
        {
            ResetMidpoint();
        }
    }

    void EnableVirtualToRealConnection()
    {
        var virtualToRealConnection = redirectedTarget.transform
            .Find("VirtualToRealConnection").GetComponent<VirtualToRealConnection>();

        virtualToRealConnection.enabled = true;
        virtualToRealConnection.realPosition = redirectionTarget.transform;
    }

    void SetMidpoint()
    {
        Vector3 midpoint = (realHandOfOtherPlayerVirtual.position + ownRealHand.position) * 0.5f;
        redirectionTarget.transform.position = midpoint;

        initialMidpoint = midpoint;
        isMidPointSet = true;
    }

    void ResetMidpoint()
    {
        redirectionTarget.transform.position = initialMidpoint;
        isMidPointSet = false;
    }

    void HandleObjectAttachments()
    {
        if (!attachObjects) return;
        
        redirectedTarget.transform.position = virtualHandOfOtherPlayer.position;
        otherPlayerHandObject.transform.position = realHandOfOtherPlayer.position;

        if (attachMethod == AttachMethod.otherHand)
        {
            redirectionTarget.transform.position = realHandOfOtherPlayerVirtual.transform.position;
        }
    }
}