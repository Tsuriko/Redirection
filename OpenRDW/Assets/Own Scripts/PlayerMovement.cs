using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public GameObject playerObject; // The GameObject you want to move and rotate
    public float moveDistance = 1.0f; // Distance moved per key press
    public float rotationAngle = 5.0f; // Rotation angle per key press

    private void Start()
    {
        // Subscribe to the events
        ConfigurationScript.Instance.OnWKeyPressed += MoveForward;
        ConfigurationScript.Instance.OnAKeyPressed += MoveLeft;
        ConfigurationScript.Instance.OnSKeyPressed += MoveBackward;
        ConfigurationScript.Instance.OnDKeyPressed += MoveRight;
        ConfigurationScript.Instance.OnLeftArrowPressed += RotateLeft;
        ConfigurationScript.Instance.OnRightArrowPressed += RotateRight;
    }

    private void OnDestroy()
    {
        // Unsubscribe from the events
        ConfigurationScript.Instance.OnWKeyPressed -= MoveForward;
        ConfigurationScript.Instance.OnAKeyPressed -= MoveLeft;
        ConfigurationScript.Instance.OnSKeyPressed -= MoveBackward;
        ConfigurationScript.Instance.OnDKeyPressed -= MoveRight;
        ConfigurationScript.Instance.OnLeftArrowPressed -= RotateLeft;
        ConfigurationScript.Instance.OnRightArrowPressed -= RotateRight;
    }

    void MoveForward()
    {
        playerObject.transform.Translate(Vector3.forward * moveDistance, Space.World);
    }

    void MoveBackward()
    {
        playerObject.transform.Translate(Vector3.back * moveDistance, Space.World);
    }

    void MoveLeft()
    {
        playerObject.transform.Translate(Vector3.left * moveDistance, Space.World);
    }

    void MoveRight()
    {
        playerObject.transform.Translate(Vector3.right * moveDistance, Space.World);
    }

    void RotateLeft()
    {
        playerObject.transform.Rotate(Vector3.up, -rotationAngle);
    }

    void RotateRight()
    {
        playerObject.transform.Rotate(Vector3.up, rotationAngle);
    }
}
