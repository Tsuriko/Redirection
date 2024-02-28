using UnityEngine;

public class StudyProgressionController : MonoBehaviour
{
    private enum StudyStep { Initialization, PlayerSync, saveMarkerPosition, FirstTask, RandomTask, Completed }

    public RandomVariablesManager randomVariablesManager;
    public GlobalScript globalScript;
    private StudyStep currentStep = StudyStep.Initialization;
    public RandomVariablesManager.VariablesCombination randomCombination;

    void Start()
    {
        globalScript = GetComponent<GlobalScript>();
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
        currentStep++;
        switch (currentStep)
        {
            case StudyStep.Initialization:
                InitializeStudy();
                break;
            case StudyStep.PlayerSync:
                SyncPlayers();
                break;
            case StudyStep.saveMarkerPosition:
                SaveMarkerPosition();
                break;
            case StudyStep.FirstTask:
                StartFirstTask();
                break;
            case StudyStep.RandomTask:
                StartRandomTask();
                break;
            case StudyStep.Completed:
                CompleteStudy();
                break;
        }
    }

    private void InitializeStudy()
    {
        Debug.Log("Initializing Study, Put the HMD on the same space like the other and press Space to continue");
        globalScript.enableKeyPresses = false;
        SelectRandomCombination();

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
        globalScript.targetDistanceBetweenPlayers = 4;
        globalScript.rotationOffsetMaster = 0;
        globalScript.rotationOffsetOther = 0;
        globalScript.ConfigurePlayerPositionController();
        globalScript.ActivatePlayerPositionController();

    }

    private void StartRandomTask()
    {
        Debug.Log("Starting Random Task");
        // Example of accessing a random combination:
        var randomCombination = SelectRandomCombination();
        // Use the randomCombination for the task
        // Example: Debug.Log($"Using algorithm: {randomCombination.SelectedAlgorithm}");
    }

    private void CompleteStudy()
    {
        Debug.Log("Study Completed");
        // Insert completion logic here
    }

    private RandomVariablesManager.VariablesCombination SelectRandomCombination()
    {
        if (randomVariablesManager == null || randomVariablesManager.AllCombinations.Count == 0)
        {
            Debug.LogError("RandomVariablesManager is not set or has no combinations.");
            return default;
        }

        int randomIndex = UnityEngine.Random.Range(0, randomVariablesManager.AllCombinations.Count);
        Debug.Log(randomVariablesManager.AllCombinations.Count);
        Debug.Log(randomVariablesManager.AllCombinations[randomIndex]);
        randomCombination = randomVariablesManager.AllCombinations[randomIndex];
        return randomVariablesManager.AllCombinations[randomIndex];
    }
}