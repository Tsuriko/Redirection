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




    public float offsetValue;
    public int personRedirected;
    public float currentOffsetMaster;
    public float currentOffsetOther;
    public bool currentLiveRedirection;
    public float currentRedirectedWalkingIntensity;
    public int currentRandomTask;
    public Vector3 initialRealTargetPostion;
    public Vector3 initialVirtualTargetPostion;
    public Vector3 initialHostVirtualPosition;
    public Vector3 initialOtherVirtualPosition;
    public Vector3 initialHostVirtualRotation;
    public Vector3 initialOtherVirtualRotation;
    public Vector3 initalHostRealPosition;
    public Vector3 initalOtherRealPosition;
    public Vector3 initalHostRealRotation;
    public Vector3 initalOtherRealRotation;
    public Vector3 initalHostRealHandPosition;
    public Vector3 initalOtherRealHandPosition;
    public Vector3 initalHostVirtualHandPosition;
    public Vector3 initalOtherVirtualHandPosition;
    public string initialTime;

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
    private string finalTime;






    public void SetupNewParticipant(string studyName, int participantNumber)
    {
        string currentFolder = Application.streamingAssetsPath + "/" + studyName + "/" + participantNumber + "/";
        Directory.CreateDirectory(currentFolder);
        dataOutputFile = currentFolder + participantNumber + "_data.csv";

        int i = 2;
        while (true)
        {
            if (File.Exists(dataOutputFile))
            {
                Debug.LogError(logPrefix + "DataOutputFile already exists at '" + dataOutputFile + "'. A new file will be added at '" + dataOutputFile + i + "'.");
                dataOutputFile = currentFolder + participantNumber + "_data" + i + ".csv";
                i++;
            }
            else
            {
                File.WriteAllText(dataOutputFile,
                    "TrialNumber" + csvCellSeparator +
                    "OffsetValue" + csvCellSeparator +
                    "PersonRedirected" + csvCellSeparator +
                    "CurrentOffsetMaster" + csvCellSeparator +
                    "CurrentOffsetOther" + csvCellSeparator +
                    "CurrentLiveRedirection" + csvCellSeparator +
                    "CurrentRedirectedWalkingIntensity" + csvCellSeparator +
                    "CurrentRandomTask" + csvCellSeparator +
                    "InitialRealTargetPosition" + csvCellSeparator +
                    "InitialVirtualTargetPosition" + csvCellSeparator +
                    "InitialHostVirtualPosition" + csvCellSeparator +
                    "InitialOtherVirtualPosition" + csvCellSeparator +
                    "InitialHostVirtualRotation" + csvCellSeparator +
                    "InitialOtherVirtualRotation" + csvCellSeparator +
                    "InitialHostRealPosition" + csvCellSeparator +
                    "InitialOtherRealPosition" + csvCellSeparator +
                    "InitialHostRealRotation" + csvCellSeparator +
                    "InitialOtherRealRotation" + csvCellSeparator +
                    "InitialHostRealHandPosition" + csvCellSeparator +
                    "InitialOtherRealHandPosition" + csvCellSeparator +
                    "InitialHostVirtualHandPosition" + csvCellSeparator +
                    "InitialOtherVirtualHandPosition" + csvCellSeparator +
                    "InitialTime" + csvCellSeparator +
                    "MidRealTargetPosition" + csvCellSeparator +
                    "MidVirtualTargetPosition" + csvCellSeparator +
                    "MidHostVirtualPosition" + csvCellSeparator +
                    "MidOtherVirtualPosition" + csvCellSeparator +
                    "MidHostVirtualRotation" + csvCellSeparator +
                    "MidOtherVirtualRotation" + csvCellSeparator +
                    "MidHostRealPosition" + csvCellSeparator +
                    "MidOtherRealPosition" + csvCellSeparator +
                    "MidHostRealRotation" + csvCellSeparator +
                    "MidOtherRealRotation" + csvCellSeparator +
                    "MidHostRealHandPosition" + csvCellSeparator +
                    "MidOtherRealHandPosition" + csvCellSeparator +
                    "MidHostVirtualHandPosition" + csvCellSeparator +
                    "MidOtherVirtualHandPosition" + csvCellSeparator +
                    "MidTime" + csvCellSeparator +
                    "FinalRealTargetPosition" + csvCellSeparator +
                    "FinalVirtualTargetPosition" + csvCellSeparator +
                    "FinalHostVirtualPosition" + csvCellSeparator +
                    "FinalOtherVirtualPosition" + csvCellSeparator +
                    "FinalHostVirtualRotation" + csvCellSeparator +
                    "FinalOtherVirtualRotation" + csvCellSeparator +
                    "FinalHostRealPosition" + csvCellSeparator +
                    "FinalOtherRealPosition" + csvCellSeparator +
                    "FinalHostRealRotation" + csvCellSeparator +
                    "FinalOtherRealRotation" + csvCellSeparator +
                    "FinalHostRealHandPosition" + csvCellSeparator +
                    "FinalOtherRealHandPosition" + csvCellSeparator +
                    "FinalHostVirtualHandPosition" + csvCellSeparator +
                    "FinalOtherVirtualHandPosition" + csvCellSeparator +
                    "FinalTime");
            }
        }
    }


    public void WriteAllStudyData(int trialNumber)
    {
        string line = $"{trialNumber}{csvCellSeparator}" +
                      $"{this.offsetValue}{csvCellSeparator}" +
                      $"{this.personRedirected}{csvCellSeparator}" +
                      $"{this.currentOffsetMaster}{csvCellSeparator}" +
                      $"{this.currentOffsetOther}{csvCellSeparator}" +
                      $"{this.currentLiveRedirection}{csvCellSeparator}" +
                      $"{this.currentRedirectedWalkingIntensity}{csvCellSeparator}" +
                      $"{this.currentRandomTask}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialRealTargetPostion)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialVirtualTargetPostion)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialHostVirtualPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialOtherVirtualPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialHostVirtualRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialOtherVirtualRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalHostRealPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalOtherRealPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalHostRealRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalOtherRealRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalHostRealHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalOtherRealHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalHostVirtualHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalOtherVirtualHandPosition)}{csvCellSeparator}" +
                      $"{this.initialTime}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midRealTargetPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midVirtualTargetPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midHostVirtualPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midOtherVirtualPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midHostVirtualRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midOtherVirtualRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midHostRealPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midOtherRealPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midHostRealRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midOtherRealRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midHostRealHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midOtherRealHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midHostVirtualHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.midOtherVirtualHandPosition)}{csvCellSeparator}" +
                      $"{this.midTime}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalRealTargetPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalVirtualTargetPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalHostVirtualPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalOtherVirtualPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalHostVirtualRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalOtherVirtualRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalHostRealPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalOtherRealPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalHostRealRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalOtherRealRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalHostRealHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalOtherRealHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalHostVirtualHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.finalOtherVirtualHandPosition)}{csvCellSeparator}" +
                      $"{this.finalTime}";

        File.AppendAllText(this.dataOutputFile, line + this.csvLineSeparator);
    }

    private string Vector3ToString(Vector3 vector)
    {
        return $"{vector.x},{vector.y},{vector.z}";
    }
    public void SaveInitialValues()
    {

        initialRealTargetPostion = ConfigurationScript.Instance.redirectedRealTarget.transform.position;
        initialVirtualTargetPostion = ConfigurationScript.Instance.redirectedVirtualObject.transform.position;
        initialHostVirtualPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").position;
        //initialOtherVirtualPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").position;
        initialHostVirtualRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").rotation.eulerAngles;
        //initialOtherVirtualRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").rotation.eulerAngles;
        initalHostRealHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Right Hand").position;
        initalHostVirtualHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand").position;
        //initalOtherRealHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Right Hand").position;
        //initalOtherVirtualHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Right Hand").position;
        initalHostRealPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").position;
        //initalOtherRealPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").position;
        initalHostRealRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").rotation.eulerAngles;
        //initalOtherRealRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").rotation.eulerAngles;
        initialTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        offsetValue = StudyProgressionController.instance.offsetValue;
        personRedirected = StudyProgressionController.instance.personRedirected;
        currentOffsetMaster = StudyProgressionController.instance.currentOffsetMaster;
        currentOffsetOther = StudyProgressionController.instance.currentOffsetOther;
        currentLiveRedirection = StudyProgressionController.instance.currentLiveRedirection;
        currentRedirectedWalkingIntensity = StudyProgressionController.instance.currentRedirectedWalkingIntensity;
        currentRandomTask = StudyProgressionController.instance.currentRandomTask;

    }
    public void SaveMidValues()
    {
        midRealTargetPosition = ConfigurationScript.Instance.redirectedRealTarget.transform.position;
        midVirtualTargetPosition = ConfigurationScript.Instance.redirectedVirtualObject.transform.position;
        midHostVirtualPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").position;
        //midOtherVirtualPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").position;
        midHostVirtualRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").rotation.eulerAngles;
        //midOtherVirtualRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").rotation.eulerAngles;
        midHostRealHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Right Hand").position;
        midHostVirtualHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand").position;
        //midOtherRealHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Right Hand").position;
        //midOtherVirtualHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Right Hand").position;
        midHostRealPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").position;
        //midOtherRealPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").position;
        midHostRealRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").rotation.eulerAngles;
        //midOtherRealRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").rotation.eulerAngles;
        midTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
    public void SaveFinalValues()
    {
        finalRealTargetPosition = ConfigurationScript.Instance.redirectedRealTarget.transform.position;
        finalVirtualTargetPosition = ConfigurationScript.Instance.redirectedVirtualObject.transform.position;
        finalHostVirtualPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").position;
        //finalOtherVirtualPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").position;
        finalHostVirtualRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").rotation.eulerAngles;
        //finalOtherVirtualRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").rotation.eulerAngles;
        finalHostRealHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Right Hand").position;
        finalHostVirtualHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand").position;
        //finalOtherRealHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Right Hand").position;
        //finalOtherVirtualHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Right Hand").position;
        finalHostRealPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").position;
        //finalOtherRealPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").position;
        finalHostRealRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").rotation.eulerAngles;
        //finalOtherRealRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").rotation.eulerAngles;
        finalTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

    }

}
