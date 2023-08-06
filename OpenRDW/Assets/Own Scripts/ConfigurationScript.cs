using UnityEngine;

public class ConfigurationScript : MonoBehaviour
{
    // Singleton instance
    public static ConfigurationScript Instance;

    // Public Parameters
    public GameObject redirectedVirtualObject;
    public GameObject redirectedRealTarget;
    public GameObject otherPlayerHandObject;
    public GameObject vrPlayerGuest;
    public GameObject vrPlayerHost;
    public GameObject controllerRight;
    public GameObject cameraRig;

    // Delegates and Events for Key Press Actions
    public delegate void KeyAction();
    public event KeyAction OnCKeyPressed;
    public event KeyAction OnXKeyPressed;
    public event KeyAction OnYKeyPressed;
    public event KeyAction OnVKeyPressed;
    // Add more events for other keys as needed

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

    void Update()
    {
        HandleKeyPresses();
    }

    void AssignVariables()
    {
        // Assigning variables programmatically
        redirectedVirtualObject = GameObject.Find("Redirected Virtual Object");
        redirectedRealTarget = GameObject.Find("Redirected Real Target");
        otherPlayerHandObject = GameObject.Find("OtherPlayerHandObject");
        vrPlayerHost = GameObject.Find("VR Player (Host)");
        controllerRight = GameObject.Find("Controller (right)");
        cameraRig = GameObject.Find("[CameraRig]");

        // For VR Player (Guest) which might be instantiated later
        if (!vrPlayerGuest)
        {
            vrPlayerGuest = GameObject.Find("VR Player (Guest)");
        }
    }

    void HandleKeyPresses()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnCKeyPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnXKeyPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnYKeyPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            OnVKeyPressed?.Invoke();
        }
        // Handle more keys as needed
    }
}