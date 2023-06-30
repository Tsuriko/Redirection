using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HR_Toolkit.Redirection;

public class AttachRedirectionTargets : MonoBehaviour
{
    private Transform realHandOfOtherPlayer;
    private Transform virtualHandOfOtherPlayer;
    private bool attachObects = false;
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
            realHandOfOtherPlayer = vrPlayerGuest.transform.Find("Real/Right Hand");;
            virtualHandOfOtherPlayer = vrPlayerGuest.transform.Find("Virtual/Right Hand");
            redirectedTarget.transform.Find("VirtualToRealConnection").GetComponent<VirtualToRealConnection>().enabled = true;
            redirectedTarget.transform.Find("VirtualToRealConnection").GetComponent<VirtualToRealConnection>().realPosition = GameObject.Find("Real hand of Other Player").transform;
            attachObects = true;
        }
        if (attachObects)
        {
            redirectedTarget.transform.position = virtualHandOfOtherPlayer.position;
            redirectionTarget.transform.position = realHandOfOtherPlayer.position;
        }
    }
}
