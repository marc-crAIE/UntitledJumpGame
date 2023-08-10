using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private int health = 1;


    [SerializeField] private GameObject target;
    //[SerializeField] private BulletManager bulletManager;
    [SerializeField] private GameObject parent;


    //used to randomise a time between shooting from the enemy so it is not always the same
    [SerializeField] private int minTimeBetweenShots;
    [SerializeField] private int maxTimeBetweenShots;

    private Coroutine waitingCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        waitingCoroutine = StartCoroutine(TimeBetweenShots());
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            Die();
        }
    }

    protected void ShootAtTarget()
    {
        //bulletManager.ShootAt(this.transform.position, target.transform.position, bulletSpeed);
        Debug.Log("Shooting Now");
        waitingCoroutine = StartCoroutine(TimeBetweenShots());
    }

    public void TakeDamage()
    {
        health -= 1;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Die()
    {
        StopAllCoroutines();

        //Any animations for death include here
        parent.SetActive(false);
    }

    IEnumerator TimeBetweenShots()
    {
        int timeToWait = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        Debug.Log("Waiting for " + timeToWait + " Seconds");
        yield return new WaitForSeconds(timeToWait);
        ShootAtTarget();
    }
}
