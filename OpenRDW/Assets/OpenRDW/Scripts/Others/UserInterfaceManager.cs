using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UserInterfaceManager : MonoBehaviour
{
    [Tooltip("The canvas which shows hints")]
    public Canvas canvasOverlay;
    private GameObject panelExperimentComplete;
    [HideInInspector]
    public List<string> commandFiles;

    private GlobalConfiguration globalConfiguration;
    private void Awake()
    {
        globalConfiguration = GetComponent<GlobalConfiguration>();
        panelExperimentComplete = canvasOverlay.transform.Find("PanelExperimentComplete").gameObject;
    }
    private void Start()
    {

    }
    
    public void SetActivePanelExperimentComplete(bool active) {
        panelExperimentComplete.SetActive(active);
    }
}
