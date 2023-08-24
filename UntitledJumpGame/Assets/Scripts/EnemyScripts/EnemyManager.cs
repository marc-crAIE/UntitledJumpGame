using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager _instance;

    [SerializeField] private Transform worldObject;

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
            EnemyBaseScript basic = Instantiate(basicEnemy, worldObject);
            basic.SetTarget(target);
            deadBasicEnemies.AddLast(basic);

            EnemyBaseScript tank = Instantiate(tankEnemy, worldObject);
            tank.SetTarget(target);
            deadTankEnemies.AddLast(tank);
        }
        StartCoroutine(SpawnEnemy());
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

    public void SpawnSelector()
    {
        Vector3 position = new Vector3(Random.Range(-3, 3), 10, 0);
        float leftx = Random.Range(-5, 5); // replace with screen edges
        float rightx = Random.Range(-5, 5); //replace with screen edges


        int selection = Random.Range(0, 2);
        switch(selection){
            case 0:
                SpawnBasic(position, leftx, rightx);
                break;
            default:
                SpawnTank(position, leftx, rightx);
                break;
        }
    }






    public void SpawnBasic(Vector3 pos, float left, float right)
    {
        EnemyBaseScript enemy = null;
        if (deadBasicEnemies.Count != 0)
        {
            enemy = deadBasicEnemies.First.Value;
            deadBasicEnemies.Remove(enemy);
            aliveBasicEnemies.AddLast(enemy);
            enemy.SetMovementEdges(left, right);
            enemy.Spawn(pos);
            
        }
    }

    public void SpawnTank(Vector3 pos, float left, float right)
    {
        EnemyBaseScript enemy = null;
        if (deadTankEnemies.Count != 0)
        {
            enemy = deadTankEnemies.Last.Value;
            deadTankEnemies.Remove(enemy);
            aliveTankEnemies.AddLast(enemy);
            enemy.SetMovementEdges(left, right);
            enemy.Spawn(pos);
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(2);
        SpawnSelector();
        StartCoroutine(SpawnEnemy());
    }
}
