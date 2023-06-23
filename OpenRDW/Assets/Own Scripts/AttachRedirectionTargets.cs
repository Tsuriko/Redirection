using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachRedirectionTargets : MonoBehaviour
{
    public string otherPlayerName = "foo";
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("X pressed");
            GameObject vrPlayerGuest = GameObject.Find(otherPlayerName);
            realHandOfOtherPlayer = vrPlayerGuest.transform.Find("Real/Right Hand");
            virtualHandOfOtherPlayer = vrPlayerGuest.transform.Find("Virtual/Right Hand");
            attachObects = true;
        }
        if (attachObects)
        {
            redirectedTarget.transform.position = virtualHandOfOtherPlayer.position;
            redirectionTarget.transform.position = realHandOfOtherPlayer.position;
        }
    }
}
