using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRQuestionnaireToolkit;

public class QuestionnaireScript : MonoBehaviour
{
    public Transform specifiedObject; // Assign in the Inspector or dynamically
    public float offsetDistance = 2.0f; // Distance behind the specified object

    private GameObject _vrQuestionnaireToolkit;
    private GenerateQuestionnaire _generateQuestionnaire;
    private ExportToCSV _exportToCsvScript; // Assuming this script handles the submit event

    void Start()
    {
        _vrQuestionnaireToolkit = GameObject.FindGameObjectWithTag("VRQuestionnaireToolkit");
        _generateQuestionnaire = _vrQuestionnaireToolkit.GetComponentInChildren<GenerateQuestionnaire>();

        // Subscribe to the questionnaire finished event
        _exportToCsvScript = _vrQuestionnaireToolkit.GetComponentInChildren<ExportToCSV>();
        if (_exportToCsvScript != null)
        {
            _exportToCsvScript.QuestionnaireFinishedEvent.AddListener(OnQuestionnaireSubmit);
        }
    }

    void Update()
    {
        // Update logic here (if needed)
    }

    public void EnableQuestionnaire(bool enable)
    {
        // Toggles the entire VR questionnaire toolkit active state
        _vrQuestionnaireToolkit.SetActive(enable);
    }

    public void MoveQuestionnaireBehind(Transform target)
    {
        if (target != null)
        {
            Vector3 direction = -target.forward; // Assumes the forward direction is what you consider 'front' of the object
            Vector3 newPosition = target.position + direction * offsetDistance;
            _vrQuestionnaireToolkit.transform.position = newPosition;
            // Optionally, adjust the rotation of the questionnaire to face towards or away from the object
            _vrQuestionnaireToolkit.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnQuestionnaireSubmit()
    {
        // Handle the questionnaire submit event here
        Debug.Log("Questionnaire submitted!");

        // Example action: Disable questionnaire after submission
        EnableQuestionnaire(false);

        // Or move to the next questionnaire, log data, etc.
    }
}