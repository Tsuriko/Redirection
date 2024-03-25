using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StudyLogger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]

    [Header("Info -- Readonly")]
    public string dataOutputFile;
    protected string csvCellSeparator = ";";
    protected string csvLineSeparator = "\n";
    private static string logPrefix = "<b> Logger </b> | ";



    private RandomVariablesManager.TaskCategory taskCategory;
    private float offsetValue;
    private bool currentLiveRedirection;
    private float currentRedirectedWalkingIntensity;
    private float currentRedirectionSliderValue;

    private int TaskNumber;
    private Vector3 initialRealTargetPostion;
    private Vector3 initialVirtualTargetPostion;
    private Vector3 initialHostVirtualPosition;
    private Vector3 initialOtherVirtualPosition;
    private Vector3 initialHostVirtualRotation;
    private Vector3 initialOtherVirtualRotation;
    private Vector3 initialHostRealPosition;
    private Vector3 initialOtherRealPosition;
    private Vector3 initalHostRealRotation;
    private Vector3 initialOtherRealRotation;
    private Vector3 initialHostRealHandPosition;
    private Vector3 initialOtherRealHandPosition;
    private Vector3 initialHostVirtualHandPosition;
    private Vector3 initialOtherVirtualHandPosition;
    private float initialRealHeadDistance;
    private float initialVirtualHeadDistance;
    private float initialRealHandDistance;
    private float initialVirtualHandDistance;
    private string initialTime;

    private Vector3 midRealTargetPosition;
    private Vector3 midVirtualTargetPosition;
    private Vector3 midHostVirtualPosition;
    private Vector3 midOtherVirtualPosition;
    private Vector3 midHostVirtualRotation;
    private Vector3 midOtherVirtualRotation;
    private Vector3 midHostRealPosition;
    private Vector3 midOtherRealPosition;
    private Vector3 midHostRealRotation;
    private Vector3 midOtherRealRotation;
    private Vector3 midHostRealHandPosition;
    private Vector3 midOtherRealHandPosition;
    private Vector3 midHostVirtualHandPosition;
    private Vector3 midOtherVirtualHandPosition;
    private float midRealHeadDistance;
    private float midVirtualHeadDistance;
    private float midRealHandDistance;
    private float midVirtualHandDistance;
    private string midTime;

    private Vector3 finalRealTargetPosition;
    private Vector3 finalVirtualTargetPosition;
    private Vector3 finalHostVirtualPosition;
    private Vector3 finalOtherVirtualPosition;
    private Vector3 finalHostVirtualRotation;
    private Vector3 finalOtherVirtualRotation;
    private Vector3 finalHostRealPosition;
    private Vector3 finalOtherRealPosition;
    private Vector3 finalHostRealRotation;
    private Vector3 finalOtherRealRotation;
    private Vector3 finalHostRealHandPosition;
    private Vector3 finalOtherRealHandPosition;
    private Vector3 finalHostVirtualHandPosition;
    private Vector3 finalOtherVirtualHandPosition;
    private float finalRealHeadDistance;
    private float finalVirtualHeadDistance;
    private float finalRealHandDistance;
    private float finalVirtualHandDistance;
    private string finalTime;
    private float startTime;
    private float totalTime;






    public void SetupNewParticipant(string studyName, string participantID, int studyID)
    {
        string currentFolder = Application.streamingAssetsPath + "/" + studyName + "/" + participantID + "/";
        Directory.CreateDirectory(currentFolder);
        dataOutputFile = currentFolder + participantID + "_" + studyID + "_data.csv";

        int i = 2;
        while (true)
        {
            if (File.Exists(dataOutputFile))
            {
                Debug.LogError(logPrefix + "DataOutputFile already exists at '" + dataOutputFile + "'. A new file will be added at '" + dataOutputFile + i + "'.");
                dataOutputFile = currentFolder + participantID + "_" + studyID + "_data" + i + ".csv";
                i++;
            }
            else
            {
                File.WriteAllText(dataOutputFile,
                    "TrialNumber" + csvCellSeparator +
                    "TotalTime" + csvCellSeparator +
                    "TaskCategory" + csvCellSeparator +
                    "OffsetValue" + csvCellSeparator +
                    "LiveRedirection" + csvCellSeparator +
                    "RedirectedWalkingIntensity" + csvCellSeparator +
                    "RedirectionSliderValue" + csvCellSeparator +
                    "InitialRealHeadDistance" + csvCellSeparator +
                    "InitialVirtualHeadDistance" + csvCellSeparator +
                    "InitialRealHandDistance" + csvCellSeparator +
                    "InitialVirtualHandDistance" + csvCellSeparator +
                    "InitialTime" + csvCellSeparator +
                    "MidRealHeadDistance" + csvCellSeparator +
                    "MidVirtualHeadDistance" + csvCellSeparator +
                    "MidRealHandDistance" + csvCellSeparator +
                    "MidVirtualHandDistance" + csvCellSeparator +
                    "MidTime" + csvCellSeparator +
                    "FinalRealHeadDistance" + csvCellSeparator +
                    "FinalVirtualHeadDistance" + csvCellSeparator +
                    "FinalRealHandDistance" + csvCellSeparator +
                    "FinalVirtualHandDistance" + csvCellSeparator +
                    "FinalTime" + csvCellSeparator +
                    csvLineSeparator);

                break;
            }
        }
    }


    public void WriteAllStudyData(int trialNumber)
    {
        string line = 
            $"{trialNumber}{csvCellSeparator}" +
            $"{totalTime}{csvCellSeparator}" +
            $"{taskCategory}{csvCellSeparator}" +
            $"{offsetValue}{csvCellSeparator}" +
            $"{currentLiveRedirection}{csvCellSeparator}" +
            $"{currentRedirectedWalkingIntensity}{csvCellSeparator}" +
            $"{currentRedirectionSliderValue}{csvCellSeparator}" +
            $"{initialRealHeadDistance}{csvCellSeparator}" +
            $"{initialVirtualHeadDistance}{csvCellSeparator}" +
            $"{initialRealHandDistance}{csvCellSeparator}" +
            $"{initialVirtualHandDistance}{csvCellSeparator}" +
            $"{initialTime}{csvCellSeparator}" +
            $"{midRealHeadDistance}{csvCellSeparator}" +
            $"{midVirtualHeadDistance}{csvCellSeparator}" +
            $"{midRealHandDistance}{csvCellSeparator}" +
            $"{midVirtualHandDistance}{csvCellSeparator}" +
            $"{midTime}{csvCellSeparator}" +
            $"{finalRealHeadDistance}{csvCellSeparator}" +
            $"{finalVirtualHeadDistance}{csvCellSeparator}" +
            $"{finalRealHandDistance}{csvCellSeparator}" +
            $"{finalVirtualHandDistance}{csvCellSeparator}" +
            $"{finalTime}{csvCellSeparator}";

        File.AppendAllText(this.dataOutputFile, line + this.csvLineSeparator);
    }

    private string Vector3ToString(Vector3 vector)
    {
        return $"{vector.x},{vector.y},{vector.z}";
    }
    public void SaveInitialValues()
    {   
        startTime = Time.time;

        initialRealTargetPostion = ConfigurationScript.Instance.redirectedRealTarget.transform.position;
        initialVirtualTargetPostion = ConfigurationScript.Instance.redirectedVirtualObject.transform.position;
        initialHostVirtualPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").position;

        initialHostVirtualRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").rotation.eulerAngles;

        initialHostRealHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Right Hand/Sphere").position;
        initialHostVirtualHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand/Sphere").position;

        initialHostRealPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").position;

        initalHostRealRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").rotation.eulerAngles;

        initialTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        taskCategory = StudyProgressionController.instance.taskCategory;
        offsetValue = StudyProgressionController.instance.offsetValue;
        currentLiveRedirection = StudyProgressionController.instance.currentLiveRedirection;
        currentRedirectedWalkingIntensity = StudyProgressionController.instance.currentRedirectedWalkingIntensity;
        currentRedirectionSliderValue = StudyProgressionController.instance.currentRedirectionSliderValue;
        TaskNumber = StudyProgressionController.instance.TaskNumber;
        initialOtherVirtualPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").position;
        initialOtherVirtualRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").rotation.eulerAngles;
        initialOtherRealHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Right Hand/Sphere").position;
        initialOtherVirtualHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Right Hand/Sphere").position;
        initialOtherRealPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").position;
        initialOtherRealRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").rotation.eulerAngles;
        initialRealHeadDistance = new Vector3(initialHostRealPosition.x - initialOtherRealPosition.x, 0f, initialHostRealPosition.z - initialOtherRealPosition.z).magnitude;
        Debug.Log("Real Horizontal Distance: " + initialRealHeadDistance);
        // Calculate horizontal distance in virtual world
        initialVirtualHeadDistance = new Vector3(initialHostVirtualPosition.x - initialOtherVirtualPosition.x, 0f, initialHostVirtualPosition.z - initialOtherVirtualPosition.z).magnitude;
        Debug.Log("Virtual Horizontal Distance: " + initialVirtualHeadDistance);
        initialRealHandDistance = new Vector3(initialHostRealHandPosition.x - initialOtherRealHandPosition.x, 0f, initialHostRealHandPosition.z - initialOtherRealHandPosition.z).magnitude;
        Debug.Log("Real Hand Horizontal Distance: " + initialRealHandDistance);
        initialVirtualHandDistance = new Vector3(initialHostVirtualHandPosition.x - initialOtherVirtualHandPosition.x, 0f, initialHostVirtualHandPosition.z - initialOtherVirtualHandPosition.z).magnitude;
        Debug.Log("Virtual Hand Horizontal Distance: " + initialVirtualHandDistance);

    }
    public void SaveMidValues()
    {
        midRealTargetPosition = ConfigurationScript.Instance.redirectedRealTarget.transform.position;
        midVirtualTargetPosition = ConfigurationScript.Instance.redirectedVirtualObject.transform.position;
        midHostVirtualPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").position;

        midHostVirtualRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").rotation.eulerAngles;

        midHostRealHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Right Hand/Sphere").position;
        midHostVirtualHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand/Sphere").position;

        midHostRealPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").position;

        midHostRealRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").rotation.eulerAngles;

        midTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //midOtherVirtualPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").position;
        //midOtherVirtualRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").rotation.eulerAngles;
        //midOtherRealHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Right Hand/Sphere").position;
        //midOtherVirtualHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Right Hand/Sphere").position;
        //midOtherRealPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").position;
        //midOtherRealRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").rotation.eulerAngles;
        midRealHeadDistance = new Vector3(midHostRealPosition.x - midOtherRealPosition.x, 0f, midHostRealPosition.z - midOtherRealPosition.z).magnitude;
        //Debug.Log("Real Horizontal Distance: " + midRealHeadDistance);
        midVirtualHeadDistance = new Vector3(midHostVirtualPosition.x - midOtherVirtualPosition.x, 0f, midHostVirtualPosition.z - midOtherVirtualPosition.z).magnitude;
        //Debug.Log("Virtual Horizontal Distance: " + midVirtualHeadDistance);
        midRealHandDistance = new Vector3(midHostRealHandPosition.x - midOtherRealHandPosition.x, 0f, midHostRealHandPosition.z - midOtherRealHandPosition.z).magnitude;
        //Debug.Log("Real Hand Horizontal Distance: " + midRealHandDistance);
        midVirtualHandDistance = new Vector3(midHostVirtualHandPosition.x - midOtherVirtualHandPosition.x, 0f, midHostVirtualHandPosition.z - midOtherVirtualHandPosition.z).magnitude;
        //Debug.Log("Virtual Hand Horizontal Distance: " + midVirtualHandDistance);

    }
    public void SaveFinalValues()
    {
        finalRealTargetPosition = ConfigurationScript.Instance.redirectedRealTarget.transform.position;
        finalVirtualTargetPosition = ConfigurationScript.Instance.redirectedVirtualObject.transform.position;
        finalHostVirtualPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").position;

        finalHostVirtualRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").rotation.eulerAngles;

        finalHostRealHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Right Hand/Sphere").position;
        finalHostVirtualHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand/Sphere").position;

        finalHostRealPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").position;

        finalHostRealRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").rotation.eulerAngles;

        finalTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //finalOtherVirtualPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").position;
        //finalOtherVirtualRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").rotation.eulerAngles;
        //finalOtherRealHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Right Hand/Sphere").position;
        //finalOtherVirtualHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Right Hand/Sphere").position;
        //finalOtherRealPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").position;
        //finalOtherRealRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").rotation.eulerAngles;
        finalRealHeadDistance = new Vector3(finalHostRealPosition.x - finalOtherRealPosition.x, 0f, finalHostRealPosition.z - finalOtherRealPosition.z).magnitude;
        //Debug.Log("Real Horizontal Distance: " + finalRealHeadDistance);
        finalVirtualHeadDistance = new Vector3(finalHostVirtualPosition.x - finalOtherVirtualPosition.x, 0f, finalHostVirtualPosition.z - finalOtherVirtualPosition.z).magnitude;
        //Debug.Log("Virtual Horizontal Distance: " + finalVirtualHeadDistance);
        finalRealHandDistance = new Vector3(finalHostRealHandPosition.x - finalOtherRealHandPosition.x, 0f, finalHostRealHandPosition.z - finalOtherRealHandPosition.z).magnitude;
        //Debug.Log("Real Hand Horizontal Distance: " + finalRealHandDistance);
        finalVirtualHandDistance = new Vector3(finalHostVirtualHandPosition.x - finalOtherVirtualHandPosition.x, 0f, finalHostVirtualHandPosition.z - finalOtherVirtualHandPosition.z).magnitude;
        //Debug.Log("Virtual Hand Horizontal Distance: " + finalVirtualHandDistance);
        totalTime = Time.time - startTime;




    }

}
