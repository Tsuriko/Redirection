using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR_Toolkit.Redirection;

public class AttachRedirectionTargets : MonoBehaviour
{
    private ConfigurationScript.AttachMethod attachMethod => ConfigurationScript.Instance.attachMethod;
    private Transform realHandOfOtherPlayer;
    private Transform virtualHandOfOtherPlayer;
    private Transform ownRealHand;
    private Transform realHandOfOtherPlayerVirtual;
    private Transform hostVirtualHand;  // Host's virtual hand
    private MidpointSynchronization midpointSync;

    private bool attachObjects = false;
    private bool isMidPointSet = false;

    private GameObject otherPlayerHandObject;
    public GameObject redirectionTarget;
    private GameObject redirectedTarget;

    private Vector3 initialRealMidpoint;
    private Vector3 initialVirtualMidpoint;

    void Start()
    {
        FindObjects();
        // Subscribe to the C key press event from ConfigurationScript
        ConfigurationScript.Instance.OnCKeyPressed += HandleKeyPress;
    }

    void OnDestroy()
    {
        ConfigurationScript.Instance.OnCKeyPressed -= HandleKeyPress;
    }

    void FindObjects()
    {
        redirectedTarget = ConfigurationScript.Instance.redirectedVirtualObject;
        redirectionTarget = ConfigurationScript.Instance.redirectedRealTarget;
        otherPlayerHandObject = ConfigurationScript.Instance.otherPlayerHandObject;
        midpointSync = GetComponent<MidpointSynchronization>();
    }

    void HandleKeyPress()
    {
        Debug.Log("Redirection Targets Attached");
        realHandOfOtherPlayerVirtual = GameObject.Find("Real hand of Other Player").transform;
        GameObject vrPlayerGuest = ConfigurationScript.Instance.vrPlayerGuest;
        realHandOfOtherPlayer = vrPlayerGuest.transform.Find("Real/Right Hand");
        virtualHandOfOtherPlayer = vrPlayerGuest.transform.Find("Virtual/Right Hand");
        hostVirtualHand = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand");
        ownRealHand = ConfigurationScript.Instance.controllerRight.transform;

        EnableVirtualToRealConnection();

        attachObjects = true;

        if (attachMethod == ConfigurationScript.AttachMethod.midpoint)
        {
            if (!isMidPointSet)
            {
                SetCombinedMidpoints();
                midpointSync.UpdateMidpoints(initialRealMidpoint, initialVirtualMidpoint);
            }
            else
            {
                ResetCombinedMidpoints();
            }
        }
    }

    void EnableVirtualToRealConnection()
    {
        var virtualToRealConnection = redirectedTarget.transform
            .Find("VirtualToRealConnection").GetComponent<VirtualToRealConnection>();

        virtualToRealConnection.enabled = true;
        virtualToRealConnection.realPosition = redirectionTarget.transform;
    }

    void SetCombinedMidpoints()
    {
        Vector3 realMidpoint = (realHandOfOtherPlayerVirtual.position + ownRealHand.position) * 0.5f;
        Vector3 virtualMidpoint = (hostVirtualHand.position + virtualHandOfOtherPlayer.position) * 0.5f;

        redirectionTarget.transform.position = realMidpoint;
        redirectedTarget.transform.position = virtualMidpoint;

        initialRealMidpoint = realMidpoint;
        initialVirtualMidpoint = virtualMidpoint;
        isMidPointSet = true;
    }

    void ResetCombinedMidpoints()
    {
        redirectionTarget.transform.position = initialRealMidpoint;
        redirectedTarget.transform.position = initialVirtualMidpoint;
        isMidPointSet = false;
    }

    void Update()
    {
        HandleObjectAttachments();
    }

    void HandleObjectAttachments()
    {
        if (!attachObjects) return;

        otherPlayerHandObject.transform.position = realHandOfOtherPlayer.position;

        if (attachMethod == ConfigurationScript.AttachMethod.otherHand)
        {
            redirectionTarget.transform.position = realHandOfOtherPlayerVirtual.transform.position;
            redirectedTarget.transform.position = virtualHandOfOtherPlayer.position;
        }
    }

    public void SetMidpoints(Vector3 real, Vector3 virtualPoint)
    {
        initialRealMidpoint = real;
        initialVirtualMidpoint = virtualPoint;
        redirectionTarget.transform.position = initialRealMidpoint;
        redirectedTarget.transform.position = initialVirtualMidpoint;
        isMidPointSet = true;
    }
}