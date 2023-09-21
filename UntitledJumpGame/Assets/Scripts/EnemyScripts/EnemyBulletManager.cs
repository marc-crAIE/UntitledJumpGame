using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and manages bullets in a pooling system
/// </summary>
public class EnemyBulletManager : MonoBehaviour
{
    //create a universal instance to be accessed by other scripts
    public static EnemyBulletManager _instance;

    //prefab used to create new objects
    [SerializeField] EnemyBullet bulletPrefab;

    //pooling linked lists
    private LinkedList<EnemyBullet> inactiveBullets = new LinkedList<EnemyBullet>();
    private LinkedList<EnemyBullet> activeBullets = new LinkedList<EnemyBullet>();

    //maximum number of bullets to spawn in
    public const int MAX_BULLETS = 10;
    // Start is called before the first frame update
    void Start()
    {
        //check if another instance is active
        if (_instance != null && _instance != this)
        {
            //destroy this object if another instance is active
            Destroy(this);
        }
        //fi no instances are active set this as the active instance
        else if (_instance == null)
        {
            _instance = this;
        }
        //create bullets for pooling
        for (int i = 0; i < MAX_BULLETS; i++)
        {
            CreateNewBullet();
        }
    }

    /// <summary>
    /// spawns a new bullet at the desired location with the desired direction and speed
    /// </summary>
    /// <param name="startPosition">start position which the bullet will start</param>
    /// <param name="targetPosition">target which will shoot the bullet towards</param>
    /// <param name="speed">speed at which the bullet will move</param>
    public void SpawnBullet(Transform startPosition, Transform targetPosition, float speed)
    {
        //check if the inactive bullets is not empty
        if (inactiveBullets.First != null)
        {
            //spawn the first bullet in the inactive list
            EnemyBullet bullet = inactiveBullets.First.Value;
            inactiveBullets.Remove(bullet);
            activeBullets.AddLast(bullet);
            bullet.transform.SetParent(startPosition.parent.parent);
            bullet.Spawn(startPosition, targetPosition, speed);
        }
        //if the inactive list is empty
        else
        {
            //reset the first bullet in the active list
            EnemyBullet bullet = activeBullets.First.Value;
            activeBullets.Remove(bullet);
            activeBullets.AddLast(bullet);
            bullet.transform.SetParent(startPosition.parent.parent);
            bullet.Spawn(startPosition, targetPosition, speed);
        }
    }

    /// <summary>
    /// moves bullets between the active list to inactive list
    /// </summary>
    /// <param name="bullet">bullet to deactive</param>
    public void DeSpawnBullet(EnemyBullet bullet)
    {
        activeBullets.Remove(bullet);
        inactiveBullets.AddLast(bullet);
    }

    /// <summary>
    /// instantiates a new bullet to be used in the booling.
    /// </summary>
    private void CreateNewBullet()
    {
        EnemyBullet bullet = Instantiate(bulletPrefab, transform);
        inactiveBullets.AddFirst(bullet);
    }
}
