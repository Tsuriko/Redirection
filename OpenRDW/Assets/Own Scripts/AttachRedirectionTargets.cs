using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachRedirectionTargets : MonoBehaviour
{
    public Transform midpoint;
    public Transform virtualHandofOtherPlayer;
    private GameObject redirectionTarget;
    private GameObject redirectedTarget;
    // Start is called before the first frame update
    void Start()
    {
        redirectedTarget = GameObject.Find("Redirected Target");
        redirectionTarget = GameObject.Find("Redirection Target");
    }

    // Update is called once per frame
    void Update()
    {
        redirectedTarget.transform.position = virtualHandofOtherPlayer.position;
        redirectionTarget.transform.position = midpoint.position;
    }
}
