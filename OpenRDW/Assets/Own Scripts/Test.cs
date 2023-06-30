using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    public GameObject originalObject;
    public  GameObject copiedObject;
    private Rigidbody copiedRigidbody;

    private void Start()
    {
    }

    private void Update()
    {
        // Update the position and rotation of the copied object based on the original object
        copiedObject.transform.position = originalObject.transform.position;
        copiedObject.transform.rotation = originalObject.transform.rotation;

        // Move the copied object in the Unity editor
        if (Application.isEditor && !Application.isPlaying)
        {
            copiedObject.transform.position = transform.position;
        }
    }
}