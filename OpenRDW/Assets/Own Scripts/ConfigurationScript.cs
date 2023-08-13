using UnityEngine;

public class ConfigurationScript : MonoBehaviour
{
    public enum AttachMethod
    {
        midpoint,
        otherHand
    }

    // Singleton instance
    public static ConfigurationScript Instance;

    // Public Parameters
    public AttachMethod attachMethod;
    public Camera mainCamera;
    public bool isVisualizationModeActive = false;
    [HideInInspector] public GameObject redirectedVirtualObject;
    [HideInInspector] public GameObject redirectedRealTarget;
    [HideInInspector] public GameObject otherPlayerHandObject;
    [HideInInspector] public GameObject vrPlayerGuest;
    [HideInInspector] public GameObject vrPlayerHost;
    [HideInInspector] public GameObject controllerRight;
    [HideInInspector] public GameObject cameraRig;
    public GameObject ownPlayer;

    public delegate void KeyPressedAction();
    public event KeyPressedAction OnCKeyPressed;
    public event KeyPressedAction OnXKeyPressed;
    public event KeyPressedAction OnYKeyPressed;
    public event KeyPressedAction OnVKeyPressed;
    public event KeyPressedAction OnWKeyPressed;
    public event KeyPressedAction OnAKeyPressed;
    public event KeyPressedAction OnSKeyPressed;
    public event KeyPressedAction OnDKeyPressed;
    public event KeyPressedAction OnLeftArrowPressed;
    public event KeyPressedAction OnRightArrowPressed;

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
    }

    private void OnValidate()
    {
        ToggleVisualizationMode();
    }

    private void Update()
    {
        AssignVariables();
        HandleKeyPresses();
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
        ownPlayer = GameObject.Find("OwnPlayer");
    }

    void HandleKeyPresses()
    {
        AssignVariables();
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
        if (Input.GetKeyDown(KeyCode.W)) OnWKeyPressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.A)) OnAKeyPressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.S)) OnSKeyPressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.D)) OnDKeyPressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.LeftArrow)) OnLeftArrowPressed?.Invoke();
        if (Input.GetKeyDown(KeyCode.RightArrow)) OnRightArrowPressed?.Invoke();
    }

    void ToggleVisualizationMode()
    {
        if (isVisualizationModeActive)
        {
            // Show objects in the "Visualization" layer
            mainCamera.cullingMask |= (1 << LayerMask.NameToLayer("Visualization"));
        }
        else
        {
            // Hide objects in the "Visualization" layer
            mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Visualization"));
        }
    }
}