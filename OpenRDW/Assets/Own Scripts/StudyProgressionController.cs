using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class StudyProgressionController : MonoBehaviour
{

    public static StudyProgressionController instance;
    public int StudyID = 0;
    private GlobalScript globalScript;
    private RandomVariablesManager randomVariablesManager;
    private List<RandomVariablesManager.VariablesCombination> allPossibleCombinations;
    private StudyLogger studyLogger;
    public QuestionnaireScript questionaireScript;

    [HideInInspector] public float offsetValue;
    [HideInInspector] public int personRedirected;
    [HideInInspector] public float currentOffsetMaster;
    [HideInInspector] public float currentOffsetOther;
    [HideInInspector] public bool currentLiveRedirection;
    [HideInInspector] public float currentRedirectedWalkingIntensity;

    public int currentRandomTask = 0;
    private bool firstTaskDone = false;
private PhotonView photonView;

    private enum ActionAwaiting
    {
        None,
        InitializeStudy,
        SyncPlayers,
        SaveMarkerPosition,
        TaskPreparation,
        TaskExecution,
        TaskReset,
        TaskReview,
        DetermineNextTask,
        FirstTask,
        RandomTask,
    }

    private ActionAwaiting nextAction = ActionAwaiting.InitializeStudy;

    void Start()
    {
        globalScript = FindObjectOfType<GlobalScript>();
        randomVariablesManager = FindObjectOfType<RandomVariablesManager>();
        randomVariablesManager.SetSeedWithStudyId(StudyID);
        randomVariablesManager.GenerateAllPossibleCombinations();
        allPossibleCombinations = new List<RandomVariablesManager.VariablesCombination>(randomVariablesManager.AllCombinations);
        studyLogger = FindObjectOfType<StudyLogger>();
        photonView = GetComponent<PhotonView>();
        questionaireScript = FindObjectOfType<QuestionnaireScript>();
        questionaireScript.enabled = true;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerNextAction();
        }
        if (Input.GetKeyDown(KeyCode.S) && nextAction == ActionAwaiting.FirstTask)
        {
            SynchronizeStandingPositionLocal();
        }
    }

    [PunRPC]
    private void TriggerNextAction()
    {
        switch (nextAction)
        {
            case ActionAwaiting.InitializeStudy:
                InitializeStudy();
                break;
            case ActionAwaiting.SyncPlayers:
                SyncPlayers();
                break;
            case ActionAwaiting.SaveMarkerPosition:
                SaveMarkerPosition();
                break;
            case ActionAwaiting.FirstTask:
                StartFirstTask();
                break;
            case ActionAwaiting.RandomTask:
                StartRandomTask();
                break;
            case ActionAwaiting.TaskPreparation:
                PrepareTask();
                break;
            case ActionAwaiting.TaskExecution:
                ExecuteTask();
                break;
            case ActionAwaiting.TaskReset:
                ResetTask();
                break;
            case ActionAwaiting.TaskReview:
                ReviewTask();
                break;
            case ActionAwaiting.DetermineNextTask:
                DetermineNextTask();
                break;
            default:
                Debug.Log("No action or unknown action awaited.");
                break;
        }
    }

    public void TriggerNextActionViaEndOfRedirection()
    {
        if (nextAction == ActionAwaiting.TaskReview)
        {

            CallTriggerNextAction();
        }
    }

    private void InitializeStudy()
    {
        Debug.Log("Initializing Study, Put the HMD on the same space like the other and press Space to continue");
        globalScript.enableKeyPresses = false;

        nextAction = ActionAwaiting.SyncPlayers;
    }

    private void SyncPlayers()
    {
        Debug.Log("Syncing Players");
        globalScript.syncPlayers();
        Debug.Log("Let Player go to marker and press Space to continue");
        nextAction = ActionAwaiting.SaveMarkerPosition;
    }

    private void SaveMarkerPosition()
    {
        Debug.Log("Saving Marker Position");
        globalScript.SavePositionAndRotationaveStandingPosition();
        Debug.Log("Use the S key to position the arrows direction to the other player. Press Space to continue. Next step is the first task without redirection");
        nextAction = ActionAwaiting.FirstTask;
    }

    private void StartFirstTask()
    {
        Debug.Log("Starting First Task");
        questionaireScript.MoveQuestionnaireBehind(GameObject.Find("Standing Position(virtual)").transform);
        globalScript.deleteStandingGoalObject();
        SetVariablesCombination(0, 0, false, 1);
        Debug.Log("Press Space to Teleport the virtual Players to their Position");
        nextAction = ActionAwaiting.TaskPreparation;
    }

    private void SetVariablesCombination(float offset, int personRedirected, bool liveRedirection, float redirectedWalkingIntensity)
    {
        this.currentLiveRedirection = liveRedirection;
        this.currentRedirectedWalkingIntensity = redirectedWalkingIntensity;
        this.offsetValue = offset;
        this.personRedirected = personRedirected;

        // Logic to distribute offset based on personRedirected
        switch (personRedirected)
        {
            case 0: // Both master and other share the offset equally
                this.currentOffsetMaster = offset / 2f;
                this.currentOffsetOther = offset / 2f;
                break;
            case 1: // Master gets all the offset, other gets none
                this.currentOffsetMaster = offset;
                this.currentOffsetOther = 0;
                break;
            case 2: // Other gets all the offset, master gets none
                this.currentOffsetMaster = 0;
                this.currentOffsetOther = offset;
                break;
            default:
                Debug.LogError("Invalid personRedirected value: " + personRedirected);
                break;
        }
    }
    private void StartRandomTask()
    {
        Debug.Log("Starting Random Task Number " + currentRandomTask + " of " + allPossibleCombinations.Count);
        RandomVariablesManager.VariablesCombination currentCombination = allPossibleCombinations[currentRandomTask];
        SetVariablesCombination(currentCombination.offsetValue, currentCombination.personRedirected, currentCombination.liveRedirection, currentCombination.redirectedWalkingIntensity);
        Debug.Log("Press Space to Teleport the virtual Players to their Position");
        nextAction = ActionAwaiting.TaskPreparation;
    }

    private void PrepareTask()
    {
        Debug.Log("Preparing Task");
        globalScript.SetupTrial(currentOffsetMaster, currentOffsetOther, currentLiveRedirection, currentRedirectedWalkingIntensity);
        globalScript.ConfigurePlayerPositionController();
        globalScript.ActivatePlayerPositionController();
        SaveInitialValues();
        Debug.Log("Position the Players on their Standing Position and press Space to start Redirection");
        nextAction = ActionAwaiting.TaskExecution;
    }

    private void ExecuteTask()
    {
        Debug.Log("Executing Task");
        //globalScript.ActivateRedirectionLogic();
        nextAction = ActionAwaiting.TaskReview;
    }
    private void ReviewTask()
    {
        Debug.Log("Reviewing Task");
        globalScript.EndHandRedirection();
        globalScript.EndRedirectedWalking();
        globalScript.spawnStandingGoalObject();
        questionaireScript.EnableQuestionnaire(true);
        nextAction = ActionAwaiting.TaskReset;
    }

    private void ResetTask()
    {
        Debug.Log("Resetting Task");
        SaveFinalValues();
        //globalScript.deleteStandingGoalObject();
        nextAction = ActionAwaiting.DetermineNextTask;
    }
    private void DetermineNextTask()
    {
        Debug.Log("Determining Next Task");
        if (!firstTaskDone)
        {
            firstTaskDone = true;
            nextAction = ActionAwaiting.RandomTask;
        }
        else
        {
            currentRandomTask++;
            if (currentRandomTask < allPossibleCombinations.Count)
            {
                nextAction = ActionAwaiting.RandomTask;
            }
            else
            {
                Debug.Log("All tasks done");
            }
        }
    }
    private void SynchronizeStandingPositionLocal()
    {
        globalScript.SetAndSynchronizeStandingPosition();
    }
    public void SaveInitialValues()
    {
        studyLogger.SaveInitialValues();
    }
    public void SaveMidValues()
    {
        studyLogger.SaveMidValues();
    }
    public void SaveFinalValues()
    {
        studyLogger.SaveFinalValues();
    }
    public void CallTriggerNextAction()
    {
        photonView.RPC("TriggerNextAction", RpcTarget.All);
    }

}