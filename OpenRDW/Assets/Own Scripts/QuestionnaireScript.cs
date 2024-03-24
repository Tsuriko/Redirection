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

    void Start()
    {
        _vrQuestionnaireToolkit = GameObject.Find("VRQuestionnaireToolkit");
        vivePointers = GameObject.Find("VivePointers");
        _generateQuestionnaire = _vrQuestionnaireToolkit.GetComponentInChildren<GenerateQuestionnaire>();
        EnableQuestionnaire(false); // Disable the questionnaire by default
       
        // Subscribe to the questionnaire finished event
        _exportToCsvScript = _vrQuestionnaireToolkit.GetComponentInChildren<ExportToCSV>();
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
        // Handle the questionnaire submit event here
        Debug.Log("Questionnaire submitted!");

        // Example action: Disable questionnaire after submission
        EnableQuestionnaire(false);

        // Or move to the next questionnaire, log data, etc.
    }
}