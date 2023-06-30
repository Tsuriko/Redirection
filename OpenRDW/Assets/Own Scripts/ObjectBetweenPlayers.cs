using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBetweenPlayers : MonoBehaviour
{
    public Transform otherHand;
    public Transform objectToMovePrefab;

    private Transform ownHand;
    private Transform objectToMove;

    private void Start()
    {
        ownHand = transform; // Assuming the object you want to move is attached to the script's GameObject
        objectToMove = Instantiate(objectToMovePrefab, transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (otherHand == null || ownHand == null)
        {
            Debug.LogWarning("Player objects are not assigned.");
            return;
        }

        // Calculate the midpoint position between the two players
        Vector3 midpoint = (otherHand.position + ownHand.position) * 0.5f;

        // Set the position of the objectToMove to the calculated midpoint position
        objectToMove.position = midpoint;
    }
}
