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
    [SerializeField]private float defaultMovementSpeed = 0.4f;
    [SerializeField] private float leftEdge = -4;
    [SerializeField] private float rightEdge = 4;


    //target x position when moving side to side
    private float targetX;
    //starting x point to allow smoothing lerp
    private float startingX;
    //percentage of journey complete
    private float movementLerpJourney = 0.0f;

    //animation curve to smooth out lerping
    public AnimationCurve lerpAcceleration;


    void Awake()
    {    
        defaultMovementSpeed = movementSpeed;
    }

    /// <summary>
    /// move this enemy to the spawn location then reActivate it
    /// </summary>
    /// <param name="spawnPosition"></param>
    public void Spawn(Vector3 spawnPosition)
    {
        this.transform.position = spawnPosition;
        ResetLerps();
        this.gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        //update wiggling
        WiggleLerp();
        //update moving to position
        MoveToLerp();
    }

    /// <summary>
    /// Reset the lerping values to start when the enemy spawns
    /// </summary>
    private void ResetLerps()
    {
        GetWigglePosition();
        wiggleJourney = 0.0f;
        wiggleStartPosition = enemyMesh.transform.localPosition;

        GetXPosition();
        movementLerpJourney = 0.0f;
        startingX = this.transform.position.x;
    }

    /// <summary>
    /// get a random position to "wiggle" to
    /// </summary>
    void GetWigglePosition()
    {
        //get random x position
        float xOffset = Random.Range(0, wiggleDistance);
        //get random y position
        float yOffset = Random.Range(0, wiggleDistance);

        //create a vector 3 with the nex x and y positions
        targetPosition = new Vector3(xOffset, yOffset, enemyMesh.transform.localPosition.z);
    }

    /// <summary>
    /// lerp to the wiggle position
    /// </summary>
    void WiggleLerp()
    {
        //check if current position does not equal the target position
        if (enemyMesh.transform.localPosition != targetPosition)
        {
            //update position with lerp
            wiggleJourney += wiggleFrequency * Time.deltaTime;
            enemyMesh.transform.localPosition = Vector3.Lerp(wiggleStartPosition, targetPosition, wiggleJourney);
        }
        //if the wiggle position equals the target position and the wiggle settings are enabled
        else if (wiggleDistance != 0.0f || wiggleFrequency != 0.0f)
        {
            //get a new position to wigglle to
            GetWigglePosition();

            //reset the lerping data
            wiggleJourney = 0.0f;
            wiggleStartPosition = enemyMesh.transform.localPosition;
        }
    }

    /// <summary>
    /// update the enemy position based on the lerping position
    /// </summary>
    void MoveToLerp()
    {
        //check if this position doesnt equal the target position
        if (this.transform.position.x != targetX)
        {
            //lerp to the new position
            movementLerpJourney += movementSpeed * Time.deltaTime;
            float animatedAmout = lerpAcceleration.Evaluate(movementLerpJourney);
            this.transform.position = Vector3.Lerp(new Vector3(startingX, this.transform.position.y, this.transform.position.z), new Vector3(targetX, this.transform.position.y, this.transform.position.z), animatedAmout);
        }
        //if movement speed is positive and greater than 0
        else if (movementSpeed > 0.0f)
        {
            //get new position to lerp to
            GetXPosition();
            //reset lerp journey
            movementLerpJourney = 0.0f;
            startingX = this.transform.position.x;
        }
    }
    
    /// <summary>
    /// get the left and right position to move to
    /// </summary>
    void GetXPosition()
    {
        //if the right edge is the current target change to the left edge
        if (targetX == rightEdge)
        {
            targetX = leftEdge;
        }
        //else target the right edge
        else
        {
            targetX = rightEdge;
        }
    }

    /// <summary>
    /// set the left and right edges of the screen this enemy will move between
    /// </summary>
    /// <param name="leftX">x value this enemy will move toward</param>
    /// <param name="rightX">second x value this enemy will move towards</param>
    public void SetEdges(float leftX, float rightX)
    {
        leftEdge = leftX;
        rightEdge = rightX;
    }

    /// <summary>
    /// set the movement speed to the input speed
    /// </summary>
    /// <param name="newSpeed">float of the new speed to set this enemy to</param>
    public void SetMovementSpeed(float newSpeed)
    {
        movementSpeed = newSpeed;
    }

    /// <summary>
    /// if the parameters are empty reset this movement speed to the default speed (speed set at the start of the game)
    /// </summary>
    public void ResetMovementSpeed()
    {
        movementSpeed = defaultMovementSpeed;
    }
}
