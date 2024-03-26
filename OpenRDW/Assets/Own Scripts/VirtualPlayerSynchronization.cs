using UnityEngine;
using Valve.VR;

public class VirtualPlayerSynchronization : MonoBehaviour
{
    public KeyCode playerSyncActivationKey = KeyCode.V;

    private bool movePlayer = false;

    private void Update()
    {
        if (Input.GetKeyDown(playerSyncActivationKey))
        {
            Debug.Log("Player Moved");
            movePlayer = true;
        }

        if (movePlayer)
        {
            Transform realParent = GameObject.Find("VR Player (Host)/Real").transform;
            Transform ownPlayer = ConfigurationScript.Instance.ownPlayer.transform;



            ownPlayer.position = realParent.position - new Vector3(0f, 6.0f, 0f);
            ownPlayer.rotation = realParent.rotation;

            movePlayer = false;
        }
    }
}
