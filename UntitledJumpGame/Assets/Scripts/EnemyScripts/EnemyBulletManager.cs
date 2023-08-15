using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletManager : MonoBehaviour
{
    public static EnemyBulletManager _instance;

    [SerializeField] EnemyBullet bulletPrefab;

    private LinkedList<EnemyBullet> inactiveBullets = new LinkedList<EnemyBullet>();
    private LinkedList<EnemyBullet> activeBullets = new LinkedList<EnemyBullet>();

    public int maxBullets = 10;
    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else if (_instance == null)
        {
            _instance = this;
        }
        for (int i = 0; i < maxBullets; i++)
        {
            CreateNewBullet();
        }
    }

    public void SpawnBullet(Transform startPosition, Transform targetPosition, float speed)
    {
        if (inactiveBullets.First != null)
        {
            EnemyBullet bullet = inactiveBullets.First.Value;
            inactiveBullets.Remove(bullet);
            activeBullets.AddLast(bullet);
            bullet.spawn(startPosition, targetPosition, speed);
        }
        else
        {
            EnemyBullet bullet = activeBullets.First.Value;
            activeBullets.Remove(bullet);
            activeBullets.AddLast(bullet);
            bullet.spawn(startPosition, targetPosition, speed);
        }
    }

    public void DeSpawnBullet(EnemyBullet bullet)
    {
        activeBullets.Remove(bullet);
        inactiveBullets.AddLast(bullet);
    }

    private void CreateNewBullet()
    {
        EnemyBullet bullet = Instantiate(bulletPrefab);
        inactiveBullets.AddFirst(bullet);
        bullet.transform.parent = this.transform;
    }
}
