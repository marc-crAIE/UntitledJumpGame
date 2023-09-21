using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy Combat controls the enemies combat abilities and information
/// </summary>
public class EnemyCombat : MonoBehaviour
{
    //health and bullet speed
    [SerializeField] private int health = 1;
    private int defaultHealth = 1;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private AudioSource soundMaker;


    //target to shoot at
    private GameObject target;


    [SerializeField] private GameObject parent;
    [SerializeField] private EnemyBaseScript parentScript;


    //used to randomise a time between shooting from the enemy so it is not always the same
    [SerializeField] private int minTimeBetweenShots;
    [SerializeField] private int maxTimeBetweenShots;

    //coroutine holder
    private Coroutine waitingCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        StopAllCoroutines();
        //start waiting
        waitingCoroutine = StartCoroutine(TimeBetweenShots());
        defaultHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        //check if health is below or equal to 0
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Shoot a bullet at the target game object
    /// </summary>
    protected void ShootAtTarget()
    {
        //spawn in a bullet from the pool
        EnemyBulletManager._instance.SpawnBullet(this.transform, target.transform, bulletSpeed);

        soundMaker.PlayOneShot(parentScript.GetShootingSound());
        //start a new waiting coroutine
        waitingCoroutine = StartCoroutine(TimeBetweenShots());
    }

    /// <summary>
    /// minus 1 health from this enemy
    /// </summary>
    public void TakeDamage()
    {
        health -= 1;
    }

    /// <summary>
    /// minus an input amount of health from the enemy
    /// </summary>
    /// <param name="damage">amount of health to minus</param>
    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    /// <summary>
    /// kills this enemy
    /// </summary>
    public void Die()
    {
        StopAllCoroutines();

        //Any animations for death include here
        parent.SetActive(false);

        //Updates pooling of this gameObject
        parentScript.Die();
    }

    /// <summary>
    /// coroutine which times between each shot
    /// </summary>
    /// <returns></returns>
    IEnumerator TimeBetweenShots()
    {
        //generate a random time
        int timeToWait = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);

        //wait for the amount of time
        yield return new WaitForSecondsRealtime(timeToWait);
        //shoot at the target
        ShootAtTarget();
    }

    /// <summary>
    /// Resets this enemy for respawning
    /// </summary>
    public void ResetCombat()
    {
        health = defaultHealth;
        if (waitingCoroutine != null)
        {
            StopCoroutine(waitingCoroutine);
        }
        waitingCoroutine = StartCoroutine(TimeBetweenShots());
    }

    /// <summary>
    /// sets the target this enemy will shoot at
    /// </summary>
    /// <param name="targetObject">gameobject of the target to shoot at</param>
    public void SetTarget(GameObject targetObject)
    {
        target = targetObject;
    }
}
