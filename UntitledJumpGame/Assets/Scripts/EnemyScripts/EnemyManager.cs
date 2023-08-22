using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager _instance;

#region enemyTypes
    [SerializeField] private EnemyBaseScript basicEnemy;
    [SerializeField] private EnemyBaseScript tankEnemy;
#endregion


    [SerializeField] private int numOfEachPooledEnemies = 3;
    [SerializeField] GameObject target;


    private LinkedList<EnemyBaseScript> deadBasicEnemies = new LinkedList<EnemyBaseScript>();
    private LinkedList<EnemyBaseScript> aliveBasicEnemies = new LinkedList<EnemyBaseScript>();


    private LinkedList<EnemyBaseScript> deadTankEnemies = new LinkedList<EnemyBaseScript>();
    private LinkedList<EnemyBaseScript> aliveTankEnemies = new LinkedList<EnemyBaseScript>();

    private void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        for (int i = 0; i < numOfEachPooledEnemies; i ++)
        {
            EnemyBaseScript basic = Instantiate(basicEnemy);
            basic.SetTarget(target);
            deadBasicEnemies.AddLast(basic);

            EnemyBaseScript tank = Instantiate(tankEnemy);
            tank.SetTarget(target);
            deadTankEnemies.AddLast(tank);
        }
    }


    public void DeactivateEnemy(EnemyBaseScript enemy)
    {
        if (aliveBasicEnemies.Contains(enemy))
        {
            aliveBasicEnemies.Remove(enemy);
            deadBasicEnemies.AddLast(enemy);
        }
        else if (aliveTankEnemies.Contains(enemy))
        {
            aliveTankEnemies.Remove(enemy);
            deadTankEnemies.AddLast(enemy);
        }
    }

    public void SpawnRandom()
    {

    }

    public void SpawnBasic()
    {

    }

    public void SpawnTank()
    {

    }
}
