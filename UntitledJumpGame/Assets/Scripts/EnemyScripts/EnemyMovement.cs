using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //wobble is used to add character to enemies, increases how much they wobble/jitter
    [SerializeField] private float wiggleDistance = 0.2f;
    [SerializeField] private float wiggleFrequency = 5.0f;
    private Vector3 targetPosition = Vector3.zero;
    [SerializeField] private GameObject enemyMesh;
    //journey is used for when lerping to a new position
    private float wiggleJourney = 0.0f;
    private Vector3 wiggleStartPosition;

    //movement speed determines how fast the enemy will move from the left to the right(and vice versa) of the screen
    [SerializeField] private float movementSpeed = 0.4f;
    [SerializeField] private float leftEdge = -4;
    [SerializeField] private float rightEdge = 4;
    private float targetX;
    private float startingX;
    private float movementLerpJourney = 0.0f;

    void FixedUpdate()
    {
        WiggleLerp();
        MoveToLerp();
    }

    void GetWigglePosition()
    {
        float xOffset = Random.Range(0, wiggleDistance);
        float yOffset = Random.Range(0, wiggleDistance);

        targetPosition = new Vector3(xOffset, yOffset, enemyMesh.transform.localPosition.z);
    }

    void WiggleLerp()
    {
        if (enemyMesh.transform.localPosition != targetPosition)
        {
            wiggleJourney += wiggleFrequency * Time.deltaTime;
            enemyMesh.transform.localPosition = Vector3.Lerp(wiggleStartPosition, targetPosition, wiggleJourney);
        }
        else if (wiggleDistance != 0.0f || wiggleFrequency != 0.0f)
        {
            GetWigglePosition();
            wiggleJourney = 0.0f;
            wiggleStartPosition = enemyMesh.transform.localPosition;
        }
    }


    void MoveToLerp()
    {
        if (this.transform.position.x != targetX)
        {
            movementLerpJourney += movementSpeed * Time.deltaTime;
            this.transform.position = Vector3.Lerp(new Vector3(startingX, this.transform.position.y, this.transform.position.z), new Vector3(targetX, this.transform.position.y, this.transform.position.z), movementLerpJourney);
        }
        else if (movementSpeed > 0.0f)
        {
            GetXPosition();
            movementLerpJourney = 0.0f;
            startingX = this.transform.position.x;
        }
    }
    
    void GetXPosition()
    {
        if (targetX == rightEdge)
        {
            targetX = leftEdge;
        }
        else
        {
            targetX = rightEdge;
        }
    }
}
