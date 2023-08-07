using UnityEngine;

public class ConfigurationScript : MonoBehaviour
{
    // Singleton instance
    public static ConfigurationScript Instance;

    public enum AttachMethod
    {
        midpoint,
        otherHand
    }

    // Public Parameters
    public AttachMethod attachMethod;
    [HideInInspector] public GameObject redirectedVirtualObject;
    [HideInInspector] public GameObject redirectedRealTarget;
    [HideInInspector] public GameObject otherPlayerHandObject;
    [HideInInspector] public GameObject vrPlayerGuest;
    [HideInInspector] public GameObject vrPlayerHost;
    [HideInInspector] public GameObject controllerRight;
    [HideInInspector] public GameObject cameraRig;




    public delegate void KeyPressedAction();
    public event KeyPressedAction OnCKeyPressed;
    public event KeyPressedAction OnXKeyPressed;
    public event KeyPressedAction OnYKeyPressed;
    public event KeyPressedAction OnVKeyPressed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        AssignVariables();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnCKeyPressed?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            OnXKeyPressed?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            OnYKeyPressed?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            OnVKeyPressed?.Invoke();
        }
    }

    void AssignVariables()
    {
        // Assigning variables programmatically
        redirectedVirtualObject = GameObject.Find("Redirected Virtual Object");
        redirectedRealTarget = GameObject.Find("Redirected Real Target");
        otherPlayerHandObject = GameObject.Find("OtherPlayerHandObject");
        vrPlayerGuest = GameObject.Find("VR Player (Guest)");
        vrPlayerHost = GameObject.Find("VR Player (Host)");
        controllerRight = GameObject.Find("Controller (right)");
        cameraRig = GameObject.Find("[CameraRig]");
    }
}