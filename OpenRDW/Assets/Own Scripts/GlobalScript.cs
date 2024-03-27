using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // Include the Photon PUN namespace
using HR_Toolkit;

public class GlobalScript : MonoBehaviour
{
    [Header("Key Presses Control")]
    [Tooltip("Enable or disable all key presses.")]
    public bool enableKeyPresses = true; //
    [Header("Activation Keys")]
    [Tooltip("Key to activate Player Synchronization.")]
    public KeyCode playerSyncActivationKey = KeyCode.Y;
    [Tooltip("Key to activate Redirection Control.")]
    public KeyCode redirectionControlActivationKey = KeyCode.R;
    [Tooltip("Key to activate Attach Redirection Targets.")]
    public KeyCode attachRedirectionTargetsActivationKey = KeyCode.C;
    [Tooltip("Key to activate Player Position Controller.")]
    public KeyCode playerPositionControllerActivationKey = KeyCode.G;
    [Tooltip("Key to activate Standing Position functionality.")]
    public KeyCode standingPositionActivationKey = KeyCode.S; // Add a new KeyCode for Standing Position activation

    [Header("Player Synchronization Settings")]
    [Tooltip("Target location for player synchronization.")]
    public Transform playerSyncTargetLocation;

    [Header("Real Object To Virtual Settings")]
    [Tooltip("Virtual avatar for real to virtual object mapping.")]
    public Transform virtualAvatar;
    [Tooltip("List of objects to clone into the virtual environment.")]
    public List<GameObject> objectsToClone;

    [Header("Redirection Settings")]
    [Tooltip("Target transform for redirection.")]
    public Transform redirectionRealTarget;
    [Range(0f, 1f)]
    [Tooltip("Slider value to adjust redirection intensity.")]
    public float redirectionSliderValue = 0.5f;

    [Tooltip("RDW Redirection intensity value.")]
    [Range(0f, 2f)]
    public float redirectIntensity = 1f;
    [Tooltip("Hand Redirection Live redirection Algorithm")]
    public bool liveRedirection = false;


    [Header("StandingPosition Seettings")]
    [Tooltip("Standing Goal Object")]
    public GameObject standingGoalObject;
    [Tooltip("Standing Position Offset")]
    public float standingPositionOffset = 0;
    [Tooltip("Standing Position Offset Other Player")]
    public float standingPositionOffsetOther = 0;

    private Transform realAvatar;

    // References to the script components
    private PlayerSynchronization playerSyncScriptComponent;
    private RealObjectToVirtual realObjectToVirtualScriptComponent;
    private RedirectionControl redirectionControlScriptComponent;
    private AttachRedirectionTargets attachRedirectionTargetsScriptComponent;
    private PlayerPositionController playerPositionControllerScriptComponent;
    private StandingPosition standingPositionScriptComponent;

    private bool hasActivatedScriptsAfterDelay = false;

    void Start()
    {
        // Dynamic component discovery
        playerSyncScriptComponent = FindObjectOfType<PlayerSynchronization>();
        realObjectToVirtualScriptComponent = FindObjectOfType<RealObjectToVirtual>();
        redirectionControlScriptComponent = FindObjectOfType<RedirectionControl>();
        attachRedirectionTargetsScriptComponent = FindObjectOfType<AttachRedirectionTargets>();
        playerPositionControllerScriptComponent = FindObjectOfType<PlayerPositionController>();
        standingPositionScriptComponent = FindObjectOfType<StandingPosition>();

        // Initial component state control
        if (playerSyncScriptComponent != null) playerSyncScriptComponent.enabled = false;
        if (realObjectToVirtualScriptComponent != null) realObjectToVirtualScriptComponent.enabled = false;
        if (redirectionControlScriptComponent != null) redirectionControlScriptComponent.enabled = false;
        if (attachRedirectionTargetsScriptComponent != null) attachRedirectionTargetsScriptComponent.enabled = false;
        if (playerPositionControllerScriptComponent != null) playerPositionControllerScriptComponent.enabled = false;
        if (standingPositionScriptComponent != null) standingPositionScriptComponent.enabled = false;
    }

    void Update()
    {
        // Event-driven component activation
        if (!enableKeyPresses) return;
        if (Input.GetKeyDown(playerSyncActivationKey))
        {
            syncPlayers();
        }

        if (Input.GetKeyDown(redirectionControlActivationKey) && PhotonNetwork.IsMasterClient)
        {
            ActivateRedirectionLogic();
        }

        if (Input.GetKeyDown(attachRedirectionTargetsActivationKey))
        {
            activateAttachRedirectionTargetsScript();
        }

        if (Input.GetKeyDown(playerPositionControllerActivationKey) && PhotonNetwork.IsMasterClient)
        {
            if (playerPositionControllerScriptComponent != null)
            {
                ActivatePlayerPositionController();
            }
        }
        if (Input.GetKeyDown(standingPositionActivationKey))
        {
            ActivateStandingPosition();
        }
    }

    public void activateAttachRedirectionTargetsScript()
    {
        if (attachRedirectionTargetsScriptComponent != null)
        {
            attachRedirectionTargetsScriptComponent.enabled = true;
            attachRedirectionTargetsScriptComponent.HandleKeyPress();
        }
    }

    public void syncPlayers()
    {
        ActivatePlayerSynchronization();
        if (!hasActivatedScriptsAfterDelay) {
            StartCoroutine(ActivateScriptsAfterDelay(0.1f));
            hasActivatedScriptsAfterDelay = true;
        }
        
    }

    private void ActivatePlayerSynchronization()
    {
        if (playerSyncScriptComponent != null)
        {
            playerSyncScriptComponent.enabled = true;
            ConfigurePlayerSynchronization();
            playerSyncScriptComponent.MovePlayer();
        }
    }

    private IEnumerator ActivateScriptsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (realObjectToVirtualScriptComponent != null)
        {
            realObjectToVirtualScriptComponent.enabled = true;
            ConfigureRealObjectToVirtual();
            realObjectToVirtualScriptComponent.EnableScript();
        }

        if (redirectionControlScriptComponent != null)
        {
            ConfigureRedirectionControl();
            redirectionControlScriptComponent.enabled = true;
        }
        realAvatar = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head");
        ActivateStandingPosition();
        ConfigurePlayerPositionController();

    }

    private void ConfigurePlayerSynchronization()
    {
        if (playerSyncScriptComponent != null && playerSyncTargetLocation != null)
        {
            playerSyncScriptComponent.targetLocation = playerSyncTargetLocation;
        }
    }

    private void ConfigureRealObjectToVirtual()
    {
        if (realObjectToVirtualScriptComponent != null)
        {
            realObjectToVirtualScriptComponent.virtualAvatar = virtualAvatar;
            realObjectToVirtualScriptComponent.objectsToClone = objectsToClone;
        }
    }

    public void ConfigureRedirectionControl()
    {
        if (redirectionControlScriptComponent != null)
        {
            redirectionControlScriptComponent.user = virtualAvatar;
            if (!redirectionRealTarget)
            {
                redirectionRealTarget = GameObject.Find("Redirected Real Target").transform;
            }
            redirectionControlScriptComponent.realTarget = redirectionRealTarget;
            redirectionControlScriptComponent.sliderValue = redirectionSliderValue;
            redirectionControlScriptComponent.redirectIntensity = redirectIntensity;
            redirectionControlScriptComponent.liveRedirection = liveRedirection;
        }
    }

    public void ActivateRedirectionLogic()
    {
        if (redirectionControlScriptComponent != null && redirectionControlScriptComponent.enabled && PhotonNetwork.IsMasterClient)
        {
            ConfigureRedirectionControl();
            redirectionControlScriptComponent.StartRedirectionExternally();
        }
    }
    public void EndHandRedirection()
    {
        if (redirectionControlScriptComponent != null)
        {
            redirectionControlScriptComponent.EndHandRedirection();
        }
    }
    public void EndRedirectedWalking()
    {
        if (redirectionControlScriptComponent != null)
        {
            redirectionControlScriptComponent.EndRedirectedWalking();
        }
    }
    public void resetRedirection()
    {
        if (redirectionControlScriptComponent != null)
        {
            redirectionControlScriptComponent.resetRedirection();
        }
    }
    public void EndRedirection()
    {
        if (redirectionControlScriptComponent != null)
        {
            redirectionControlScriptComponent.EndRedirection();
        }
    }

    public void ActivatePlayerPositionController()
    {
        if (playerPositionControllerScriptComponent != null)
        {
            playerPositionControllerScriptComponent.enabled = true;
            playerPositionControllerScriptComponent.ActivatePlayerPositioning();
        }
    }

    public void ConfigurePlayerPositionController()
    {
        if (playerPositionControllerScriptComponent != null)
        {
            playerPositionControllerScriptComponent.enabled = true;
        }
    }
    public void ActivateStandingPosition()
    {
        if (standingPositionScriptComponent != null)
        {

            ConfigureStandingPosition();
            standingPositionScriptComponent.enabled = true;

        }
    }
    public void SavePositionAndRotationaveStandingPosition()
    {
        if (standingPositionScriptComponent != null)
        {
            standingPositionScriptComponent.CallSavePositionAndRotation();
        }
    }
    public void SetAndSynchronizeStandingPosition()
    {
        if (standingPositionScriptComponent != null)
        {
            standingPositionScriptComponent.SavePositionAndRotationRPC();
            standingPositionScriptComponent.SpawnVirtualCloneWithOffset(0);
        }
    }
    public void SavePositionAndRotationToFaceObject()
    {
        if (standingPositionScriptComponent != null)
        {
            standingPositionScriptComponent.CallSavePositionAndRotationToFaceObjectRPC();
            standingPositionScriptComponent.SpawnVirtualCloneWithOffset(0);
        }
    }


    public void spawnStandingGoalObject()
    {
        if (standingPositionScriptComponent != null)
        {
            Debug.Log("spawnStandingGoalObject()");
            ConfigureStandingPosition();
            standingPositionScriptComponent.CallSpawnVirtualCloneWithOffset();
        }
    }
    public void deleteStandingGoalObject()
    {
        if (standingPositionScriptComponent != null)
        {
            standingPositionScriptComponent.CallDeleteAllSpawnedVirtualClones();
        }
    }
    public void ConfigureStandingPosition()
    {
        if (standingPositionScriptComponent != null)
        {
            // Configure the StandingPosition component
            // For example, set the objectToSpawn, virtualAvatar, and realAvatar
            standingPositionScriptComponent.objectToSpawn = standingGoalObject;
            standingPositionScriptComponent.realAvatar = realAvatar;
            standingPositionScriptComponent.virtualAvatar = virtualAvatar;
            standingPositionScriptComponent.offset = standingPositionOffset;
            standingPositionScriptComponent.offsetOther = standingPositionOffsetOther;

        }
    }
    public void SetupTrial(float offset, bool liveRedirection, float redirectedWalkingIntensity, float redirectionSliderValue)
    {        
        this.standingPositionOffset = offset;
        this.standingPositionOffsetOther = offset;
        this.liveRedirection = liveRedirection;
        this.redirectIntensity = redirectedWalkingIntensity;
        this.redirectionSliderValue = redirectionSliderValue;
    }
}