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




    private float offsetValue;
    private int personRedirected;
    private float currentOffsetMaster;
    private float currentOffsetOther;
    private bool currentLiveRedirection;
    private float currentRedirectedWalkingIntensity;
    private int currentRandomTask;
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






    public void SetupNewParticipant(string studyName, int studyNumber)
    {
        string currentFolder = Application.streamingAssetsPath + "/" + studyName + "/" + studyNumber + "/";
        Directory.CreateDirectory(currentFolder);
        dataOutputFile = currentFolder + studyNumber + "_data.csv";

        int i = 2;
        while (true)
        {
            if (File.Exists(dataOutputFile))
            {
                Debug.LogError(logPrefix + "DataOutputFile already exists at '" + dataOutputFile + "'. A new file will be added at '" + dataOutputFile + i + "'.");
                dataOutputFile = currentFolder + studyNumber + "_data" + i + ".csv";
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
                    "FinalTime"+ this.csvLineSeparator);
                    break;
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
                      $"{Vector3ToString(this.initialHostRealPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialOtherRealPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initalHostRealRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialOtherRealRotation)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialHostRealHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialOtherRealHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialHostVirtualHandPosition)}{csvCellSeparator}" +
                      $"{Vector3ToString(this.initialOtherVirtualHandPosition)}{csvCellSeparator}" +
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

                      Debug.Log(line);
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
        
        initialHostVirtualRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Head").rotation.eulerAngles;
        
        initialHostRealHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Right Hand/Sphere").position;
        initialHostVirtualHandPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Virtual/Right Hand/Sphere").position;
        
        initialHostRealPosition = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").position;
        
        initalHostRealRotation = ConfigurationScript.Instance.vrPlayerHost.transform.Find("Real/Head").rotation.eulerAngles;
        
        initialTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Debug.Log(StudyProgressionController.instance.offsetValue);
        offsetValue = StudyProgressionController.instance.offsetValue;
        personRedirected = StudyProgressionController.instance.personRedirected;
        currentOffsetMaster = StudyProgressionController.instance.currentOffsetMaster;
        currentOffsetOther = StudyProgressionController.instance.currentOffsetOther;
        currentLiveRedirection = StudyProgressionController.instance.currentLiveRedirection;
        currentRedirectedWalkingIntensity = StudyProgressionController.instance.currentRedirectedWalkingIntensity;
        currentRandomTask = StudyProgressionController.instance.currentRandomTask;
        initialOtherVirtualPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").position;
        initialOtherVirtualRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Head").rotation.eulerAngles;
        initialOtherRealHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Right Hand/Sphere").position;
        initialOtherVirtualHandPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Virtual/Right Hand/Sphere").position;
        initialOtherRealPosition = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").position;
        initialOtherRealRotation = ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Head").rotation.eulerAngles;
        Debug.Log("Real Distance:" + (initialHostRealPosition - initialOtherRealPosition).magnitude);
        Debug.Log("Virtual Distance:" + (initialHostVirtualPosition - initialOtherVirtualPosition).magnitude);
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

    }

}
