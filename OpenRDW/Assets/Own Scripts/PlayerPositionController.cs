using UnityEngine;
using Photon.Pun;

public class PlayerPositionController : MonoBehaviourPun
{
    public KeyCode triggerKey = KeyCode.G; // Replace with your desired button

    public GameObject head;
    public GameObject ownPlayer;
    public bool isMaster;

    void Start()
    {
        // Find the head GameObjects of both players during initialization
        head = GameObject.Find("VR Player (Host)/Virtual/Head");
        ownPlayer = GameObject.Find("OwnPlayer");

        if (head == null)
        {
            Debug.LogError("Head GameObjects not found. Make sure they have the correct names.");
        }
        else
        {
            Debug.Log("Head GameObjects successfully found.");
        }
    }

    public void ActivatePlayerPositioning()
    {
        isMaster = PhotonNetwork.IsMasterClient;
        MovePlayer();

    }
    [PunRPC]
    private void MovePlayer(){
        

        if (head != null)
        {
            // Apply only the rotation for both players
            MoveOwnPlayerLocallyOnlyRotation(CalculateRotation());

            // Recalculate position based on the new rotation

            // Apply the recalculated position
            Vector3 newPosition = CalculatePosition();
            MoveOwnPlayerLocally(newPosition);
        }
    }

private Quaternion CalculateRotation()
{
    // Desired direction facing along the X-axis, based on whether they're master or not.
    Vector3 desiredDirection = isMaster ? Vector3.left : Vector3.right;

    // Calculate the current forward direction of the head in world space
    Vector3 currentHeadForward = head.transform.forward;
    // Project the current forward and desired direction onto the horizontal plane (y = 0)
    currentHeadForward.y = 0;
    desiredDirection.y = 0;

    // Calculate the rotation needed to align the head's forward direction with the desired direction
    Quaternion fromCurrentToDesired = Quaternion.FromToRotation(currentHeadForward, desiredDirection);

    // Extract the y component of the rotation
    float yRotation = fromCurrentToDesired.eulerAngles.y;

    // Create a new Quaternion for the parent that only includes the y-component rotation
    Quaternion parentTargetRotation = Quaternion.Euler(0, yRotation, 0) * ownPlayer.transform.rotation;

    return parentTargetRotation;
}

    private Vector3 CalculatePosition()
    {

        // Calculate horizontal distance between the virtual representations of the master and other player
        GameObject hostPlayerVirtual = GameObject.Find("VR Player (Host)/Real/Head");
        GameObject otherPlayerVirtual = GameObject.Find("VR Player (Guest)/Real/Head");
        float horizontalDistanceBetweenPlayers = (hostPlayerVirtual.transform.position - otherPlayerVirtual.transform.position).magnitude;

        // Position players on opposite sides of the origin (0, 0, 0) based on the horizontal distance
        float halfDistance = horizontalDistanceBetweenPlayers / 2;
        Vector3 newHeadPosition = isMaster ? new Vector3(halfDistance, 0, 0) : new Vector3(-halfDistance, 0, 0);
        Debug.Log(newHeadPosition);

        // Calculate the offset from the OwnPlayer to the head
        Vector3 offsetToHead = head.transform.position - ownPlayer.transform.position;

        // Calculate and return the new position for the OwnPlayer, adjusting for the offset
        return newHeadPosition - offsetToHead;
    }
    private void MoveOwnPlayerLocally(Vector3 newPosition)
    {

        if (ownPlayer != null)
        {
            Debug.Log("Moving own player locally.");
            newPosition.y = 0;
            ownPlayer.transform.position = newPosition;
            Debug.Log(newPosition);
        }
        else
        {
            Debug.LogError("OwnPlayer GameObject not found.");
        }
    }
private void MoveOwnPlayerLocallyOnlyRotation(Quaternion newRotation)
{
    if (ownPlayer != null && head != null)
    {
        Debug.Log("Rotating own player around the head locally.");

        // Calculate the rotation difference
        Quaternion rotationDifference = newRotation * Quaternion.Inverse(ownPlayer.transform.rotation);

        // Get the angle and axis from the rotation difference
        rotationDifference.ToAngleAxis(out float angle, out Vector3 axis);

        // Check if the rotation is significant
        if (angle > 0.01f || angle < -0.01f) // A small threshold to avoid floating point imprecision issues
        {
            // Apply the rotation around the head position
            ownPlayer.transform.RotateAround(head.transform.position, axis, angle);
        }
    }
}
}