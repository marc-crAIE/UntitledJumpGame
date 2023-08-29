using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    //hold the movement and the combat scripts of this enemy
    [SerializeField] private EnemyCombat combatController;
    [SerializeField] private EnemyMovement movementController;

    void Start()
    {
        //if the movement or combat controllers are enempty find them on this object
        if (combatController == null)
        {
            //combat controller is on the child of this object
            combatController = GetComponentInChildren<EnemyCombat>();
        }
        if (movementController == null)
        {
            movementController = GetComponent<EnemyMovement>();
        }    
    }

    private void FixedUpdate()
    {
        //check fi this enemy has moved below the edge of the screen
        if (transform.position.y < -EnemyManager._instance.GetGameHeight() * 0.5)
        {
            //if the enemy is below the screen kill this enemy
            combatController.StopAllCoroutines();
            gameObject.SetActive(false);
            Die();
        }
    }

    /// <summary>
    /// Spawns this enemy at the input vector 3 position
    /// </summary>
    /// <param name="spawnPosition">Vector 3 containing the position to spawn this enemy</param>
    public void Spawn(Vector3 spawnPosition)
    {
        movementController.Spawn(spawnPosition);
        combatController.ResetCombat();
    }

    /// <summary>
    /// Takes 2 floats and sets them to the x values this enemy will move between
    /// </summary>
    /// <param name="leftEdge">left point the enemy will move towards</param>
    /// <param name="rightEdge">right point the enemy will move towards</param>
    public void SetMovementEdges(float leftEdge, float rightEdge)
    {
        movementController.SetEdges(leftEdge, rightEdge);
    }

    /// <summary>
    /// Sets this enemies movement speed to the defualt movement speed
    /// </summary>
    public void SetMovementSpeed()
    {
        movementController.ResetMovementSpeed();
    }

    /// <summary>
    /// Sets this enemies movement speed to the input speed
    /// </summary>
    /// <param name="speed">float containing the new speed of the enemy</param>
    public void SetMovementSpeed(float speed)
    {
        movementController.SetMovementSpeed(speed);
    }

    /// <summary>
    /// updates the target this enemy will shoot at
    /// </summary>
    /// <param name="target">GameObject of the target to shoot at</param>
    public void SetTarget(GameObject target)
    {
        combatController.SetTarget(target);
    }

    /// <summary>
    /// deactivates this enemy
    /// </summary>
    public void Die()
    {
        EnemyManager._instance.DeactivateEnemy(this);
    }
}
