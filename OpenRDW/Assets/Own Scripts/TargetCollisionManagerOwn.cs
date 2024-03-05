using HR_Toolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;


public class TargetCollisionManagerOwn : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("instance.isTaskReview()");
        if (!StudyProgressionController.instance.isTaskReview()) return;
        Debug.Log("instance.isTaskReview()");
        
        // check whether this is the current target
        if (collision.gameObject == ConfigurationScript.Instance.vrPlayerGuest.transform.Find("Real/Right Hand").gameObject)
        {
            Debug.Log("Target reached");
            StudyProgressionController.instance.TriggerNextActionViaEndOfRedirection();
        }
    }
}
