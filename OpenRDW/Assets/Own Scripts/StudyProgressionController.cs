using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class StudyProgressionController : MonoBehaviour
{

    public static StudyProgressionController instance;
    public int StudyID = 0;
    private GlobalScript globalScript;
    private RandomVariablesManager randomVariablesManager;
    public List<RandomVariablesManager.VariablesCombination> allPossibleCombinations;
    private StudyLogger studyLogger;
    public QuestionnaireScript questionaireScript;
    public RandomVariablesManager.VariablesCombination currentCombination;

    [HideInInspector] public float offsetValue = 1;
    [HideInInspector] public int personRedirected = 0;
    [HideInInspector] public float currentOffsetMaster = 0;
    [HideInInspector] public float currentOffsetOther = 0;
    [HideInInspector] public bool currentLiveRedirection = false;
    [HideInInspector] public float currentRedirectedWalkingIntensity = 0;

    public int currentRandomTask = 0;
    private bool firstTaskDone = false;
    private PhotonView photonView;
    public bool IsMasterClient;

    public enum ActionAwaiting
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

    public ActionAwaiting nextAction = ActionAwaiting.InitializeStudy;

    void Start()
    {
        globalScript = FindObjectOfType<GlobalScript>();
        randomVariablesManager = FindObjectOfType<RandomVariablesManager>();

        studyLogger = FindObjectOfType<StudyLogger>();
        photonView = GetComponent<PhotonView>();
        questionaireScript = FindObjectOfType<QuestionnaireScript>();
        questionaireScript.enabled = true;
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (nextAction == ActionAwaiting.TaskPreparation || nextAction == ActionAwaiting.TaskExecution || nextAction == ActionAwaiting.TaskReset || nextAction == ActionAwaiting.TaskReview || nextAction == ActionAwaiting.FirstTask || nextAction == ActionAwaiting.RandomTask)
            {
                CallTriggerNextAction();
            }
            if (nextAction == ActionAwaiting.InitializeStudy || nextAction == ActionAwaiting.SyncPlayers || nextAction == ActionAwaiting.SaveMarkerPosition)
            {
                TriggerNextAction();
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && (nextAction == ActionAwaiting.FirstTask || nextAction == ActionAwaiting.RandomTask))
        {
            SynchronizeStandingPositionLocal();
        }
        if (Input.GetKeyDown(KeyCode.Y) && (nextAction == ActionAwaiting.FirstTask || nextAction == ActionAwaiting.RandomTask || nextAction == ActionAwaiting.SaveMarkerPosition))
        {
            SyncPlayers();
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
            case ActionAwaiting.TaskReview:
                ReviewTask();
                break;
            case ActionAwaiting.TaskReset:
                ResetTask();
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
            Debug.Log("TriggerNextActionViaEndOfRedirection");
        }
    }

    private void InitializeStudy()
    {
        IsMasterClient = PhotonNetwork.IsMasterClient;
        if (IsMasterClient)
        {
            photonView.RPC("UpdateStudyID", RpcTarget.All, StudyID);
        }
        Debug.Log("Initializing Study, Put the HMD on the same space like the other and press Space to continue");
        globalScript.enableKeyPresses = false;
        

        nextAction = ActionAwaiting.SyncPlayers;
    }

    private void SyncPlayers()
    {
        Debug.Log("Syncing Players");
        globalScript.syncPlayers();
        Debug.Log("Let Player put the HMD on and let them to the real marker and press Space to continue");
        nextAction = ActionAwaiting.SaveMarkerPosition;
    }

    private void SaveMarkerPosition()
    {
        Debug.Log("Saving Marker Position");
        SynchronizeStandingPositionLocal();
        Debug.Log("Use the S key to position the arrows direction to the other player. Press Space to continue. Now only the master client can press Space");
        nextAction = ActionAwaiting.FirstTask;
    }

    private void StartFirstTask()
    {
        Debug.Log("Starting First Task");
        studyLogger.SetupNewParticipant("RDW Test", StudyID);
        questionaireScript.MoveQuestionnaireBehind(GameObject.Find("Standing Position(virtual)").transform);
        SetVariablesCombination(0, 0, false, 1);
        Debug.Log("Let the player face each other and press Space to Teleport the virtual Players to their Position. The first task will start" );
        globalScript.SetupTrial(currentOffsetMaster, currentOffsetOther, currentLiveRedirection, currentRedirectedWalkingIntensity);
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
        currentCombination = allPossibleCombinations[currentRandomTask];
        SetVariablesCombination(currentCombination.offsetValue, currentCombination.personRedirected, currentCombination.liveRedirection, currentCombination.redirectedWalkingIntensity);
        globalScript.SetupTrial(currentOffsetMaster, currentOffsetOther, currentLiveRedirection, currentRedirectedWalkingIntensity);
        if(IsMasterClient) globalScript.spawnStandingGoalObject();
        Debug.Log("Position the Players on their Standing Position. Press Space to Teleport the virtual Players to their Position");
        nextAction = ActionAwaiting.TaskPreparation;
    }

    private void PrepareTask()
    {
        Debug.Log("Preparing Task");
        globalScript.
        if (firstTaskDone) {
            //globalScript.ConfigurePlayerPositionController();
            if (IsMasterClient) globalScript.ActivatePlayerPositionController();
        }
        Debug.Log("Press Space to start Redirection");
        nextAction = ActionAwaiting.TaskExecution;
    }

    private void ExecuteTask()
    {
        globalScript.activateAttachRedirectionTargetsScript();
        globalScript.activateAttachRedirectionTargetsScript();
        SaveInitialValues();
        if (firstTaskDone)
        {
            globalScript.resetRedirection();
            globalScript.ActivateRedirectionLogic();
        }
        globalScript.deleteStandingGoalObject();
        
        Debug.Log("Executing Task");
        
        nextAction = ActionAwaiting.TaskReview;
    }
    private void ReviewTask()
    {
        Debug.Log("Reviewing Task");
        if (firstTaskDone)
        {
            globalScript.EndRedirection();
        }
        //if (IsMasterClient) globalScript.spawnStandingGoalObject();
        questionaireScript.EnableQuestionnaire(true);
        Debug.Log("Let the Player fill out the Questionnaire and press Space to continue. Next step is to reset the task");
        nextAction = ActionAwaiting.TaskReset;
    }

    private void ResetTask()
    {
        Debug.Log("Resetting Task");
        SaveFinalValues();
        //globalScript.deleteStandingGoalObject();
        studyLogger.WriteAllStudyData(!firstTaskDone ? -1 : currentRandomTask);
        Debug.Log("Break Time. Press Space to continue");
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
        Debug.Log("SynchronizeStandingPositionLocal");
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
        if (IsMasterClient) photonView.RPC("TriggerNextAction", RpcTarget.All);
    }
    [PunRPC]
    public void UpdateStudyID(int newStudyID)
    {
        StudyID = newStudyID;
        randomVariablesManager.SetSeedWithStudyId(StudyID);
        randomVariablesManager.GenerateAllPossibleCombinationsRandomly();
        allPossibleCombinations = new List<RandomVariablesManager.VariablesCombination>(randomVariablesManager.AllCombinations);
    }
    public bool isTaskReview()
    {
        return nextAction == ActionAwaiting.TaskReview;
    }

}