using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Bullet class used to update this bullet in game, with the possibility to set a new position and target when spawning
/// </summary>
public class EnemyBullet : MonoBehaviour
{
    float speed = 0;
    [SerializeField] private ParticleSystem particles;

    void Start()
    {
        if (particles == null)
        {
            particles = GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        //move forward at the set speed
        this.gameObject.transform.Translate(this.gameObject.transform.forward * (speed * Time.deltaTime), Space.World);

        //similar to the enemy times by 0.6 so that the bullet has time to elave the screen before deactivating
        if (transform.position.y < -EnemyManager._instance.GetGameHeight() * 0.6)
        {
            Die();
        }
    }

    /// <summary>
    /// On trigger enter catch
    /// </summary>
    /// <param name="other">object the collision trigger is activated with</param>
    private void OnTriggerEnter(Collider other)
    {
        //check if the object doesn't have the enemy tag
        if (!other.gameObject.CompareTag("Enemy"))
        {
            //destroy this object
            Die();
        }

        //check if the object has the player tag
        if (other.gameObject.CompareTag("Player"))
        {
            //Hurt the player
            //GameManager._instance.player.Die();
            Debug.Log("Player Dies!");
        }
    }

    /// <summary>
    /// moves the bullet to the set location, looks at the target then sets the movement speed
    /// </summary>
    /// <param name="startPosition">position to move to</param>
    /// <param name="target">target to look at</param>
    /// <param name="speed"></param>
    public void Spawn(Transform startPosition, Transform target, float speed)
    {
        //set the position of this bullet
        this.gameObject.transform.position = new Vector3(startPosition.position.x, startPosition.position.y, startPosition.position.z);
        //set the speed of this bullet
        this.speed = speed;
        //look at the target
        this.gameObject.transform.LookAt(target.position);
        //set this bullet to active
        this.gameObject.SetActive(true);
        //set the parent of the particles to the moving world
        var main = particles.main;
        main.customSimulationSpace = this.transform.parent;
        //clear all partricles to remove a line of particles that spawns when the bullet becomes active
        particles.Clear(true);
    }

    /// <summary>
    /// disables this bullet and calls the function in bullet manager to move it to the inactive list
    /// </summary>
    private void Die()
    {
        this.gameObject.SetActive(false);
        EnemyBulletManager._instance.DeSpawnBullet(this);
    }
}
