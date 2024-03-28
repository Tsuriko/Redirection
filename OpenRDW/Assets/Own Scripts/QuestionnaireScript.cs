using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRQuestionnaireToolkit;

public class QuestionnaireScript : MonoBehaviour
{
    public Transform specifiedObject; // Assign in the Inspector or dynamically
    public float offsetDistance = 2.0f; // Distance behind the specified object

    public GameObject _vrQuestionnaireToolkit;
    private GenerateQuestionnaire _generateQuestionnaire;
    private ExportToCSV _exportToCsvScript; // Assuming this script handles the submit event
    private GameObject vivePointers;
    public int questionnaireIndex = 0;
    public bool tlxactive = false;

    void Start()
    {
        _vrQuestionnaireToolkit = GameObject.Find("VRQuestionnaireToolkit");
        vivePointers = GameObject.Find("VivePointers");
        _generateQuestionnaire = _vrQuestionnaireToolkit.GetComponentInChildren<GenerateQuestionnaire>();
        EnableQuestionnaire(false); // Disable the questionnaire by default
       
        // Subscribe to the questionnaire finished event
        _exportToCsvScript = _vrQuestionnaireToolkit.GetComponentInChildren<ExportToCSV>();
        _exportToCsvScript.QuestionnaireFinishedEvent.AddListener(SwitchQuestionnaire);
    }

    void SwitchQuestionnaire()
    {

        questionnaireIndex++;
        tlxactive = false;
        EnableQuestionnaire(false);

        // Deactivate all questionnaires
        for (int i = 0; i < _generateQuestionnaire.Questionnaires.Count; i++)
        {
            _generateQuestionnaire.Questionnaires[i].SetActive(false);
        }

        // Validate index and activate the corresponding questionnaire
        if (questionnaireIndex >= 0 && questionnaireIndex < _generateQuestionnaire.Questionnaires.Count)
        {
            if (questionnaireIndex == 1 || questionnaireIndex == 3 || questionnaireIndex == 5 || questionnaireIndex == 7) tlxactive = true;
            _generateQuestionnaire.Questionnaires[questionnaireIndex].SetActive(true);
        }
        else
        {
            Debug.Log("Invalid category index");
        }
    }
    public void checkTLX()
    {
        if (tlxactive) EnableQuestionnaire(true);
    }
    void Update()
    {
        // Update logic here (if needed)
    }

    public void EnableQuestionnaire(bool enable)
    {
        // Toggles the entire VR questionnaire toolkit active state
        _vrQuestionnaireToolkit.SetActive(enable);
        vivePointers.SetActive(enable);
    }

    public void MoveQuestionnaireBehind(Transform target)
    {
        

        if (target != null)
        {
            Vector3 direction = -target.forward; // Assumes the forward direction is what you consider 'front' of the object
            Vector3 newPosition = target.position + direction * offsetDistance;
            newPosition.y = 1.5f;
            _vrQuestionnaireToolkit.transform.position = newPosition;
            
            // Optionally, adjust the rotation of the questionnaire to face towards or away from the object
            _vrQuestionnaireToolkit.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void OnQuestionnaireSubmit()
    {
        if (tlxactive) return;
        EnableQuestionnaire(false);
        StudyProgressionController.instance.QuestionSubmitted();

        // Or move to the next questionnaire, log data, etc.
    }
    public void SetParticipantID(string participantID)
    {
        _vrQuestionnaireToolkit.GetComponent<StudySetup>().ParticipantId = participantID;
    }
}