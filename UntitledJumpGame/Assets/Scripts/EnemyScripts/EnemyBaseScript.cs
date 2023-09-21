using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds references to the combat controller and movement controller of the enemy
/// with functions to access and modify both to reduce/remove the need to use the getComponent function
/// </summary>
public class EnemyBaseScript : MonoBehaviour
{
    //hold the movement and the combat scripts of this enemy
    [SerializeField] private EnemyCombat combatController;
    [SerializeField] private EnemyMovement movementController;
    [SerializeField] private ParticleSystem particles;

    [SerializeField] protected AudioClip bop1;
    [SerializeField] protected AudioClip bop2;

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
        if (particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }

        var main = particles.main;
        main.customSimulationSpace = this.transform.parent;
    }

    private void FixedUpdate()
    {
        //check if this enemy has moved below the edge of the screen
        //normally times by 0.5 but it will disappear when half the enemy is off the screen so giving another 0.05 vertical screen space to ensure it is off the screen first
        if (transform.position.y < -EnemyManager._instance.GetGameHeight() * 0.6)
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

    public AudioClip GetShootingSound()
    {
        int choice = Random.Range(0, 2);

        switch(choice)
        {
            case 0:
                return bop1;
            case 1:
            default:
                return bop2;
        }
    }
}
