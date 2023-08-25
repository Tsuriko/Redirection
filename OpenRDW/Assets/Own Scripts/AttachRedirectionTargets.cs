using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR_Toolkit.Redirection;

public class AttachRedirectionTargets : MonoBehaviour
{
    public ConfigurationScript.AttachMethod attachMethod => ConfigurationScript.Instance.attachMethod;
    private Transform realHandOfOtherPlayer;
    private Transform virtualHandOfOtherPlayer;
    private Transform ownRealHand;
    private Transform realHandOfOtherPlayerVirtual;
    private Transform hostVirtualHand;
    private Transform hostRealHand;
    private GameObject midpointObject;
    private GameObject midpointObjectStreamed;
    public MidpointSynchronization midpointSync;

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
        midpointObject = GameObject.Find("midpointObject");
        midpointObjectStreamed = GameObject.Find("MidpointStreamed");
    }

    void HandleKeyPress()
    {
        FindObjects();
        Debug.Log("Redirection Targets Attached");
        realHandOfOtherPlayerVirtual = GameObject.Find("Real hand of Other Player").transform;
        GameObject vrPlayerGuest = ConfigurationScript.Instance.vrPlayerGuest;
        realHandOfOtherPlayer = vrPlayerGuest.transform.Find("Real/Right Hand");
        virtualHandOfOtherPlayer = vrPlayerGuest.transform.Find("Virtual/Right Hand");
        hostVirtualHand = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand");
        hostRealHand = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Right Hand");
        ownRealHand = ConfigurationScript.Instance.controllerRight.transform;
        midpointSync = ConfigurationScript.Instance.vrPlayerHost.GetComponent<MidpointSynchronization>();

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
        Vector3 realMidpoint = (realHandOfOtherPlayer.position + hostRealHand.position) * 0.5f;
        Vector3 virtualMidpoint = (hostVirtualHand.position + virtualHandOfOtherPlayer.position) * 0.5f;

        midpointObject.transform.position = realMidpoint;
        redirectionTarget.transform.position = midpointObjectStreamed.transform.position;
        redirectedTarget.transform.position = virtualMidpoint;

        initialRealMidpoint = realMidpoint;
        initialVirtualMidpoint = virtualMidpoint;
        isMidPointSet = true;
    }

    void ResetCombinedMidpoints()
    {
        midpointObject.transform.position = initialRealMidpoint;
        redirectionTarget.transform.position = midpointObjectStreamed.transform.position;
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
        midpointObject.transform.position = initialRealMidpoint;
        redirectionTarget.transform.position = midpointObjectStreamed.transform.position;
        redirectedTarget.transform.position = initialVirtualMidpoint;
        isMidPointSet = true;
    }
}