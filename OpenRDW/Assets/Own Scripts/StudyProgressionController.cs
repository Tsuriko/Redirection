using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class StudyProgressionController : MonoBehaviour
{

    public static StudyProgressionController instance;
    public int StudyID = 1;
    public string ParticipantId = "0";
    private GlobalScript globalScript;
    private RandomVariablesManager randomVariablesManager;
    public List<List<RandomVariablesManager.VariablesCombination>> studyCategoryOrderList = new List<List<RandomVariablesManager.VariablesCombination>>();
    public int currentStudyCategoryIndex = 0;
    private StudyLogger studyLogger;
    public QuestionnaireScript questionaireScript;
    public List<RandomVariablesManager.VariablesCombination> currenCategoryList;
    public int currentCategoryIndex = 0;
    public RandomVariablesManager.VariablesCombination currentCombination;
    public bool IsQuestionSubmitted = false;
    public bool IsOtherQuestionSubmitted = false;
    public int TaskNumber = 0;

    [HideInInspector] public RandomVariablesManager.TaskCategory taskCategory;
    [HideInInspector] public float offsetValue = 0;
    [HideInInspector] public bool currentLiveRedirection = false;
    [HideInInspector] public float currentRedirectedWalkingIntensity = 0;
    [HideInInspector] public float currentRedirectionSliderValue = 0;

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
            if (nextAction == ActionAwaiting.TaskPreparation || nextAction == ActionAwaiting.TaskExecution || nextAction == ActionAwaiting.TaskReview || nextAction == ActionAwaiting.TaskReset || nextAction == ActionAwaiting.FirstTask || nextAction == ActionAwaiting.RandomTask || nextAction == ActionAwaiting.SaveMarkerPosition)
            {
                CallTriggerNextAction();
            }
            if (nextAction == ActionAwaiting.InitializeStudy || nextAction == ActionAwaiting.SyncPlayers )
            {
                TriggerNextAction();
            }
        }
        if (Input.GetKeyDown(KeyCode.S) && (nextAction == ActionAwaiting.FirstTask || nextAction == ActionAwaiting.RandomTask))
        {
            CallSynchronizeStandingPosition();
        }
        if (Input.GetKeyDown(KeyCode.Y) && (nextAction == ActionAwaiting.FirstTask || nextAction == ActionAwaiting.RandomTask || nextAction == ActionAwaiting.SaveMarkerPosition))
        {
            globalScript.syncPlayers();
        }
        if (Input.GetKeyDown(KeyCode.C) && nextAction == ActionAwaiting.TaskExecution)
        {
            globalScript.activateAttachRedirectionTargetsScript();
        }
        if (IsQuestionSubmitted && IsOtherQuestionSubmitted && nextAction == ActionAwaiting.TaskReset)
        {
            IsQuestionSubmitted = false;
            IsOtherQuestionSubmitted = false;
            CallTriggerNextAction();
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
        SetParticipantID();

        Debug.Log("Initializing Study, Put the HMD on the same space like the other and press Space to continue");
        globalScript.enableKeyPresses = false;


        nextAction = ActionAwaiting.SyncPlayers;
    }

    private void SyncPlayers()
    {
        Debug.Log("Syncing Players");
        globalScript.syncPlayers();
        Debug.Log("Let Player put the HMD on and let them to the real marker and press Space to continue. Now only the master client can press Space");
        nextAction = ActionAwaiting.SaveMarkerPosition;
    }

    private void SaveMarkerPosition()
    {
        Debug.Log("Saving Marker Position");
        SynchronizeStandingPositionToFaceObject();
        Debug.Log("Use the S key to position the arrows direction to the other player. Press Space to continue. ");
        nextAction = ActionAwaiting.FirstTask;
    }

    private void StartFirstTask()
    {
        Debug.Log("Starting First Task");
        studyLogger.SetupNewParticipant("RDW Test", ParticipantId, StudyID);

        SetVariablesCombination(RandomVariablesManager.TaskCategory.FirstTask, 0, false, 0, 0);
        Debug.Log("Let the player face each other and press Space to Teleport the virtual Players to their Position. The first task will start");
        globalScript.SetupTrial(offsetValue, currentLiveRedirection, currentRedirectedWalkingIntensity, currentRedirectionSliderValue);
        //nextAction = ActionAwaiting.TaskPreparation;
        nextAction = ActionAwaiting.TaskPreparation;
    }

    private void SetVariablesCombination(RandomVariablesManager.TaskCategory taskCategory, float offset, bool liveRedirection, float redirectedWalkingIntensity, float redirectionSliderValue)
    {

        this.taskCategory = taskCategory;
        this.currentLiveRedirection = liveRedirection;
        this.currentRedirectedWalkingIntensity = redirectedWalkingIntensity;
        this.offsetValue = offset;
        this.currentRedirectionSliderValue = redirectionSliderValue;
    }
    private void StartRandomTask()
    {
        Debug.Log("Starting Category " + currentStudyCategoryIndex + " Task Number " + (currentCategoryIndex+1) + " of " + currenCategoryList.Count);
        currentCombination = currenCategoryList[currentCategoryIndex];
        SetVariablesCombination(currentCombination.taskCategory, currentCombination.offsetValue, currentCombination.liveRedirection, currentCombination.redirectedWalkingIntensity, currentCombination.redirectionSliderValue);
        globalScript.SetupTrial(offsetValue, currentLiveRedirection, currentRedirectedWalkingIntensity, currentRedirectionSliderValue);
        if (IsMasterClient) globalScript.spawnStandingGoalObject();
        Debug.Log("Position the Players on their Standing Position. Press Space to Teleport the virtual Players to their Position");
        nextAction = ActionAwaiting.TaskPreparation;
        //nextAction = ActionAwaiting.TaskExecution;
    }

    private void PrepareTask()
    {
        if (firstTaskDone)
        {
            globalScript.ActivatePlayerPositionController();
        }
        
        globalScript.activateAttachRedirectionTargetsScript();
        questionaireScript.MoveQuestionnaireBehind(GameObject.Find("Standing Position(virtual)").transform);
        globalScript.deleteStandingGoalObject();
        nextAction = ActionAwaiting.TaskExecution;

    }

    private void ExecuteTask()
    {
        
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
        questionaireScript.EnableQuestionnaire(false);
        questionaireScript.checkTLX();
        studyLogger.WriteAllStudyData(TaskNumber);
        TaskNumber++;
        Debug.Log("Break Time. Press Space to continue");
        if (!firstTaskDone)
        {
            firstTaskDone = true;
            nextAction = ActionAwaiting.RandomTask;
        }
        else
        {
            currentCategoryIndex++;
            if (currentCategoryIndex < currenCategoryList.Count)
            {
                nextAction = ActionAwaiting.RandomTask;
            }
            else
            {
                Debug.Log("Catgory " + currentStudyCategoryIndex + " done. Press Space to continue with the next Category");
                currentStudyCategoryIndex++;
                if (currentStudyCategoryIndex < studyCategoryOrderList.Count)
                {
                    currenCategoryList = studyCategoryOrderList[currentStudyCategoryIndex];
                    currentCategoryIndex = 0;
                    nextAction = ActionAwaiting.RandomTask;
                }
                else
                {
                    Debug.Log("All Categories done."); 
                }
            }
        }
    }
    [PunRPC]
    private void SynchronizeStandingPositionLocal()
    {
        globalScript.SetAndSynchronizeStandingPosition();
        Debug.Log("SynchronizeStandingPositionLocal");
    }
    [PunRPC]
    private void SynchronizeStandingPositionToFaceObject()
    {
        globalScript.SavePositionAndRotationToFaceObject();
        
    }
    public void CallSynchronizeStandingPosition()
    {
        if (IsMasterClient) photonView.RPC("SynchronizeStandingPositionToFaceObject", RpcTarget.All);
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
        //randomVariablesManager.GenerateStudyListOrder();
        randomVariablesManager.GenerateStudyOrderCombination(StudyID);
        studyCategoryOrderList = randomVariablesManager.studyOrderCombination;
        currenCategoryList = studyCategoryOrderList[currentStudyCategoryIndex];
    }
    public bool isTaskReview()
    {
        return nextAction == ActionAwaiting.TaskReview;
    }
    public void QuestionSubmitted()
    {
        IsQuestionSubmitted = true;
        photonView.RPC("OtherQuestionSubmitted", RpcTarget.Others);
    }
    public void OnNextPage()
    {
        questionaireScript.OnQuestionnaireSubmit();
    }
    [PunRPC]
    public void OtherQuestionSubmitted()
    {
        IsOtherQuestionSubmitted = true;
    }
    public void SetParticipantID()
    {
        questionaireScript.SetParticipantID(ParticipantId);

    }

}