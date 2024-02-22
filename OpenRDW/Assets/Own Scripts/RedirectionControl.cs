using UnityEngine;
using Photon.Pun;

namespace HR_Toolkit
{
    public class RedirectionControl : MonoBehaviourPun 
    {
        public Transform user;
        public Transform target;

        [Range(0f, 1f)]
        public float sliderValue = 0.5f;

        private CustomRDWTake3 rdwManager;
        private RedirectionManager handRedirectionManager;
        public KeyCode triggerKey = KeyCode.R;

        private float initialDistanceToTarget;
        private bool hasSwitchedToHandRedirection = false;
        private bool isRedirectionEnabled = false; // Flag to control redirection activation

        void Awake()
        {
            // Automatically find the RDW and Hand Redirection scripts in the scene
            rdwManager = FindObjectOfType<CustomRDWTake3>();

            handRedirectionManager = FindObjectOfType<RedirectionManager>();

            if (rdwManager == null || handRedirectionManager == null)
            {
                Debug.LogError("RedirectionControl: Required components not found in the scene.");
            }
        }

        void Update()
        {
            /*
            if (Input.GetKeyDown(triggerKey) && PhotonNetwork.IsMasterClient) // Only allow master client to trigger
            {
                        float currentSliderValue = sliderValue;
                        photonView.RPC("EnableRedirection", RpcTarget.AllBuffered, currentSliderValue); 
            }*/

            if (isRedirectionEnabled)
            {
                float currentDistanceToTarget = CalculateHorizontalDistance(user.position, target.position);
                float switchDistance = initialDistanceToTarget * sliderValue;


                if (!hasSwitchedToHandRedirection && currentDistanceToTarget <= switchDistance)
                {
                    Debug.Log("Switch to Hand");
                    ActivateHandRedirection();
                    hasSwitchedToHandRedirection = true;
                }
            }
        }

        [PunRPC] // Mark as an RPC method
        void EnableRedirection(float sharedSliderValue)
        {
            sliderValue = sharedSliderValue; // Update the local slider value based on the RPC call

            if (!isRedirectionEnabled)
            {
                rdwManager.realObject = target;
                initialDistanceToTarget = CalculateHorizontalDistance(user.position, target.position);
                ActivateRDW();
                isRedirectionEnabled = true;
            }
        }

        float CalculateHorizontalDistance(Vector3 pointA, Vector3 pointB)
        {
            pointA.y = 0;
            pointB.y = 0;
            return Vector3.Distance(pointA, pointB);
        }

        void ActivateRDW()
        {
            if (rdwManager != null) rdwManager.enabled = true;
           
        }

        void ActivateHandRedirection()
        {
            if (rdwManager != null) rdwManager.alignmentAchieved = true;
            if (handRedirectionManager != null)
            {
                handRedirectionManager.enabled = true;
                // If additional synchronization is required for hand redirection, consider using RPCs as needed
                handRedirectionManager.TriggerHandRedirection();
            }
        }
        public void StartRedirectionExternally()
        {
            if (!isRedirectionEnabled)
            {
                float currentSliderValue = sliderValue; 
                photonView.RPC("EnableRedirection", RpcTarget.AllBuffered, currentSliderValue); 
            }
        }
    }

}