using UnityEngine;

public class StudyProgressionController : MonoBehaviour
{
    private enum StudyStep { Initialization, InitializeStudy, PlayerSync, SaveMarkerPosition, FirstTask, Break, RandomTask, Break2, Completed }
    private enum TrialStep { Prepare, Execute, Review, None }

    public RandomVariablesManager randomVariablesManager;
    public GlobalScript globalScript;
    private StudyStep currentStep = StudyStep.Initialization;
    private TrialStep currentTrialStep = TrialStep.None;
    public RandomVariablesManager.VariablesCombination randomCombination;
    private int currentCombinationIndex = 0;
    private int totalRandomTasksCompleted = 0;

    void Start()
    {
        globalScript = FindObjectOfType<GlobalScript>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceToNextStep();
        }
    }

    private void AdvanceToNextStep()
    {
        if (currentStep == StudyStep.FirstTask || (currentStep == StudyStep.RandomTask && currentTrialStep != TrialStep.None))
        {
            AdvanceTrialStep();
            return;
        }

        if (currentStep == StudyStep.RandomTask)
        {
            totalRandomTasksCompleted++;
            if (totalRandomTasksCompleted < randomVariablesManager.AllCombinations.Count)
            {
                Debug.Log(totalRandomTasksCompleted);
                PrepareRandomTask(); // Prepare the next random task immediately
                return;
            }
        }
        if (currentStep == StudyStep.RandomTask && totalRandomTasksCompleted >= randomVariablesManager.AllCombinations.Count)
        {
            currentStep = StudyStep.Completed; // Move directly to Completed step
            CompleteStudy(); // Call the method to complete the study
            return;
        }

        currentStep++;
        Debug.Log(currentStep);
        switch (currentStep)
        {
            case StudyStep.Initialization:
                break;
            case StudyStep.InitializeStudy:
                InitializeStudy();
                break;
            case StudyStep.PlayerSync:
                SyncPlayers();
                break;
            case StudyStep.SaveMarkerPosition:
                SaveMarkerPosition();
                break;
            case StudyStep.FirstTask:
                PrepareFirstTask();
                break;
            case StudyStep.Break:
                break;
            case StudyStep.RandomTask:
                totalRandomTasksCompleted = 0;
                PrepareRandomTask();
                break;
            case StudyStep.Break2:
                break;
            case StudyStep.Completed:
                CompleteStudy();
                break;
            default:
                Debug.LogError("Unhandled study step!");
                break;
        }
    }

    private void AdvanceTrialStep()
    {
        Debug.Log(currentTrialStep);
        switch (currentTrialStep)
        {
            case TrialStep.None:
                currentTrialStep = TrialStep.Prepare;
                PrepareTrial();
                break;
            case TrialStep.Prepare:
                currentTrialStep = TrialStep.Execute;
                ExecuteTrial();
                break;
            case TrialStep.Execute:
                currentTrialStep = TrialStep.Review;
                ReviewTrial();
                break;
            case TrialStep.Review:
                currentTrialStep = TrialStep.None;
                currentStep++;
                AdvanceToNextStep();
                break;
            default:
                Debug.LogError("Unhandled trial step!");
                break;
        }
    }

    private void PrepareTrial()
    {

        Debug.Log("Preparing trial...");
    }

    private void ExecuteTrial()
    {

        Debug.Log("Executing trial...");
    }

    private void ReviewTrial()
    {

        Debug.Log("Reviewing trial...");
    }

    private void PrepareFirstTask()
    {
        Debug.Log("Preparing First Task...");
        StartFirstTask(); // Call StartFirstTask directly if preparation is part of starting the task
    }

    private void PrepareRandomTask()
    {
        Debug.Log("Preparing Random Task...");
        StartRandomTask(); // Similar to above, call StartRandomTask directly if preparation is part of starting the task
    }
    private void InitializeStudy()
    {
        Debug.Log("Initializing Study, Put the HMD on the same space like the other and press Space to continue");
        globalScript.enableKeyPresses = false;

    }
    private void SyncPlayers()
    {
        Debug.Log("Syncing Players");
        globalScript.syncPlayers();
        Debug.Log("Let Player go to marker and press Space to continue");
    }
    private void SaveMarkerPosition()
    {
        Debug.Log("Saving Marker Position");
        globalScript.SavePositionAndRotationaveStandingPosition();
        Debug.Log("Press Space to continue. Next step is the first task without redirection");
    }
    private void StartFirstTask()
    {
        Debug.Log("Starting First Task");
        globalScript.SetupTrial(0, 1, false, 4, 0);

        globalScript.ConfigurePlayerPositionController();
        globalScript.ActivatePlayerPositionController();
    }

    private void StartRandomTask()
    {
        Debug.Log("Starting Random Task");

    }

    private void CompleteStudy()
    {
        Debug.Log("Study Completed");

    }

    private RandomVariablesManager.VariablesCombination SelectNextCombination()
    {
        if (randomVariablesManager == null || randomVariablesManager.AllCombinations.Count == 0)
        {
            Debug.LogError("RandomVariablesManager is not set or has no combinations.");
            return default;
        }

        // Ensure the index is within the bounds of the list
        if (currentCombinationIndex >= randomVariablesManager.AllCombinations.Count)
        {
            Debug.LogError("No more combinations to select.");
            return default; // Or loop back to the first combination depending on your requirements
        }

        randomCombination = randomVariablesManager.AllCombinations[currentCombinationIndex];
        currentCombinationIndex++; // Move to the next combination for the next call
        return randomCombination;
    }

}