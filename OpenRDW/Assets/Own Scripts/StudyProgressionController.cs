using UnityEngine;
using System.Collections.Generic;

public class StudyProgressionController : MonoBehaviour
{

    public static StudyProgressionController instance;
    private GlobalScript globalScript;
    private RandomVariablesManager randomVariablesManager;
    private List<RandomVariablesManager.VariablesCombination> allPossibleCombinations;

    private float currentOffsetMaster;
    private float currentOffsetOther;
    private bool currentLiveRedirection;
    private float currentRedirectedWalkingIntensity;
    private int currentRandomTask = 0;
    private bool firstTaskDone = false;





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
        allPossibleCombinations = new List<RandomVariablesManager.VariablesCombination>(randomVariablesManager.AllCombinations);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TriggerNextAction();
        }
    }

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
            TriggerNextAction();
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
        //globalScript.SavePositionAndRotationaveStandingPosition();
        Debug.Log("Press Space to continue. Next step is the first task without redirection");
        nextAction = ActionAwaiting.FirstTask;
    }

    private void StartFirstTask()
    {
        Debug.Log("Starting First Task");
        SetVariablesCombination(0, 0, false, 1);

        nextAction = ActionAwaiting.TaskPreparation;
    }

    private void SetVariablesCombination(float offsetMaster, float offsetOther, bool liveRedirection, float redirectedWalkingIntensity)
    {
        this.currentOffsetMaster = offsetMaster;
        this.currentOffsetOther = offsetOther;
        this.currentLiveRedirection = liveRedirection;
        this.currentRedirectedWalkingIntensity = redirectedWalkingIntensity;
    }
    private void StartRandomTask()
    {
        Debug.Log("Starting Random Task Number " + currentRandomTask + " of " + allPossibleCombinations.Count);
        RandomVariablesManager.VariablesCombination currentCombination = allPossibleCombinations[currentRandomTask];
        SetVariablesCombination(currentCombination.OffsetMaster, currentCombination.OffsetOther, currentCombination.liveRedirection, currentCombination.redirectedWalkingIntensity);
        nextAction = ActionAwaiting.TaskPreparation;
    }

    private void PrepareTask()
    {
        Debug.Log("Preparing Task");
        globalScript.SetupTrial(currentOffsetMaster, currentOffsetOther, currentLiveRedirection, currentRedirectedWalkingIntensity);
        globalScript.ConfigurePlayerPositionController();
        globalScript.ActivatePlayerPositionController();
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
        //globalScript.spawnStandingGoalObject();
        nextAction = ActionAwaiting.TaskReset;
    }

    private void ResetTask()
    {
        Debug.Log("Resetting Task");
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


}