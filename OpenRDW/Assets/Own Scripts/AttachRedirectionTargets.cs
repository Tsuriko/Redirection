using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachRedirectionTargets : MonoBehaviour
{
    public Transform midpoint;
    public Transform virtualHandOfOtherPlayer;
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
        redirectedTarget.transform.position = virtualHandofOtherPlayer.position;
        redirectionTarget.transform.position = midpoint.position;
    }
}
