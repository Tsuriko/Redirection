using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // Include the Photon PUN namespace
using HR_Toolkit;

public class GlobalScript : MonoBehaviour
{
    [Header("Activation Keys")]
    [Tooltip("Key to activate Player Synchronization.")]
    public KeyCode playerSyncActivationKey = KeyCode.Y;
    [Tooltip("Key to activate Redirection Control.")]
    public KeyCode redirectionControlActivationKey = KeyCode.R;
    [Tooltip("Key to activate Attach Redirection Targets.")]
    public KeyCode attachRedirectionTargetsActivationKey = KeyCode.C; // Key for activating AttachRedirectionTargets

    [Header("Player Synchronization Settings")]
    [Tooltip("Target location for player synchronization.")]
    public Transform playerSyncTargetLocation;

    [Header("Real Object To Virtual Settings")]
    [Tooltip("Virtual avatar for real to virtual object mapping.")]
    public Transform virtualAvatar;
    [Tooltip("List of objects to clone into the virtual environment.")]
    public List<GameObject> objectsToClone;

    [Header("Redirection Settings")]
    [Tooltip("User transform for redirection.")]
    public Transform redirectionUser;
    [Tooltip("Target transform for redirection.")]
    public Transform redirectionTarget;
    [Range(0f, 1f)]
    [Tooltip("Slider value to adjust redirection intensity.")]
    public float redirectionSliderValue = 0.5f;

    // Reference to the AttachRedirectionTargets script
    [Header("Attach Redirection Targets")]
    [Tooltip("Attach Redirection Targets script component.")]
    public AttachRedirectionTargets attachRedirectionTargetsScriptComponent;

    // References to the script components (not exposed in the Inspector)
    private PlayerSynchronization playerSyncScriptComponent;
    private RealObjectToVirtual realObjectToVirtualScriptComponent;
    private RedirectionControl redirectionControlScriptComponent;

    // Private boolean to track coroutine activation and key press activation
    private bool hasActivatedScriptsAfterDelay = false;

    void Start()
    {
        // Dynamic component discovery
        playerSyncScriptComponent = FindObjectOfType<PlayerSynchronization>();
        realObjectToVirtualScriptComponent = FindObjectOfType<RealObjectToVirtual>();
        redirectionControlScriptComponent = FindObjectOfType<RedirectionControl>();
        attachRedirectionTargetsScriptComponent = FindObjectOfType<AttachRedirectionTargets>();

        // Initial component state control
        if (playerSyncScriptComponent != null) playerSyncScriptComponent.enabled = false;
        if (realObjectToVirtualScriptComponent != null) realObjectToVirtualScriptComponent.enabled = false;
        if (redirectionControlScriptComponent != null) redirectionControlScriptComponent.enabled = false;
        if (attachRedirectionTargetsScriptComponent != null) attachRedirectionTargetsScriptComponent.enabled = false;
    }

    void Update()
    {
        // Event-driven component activation
        if (Input.GetKeyDown(playerSyncActivationKey) && !hasActivatedScriptsAfterDelay)
        {
            ActivatePlayerSynchronization();
            StartCoroutine(ActivateScriptsAfterDelay(3)); // Delayed activation and configuration
            hasActivatedScriptsAfterDelay = true;
        }

        if (Input.GetKeyDown(redirectionControlActivationKey) && PhotonNetwork.IsMasterClient)
        {
            ActivateRedirectionLogic();
        }

        // Activation of AttachRedirectionTargets with the 'X' key
        if (Input.GetKeyDown(attachRedirectionTargetsActivationKey))
        {
            if (attachRedirectionTargetsScriptComponent != null)
            {
                attachRedirectionTargetsScriptComponent.enabled = true;
                attachRedirectionTargetsScriptComponent.HandleKeyPress();

            }
        }
    }

    private void ActivatePlayerSynchronization()
    {
        if (playerSyncScriptComponent != null)
        {
            playerSyncScriptComponent.enabled = true;
            playerSyncScriptComponent.MovePlayer();
            ConfigurePlayerSynchronization(); // Centralized configuration of component variables
        }
    }

    private IEnumerator ActivateScriptsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (realObjectToVirtualScriptComponent != null)
        {
            realObjectToVirtualScriptComponent.enabled = true;
            ConfigureRealObjectToVirtual(); // Configuration method implementation
            realObjectToVirtualScriptComponent.EnableScript();
        }

        // No need for additional delay for AttachRedirectionTargets here since it's activated by key press

        if (redirectionControlScriptComponent != null)
        {
            ConfigureRedirectionControl(); // Activation logic customization
            redirectionControlScriptComponent.enabled = true;
        }
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

    private void ConfigureRedirectionControl()
    {
        if (redirectionControlScriptComponent != null)
        {
            redirectionControlScriptComponent.user = redirectionUser;
            redirectionControlScriptComponent.target = redirectionTarget;
            redirectionControlScriptComponent.sliderValue = redirectionSliderValue;
        }
    }

    private void ActivateRedirectionLogic()
    {
        if (redirectionControlScriptComponent != null && redirectionControlScriptComponent.enabled && PhotonNetwork.IsMasterClient)
        {
            redirectionControlScriptComponent.StartRedirectionExternally();
        }
    }
}