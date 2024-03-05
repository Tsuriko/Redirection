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

        [Range(0f, 1f)]
        public float redirectIntensity = 1f;

        public bool liveRedirection = false;

        private CustomRDWTake3 rdwManager;
        private RedirectionManager handRedirectionManager;
        public KeyCode triggerKey = KeyCode.R;

        private float initialDistanceToTarget;
        private bool hasSwitchedToHandRedirection = false;
        public bool isRedirectionEnabled = false; // Flag to control redirection activation

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
                    if (StudyProgressionController.instance != null) StudyProgressionController.instance.SaveMidValues();
                    hasSwitchedToHandRedirection = true;
                }
            }
        }

        [PunRPC] // Mark as an RPC method
        void EnableRedirection(float sharedSliderValue, float sharedRedirectIntensity, bool sharedLiveRedirection)
        {
            sliderValue = sharedSliderValue;
            redirectIntensity = sharedRedirectIntensity;
            liveRedirection = sharedLiveRedirection;

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
            if (rdwManager != null) {
                rdwManager.redirectIntensity = redirectIntensity;
                rdwManager.enabled = true;
            }
           
        }

        void ActivateHandRedirection()
        {
            if (rdwManager != null) rdwManager.alignmentAchieved = true;
            ConfigurationScript.Instance.attachMethod = (liveRedirection) ? ConfigurationScript.AttachMethod.otherHand : ConfigurationScript.AttachMethod.midpoint;
            if (handRedirectionManager != null)
            {
                handRedirectionManager.enabled = true;
                handRedirectionManager.TriggerHandRedirection();
            }
        }
        public void StartRedirectionExternally()
        {
            if (!isRedirectionEnabled)
            {
                float currentSliderValue = sliderValue; 
                photonView.RPC("EnableRedirection", RpcTarget.AllBuffered, sliderValue, redirectIntensity, liveRedirection);
            }
        }
        public void EndHandRedirection()
        {
            if (handRedirectionManager != null)
            {
                handRedirectionManager.TriggerHandRedirection();
            }
        }
        public void EndRedirectedWalking()
        {
            if (rdwManager != null)
            {
                rdwManager.enabled = false;
            }
        }
        public void resetRedirection()
        {
            if (rdwManager != null)
            {
                rdwManager.enabled = false;
                rdwManager.InitializeRedirection();
            }
            if (handRedirectionManager != null)
            {
                EndHandRedirection();
            }
            hasSwitchedToHandRedirection = false;
            isRedirectionEnabled = false;
        }
    }

}