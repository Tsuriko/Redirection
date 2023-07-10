using UnityEngine;
using Valve.VR;

public class VirtualPlayerSynchronization : MonoBehaviour
{
    private bool movePlayer = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("Player Moved");
            movePlayer = true;
        }

        if (movePlayer)
        {
            Transform realParent = GameObject.Find("VR Player (Host)/Real").transform;
            Transform cameraRig = GameObject.Find("[CameraRig]").transform;



            cameraRig.position = realParent.position - new Vector3(0f, 5.0f, 0f);
            cameraRig.rotation = realParent.rotation;

            movePlayer = false;
        }
    }
}
