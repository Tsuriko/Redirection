using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR_Toolkit.Redirection;

public class AttachRedirectionTargets : MonoBehaviour
{
    private ConfigurationScript.AttachMethod attachMethod => ConfigurationScript.Instance.attachMethod;
    public Transform realHandOfOtherPlayer;
    public Transform otherPlayerRealHead;
    public Transform otherPlayerVirtualHead;
    public Transform hostRealHead;
    public Transform hostVirtualHead;
    public Transform virtualHandOfOtherPlayer;
    private Transform realHandOfOtherPlayerVirtual;
    private GameObject midpointObject;
    private GameObject midpointObjectStreamed;
    private MidpointSynchronization midpointSync;

    private bool attachObjects = false;
    private bool isMidPointSet = false;

    private GameObject otherPlayerHandObject;
    private GameObject redirectionRealTarget;
    private GameObject redirectionVirtualTarget;

    private Vector3 initialRealMidpoint;
    private Vector3 initialVirtualMidpoint;

    void Start()
    {
        FindObjects();
        // Subscribe to the C key press event from ConfigurationScript
        //ConfigurationScript.Instance.OnCKeyPressed += HandleKeyPress;
    }

    void OnDestroy()
    {
        //ConfigurationScript.Instance.OnCKeyPressed -= HandleKeyPress;
    }

    void FindObjects()
    {
        redirectionVirtualTarget = ConfigurationScript.Instance.redirectedVirtualObject;
        redirectionRealTarget = ConfigurationScript.Instance.redirectedRealTarget;
        otherPlayerHandObject = ConfigurationScript.Instance.otherPlayerHandObject;
        midpointObject = ConfigurationScript.Instance.midpointObject;
        midpointObjectStreamed = ConfigurationScript.Instance.midpointObjectStreamed;

    }

    public void HandleKeyPress()
    {
        FindObjects();
        Debug.Log("Redirection Targets Attached");
        
    realHandOfOtherPlayerVirtual = GameObject.Find("OtherPlayerHandObject(virtual)").transform;
        GameObject vrPlayerGuest = ConfigurationScript.Instance.vrPlayerGuest;
        //TODO f�r MP das zur�cksetztn
        otherPlayerRealHead = vrPlayerGuest.transform.Find("Real/Head");
        otherPlayerVirtualHead = vrPlayerGuest.transform.Find("Virtual/Head");
        hostRealHead = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head");
        hostVirtualHead = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head");
        realHandOfOtherPlayer = vrPlayerGuest.transform.Find("Real/Right Hand");
        virtualHandOfOtherPlayer = vrPlayerGuest.transform.Find("Virtual/Right Hand");
        //realHandOfOtherPlayer = GameObject.Find("Fake Real hand").transform;
        //virtualHandOfOtherPlayer = GameObject.Find("Fake Virtual hand").transform; ;
        midpointSync = ConfigurationScript.Instance.vrPlayerHost.GetComponent<MidpointSynchronization>();

        EnableVirtualToRealConnection();

        attachObjects = true;

        if (attachMethod == ConfigurationScript.AttachMethod.midpoint)
        {
            //if (!isMidPointSet)
            //{
                SetCombinedMidpoints();
                midpointSync.UpdateMidpoints(initialRealMidpoint, initialVirtualMidpoint);
            //}
            //else
            //{
            //    ResetCombinedMidpoints();
            //}
        }
    }

    void EnableVirtualToRealConnection()
    {
        var virtualToRealConnection = redirectionVirtualTarget.transform
            .Find("VirtualToRealConnection").GetComponent<VirtualToRealConnection>();

        virtualToRealConnection.enabled = true;
        virtualToRealConnection.realPosition = redirectionRealTarget.transform;
    }

    void SetCombinedMidpoints()
    {
        Vector3 realMidpoint = (otherPlayerRealHead.position + hostRealHead.position) * 0.5f;
        Vector3 virtualMidpoint = (otherPlayerVirtualHead.position + hostVirtualHead.position) * 0.5f;

        midpointObject.transform.position = realMidpoint;
        redirectionRealTarget.transform.position = midpointObjectStreamed.transform.position;
        redirectionVirtualTarget.transform.position = virtualMidpoint;

        initialRealMidpoint = realMidpoint;
        initialVirtualMidpoint = virtualMidpoint;
        isMidPointSet = true;
    }

    void ResetCombinedMidpoints()
    {
        midpointObject.transform.position = initialRealMidpoint;
        redirectionRealTarget.transform.position = midpointObjectStreamed.transform.position;
        redirectionVirtualTarget.transform.position = initialVirtualMidpoint;
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
            redirectionRealTarget.transform.position = realHandOfOtherPlayerVirtual.transform.position;
            redirectionVirtualTarget.transform.position = virtualHandOfOtherPlayer.position;
        }
        if (attachMethod == ConfigurationScript.AttachMethod.midpoint)
        {
            redirectionRealTarget.transform.position = midpointObjectStreamed.transform.position;
        }
    }

    public void SetMidpoints(Vector3 real, Vector3 virtualPoint)
    {
        initialRealMidpoint = real;
        initialVirtualMidpoint = virtualPoint;
        midpointObject.transform.position = initialRealMidpoint;
        redirectionRealTarget.transform.position = midpointObjectStreamed.transform.position;
        redirectionVirtualTarget.transform.position = initialVirtualMidpoint;
        isMidPointSet = true;
    }
}