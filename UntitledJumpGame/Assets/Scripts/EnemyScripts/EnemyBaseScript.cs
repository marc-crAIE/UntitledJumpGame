using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    [SerializeField] private EnemyCombat combatController;
    [SerializeField] private EnemyMovement movementController;

    void Start()
    {
        if (combatController == null)
        {
            combatController = GetComponent<EnemyCombat>();
        }


        if (movementController == null)
        {
            movementController = GetComponent<EnemyMovement>();
        }    
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -EnemyManager._instance.GetGameHeight() * 0.5)
        {
            combatController.StopAllCoroutines();
            gameObject.SetActive(false);
            Die();
        }
    }

    public void Spawn(Vector3 spawnPosition)
    {
        movementController.Spawn(spawnPosition);
        combatController.ResetCombat();
    }

    public void SetMovementEdges(float leftEdge, float rightEdge)
    {
        movementController.SetEdges(leftEdge, rightEdge);
    }

    public void SetMovementSpeed()
    {
        movementController.ResetMovementSpeed();
    }

    public void SetMovementSpeed(float speed)
    {
        movementController.SetMovementSpeed(speed);
    }

    public void SetTarget(GameObject target)
    {
        combatController.SetTarget(target);
    }

    public void Die()
    {
        EnemyManager._instance.DeactivateEnemy(this);
    }
}
