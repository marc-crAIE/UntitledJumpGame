using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager _instance;

    [SerializeField] PlatformSpawnerScript platformSpawner;
    public float minMoveDistance = 5;
    public float maxMoveDistance = 30;
    private float spawnEnemyAt = 0;

    #region enemyTypes
    [SerializeField] private EnemyBaseScript basicEnemy;
    [SerializeField] private EnemyBaseScript tankEnemy;
#endregion


    //number of each enemy to spawn
    [SerializeField] private int numOfEachPooledEnemies = 4;
    //target to shoot at (should be set to the player)
    [SerializeField] GameObject target;

    //basic enemy pooling
    private LinkedList<EnemyBaseScript> deadBasicEnemies = new LinkedList<EnemyBaseScript>();
    private LinkedList<EnemyBaseScript> aliveBasicEnemies = new LinkedList<EnemyBaseScript>();

    //tank enemy pooling
    private LinkedList<EnemyBaseScript> deadTankEnemies = new LinkedList<EnemyBaseScript>();
    private LinkedList<EnemyBaseScript> aliveTankEnemies = new LinkedList<EnemyBaseScript>();


    //Screen Area
    private Camera _camera;
    private float gameWidth;
    private float gameHeight;

    //enemySideBuffer so that they do not spawn or move off screen
    [SerializeField] private float enemyWidth;

    private void Start()
    {
        _camera = Camera.main;
        spawnEnemyAt = platformSpawner.transform.position.y - GetRandomSpawnDistance();
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
            EnemyBaseScript basic = Instantiate(basicEnemy, platformSpawner.transform);
            basic.SetTarget(target);
            deadBasicEnemies.AddLast(basic);

            EnemyBaseScript tank = Instantiate(tankEnemy, platformSpawner.transform);
            tank.SetTarget(target);
            deadTankEnemies.AddLast(tank);
        }
        //Update to be based on screen movement not time
        //StartCoroutine(SpawnEnemy());

        //get screen space positions
        float distance = Vector3.Distance(platformSpawner.transform.position, _camera!.transform.position);
        Vector3 viewBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, distance));

        gameWidth = (viewTopRight.x - viewBottomLeft.x);
        gameHeight = viewTopRight.y - viewBottomLeft.y;
    }

    private void FixedUpdate()
    {
        if (spawnEnemyAt > platformSpawner.transform.position.y)
        {
            SpawnSelector();
            if (maxMoveDistance > minMoveDistance)
            {
                maxMoveDistance--;
            }
            spawnEnemyAt = platformSpawner.transform.position.y - GetRandomSpawnDistance();
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

    public void SpawnSelector()
    {
        Vector3 position = new Vector3(GetRandomXInScreen(), gameHeight, 0);

        int selection = Random.Range(0, 3);
        switch(selection){
            case 0:
            case 1:
                SpawnBasic(position);
                break;
            case 2:
                SpawnTank(position);
                break;
            case 3:
                SpawnDoubleTank();
                break;
            default:
                break;
        }
    }


    public void SpawnBasic(Vector3 pos)
    {
        EnemyBaseScript enemy = null;
        if (deadBasicEnemies.Count != 0)
        {
            enemy = deadBasicEnemies.First.Value;
            deadBasicEnemies.Remove(enemy);
            aliveBasicEnemies.AddLast(enemy);
            enemy.SetMovementEdges(GetRandomXInScreen(), GetRandomXInScreen());
            enemy.Spawn(pos);
            
        }
    }

    public void SpawnTank(Vector3 pos)
    {
        EnemyBaseScript enemy = null;
        if (deadTankEnemies.Count != 0)
        {
            enemy = deadTankEnemies.First.Value;
            deadTankEnemies.Remove(enemy);
            aliveTankEnemies.AddLast(enemy);
            enemy.SetMovementEdges(GetRandomXInScreen(), GetRandomXInScreen());
            enemy.Spawn(pos);
            enemy.SetMovementSpeed();
        }
    }

    public void SpawnDoubleTank()
    {
        SideSpawner(true);
        SideSpawner(false);
    }


    public void SideSpawner(bool left)
    {
        float side = 1;
        if (left)
        {
            side *= -1;
        }

        EnemyBaseScript enemy = null;
        Vector3 pos = new Vector3(0 + gameWidth * side * 0.25f, gameHeight, 0);
        if (deadTankEnemies.Count > 0)
        {
            enemy = deadTankEnemies.First.Value;
            deadTankEnemies.Remove(enemy);
            aliveTankEnemies.AddLast(enemy);
            enemy.SetMovementEdges(GetRandomXInScreen(), GetRandomXInScreen());
            enemy.Spawn(pos);
            enemy.SetMovementSpeed(0);
        }
    }

    public float GetRandomXInScreen()
    {
        return Random.Range(-gameWidth * 0.5f + enemyWidth, gameWidth * 0.5f - enemyWidth); // replace with screen edges
    }

    private float GetRandomSpawnDistance()
    {
        return Random.Range(minMoveDistance, maxMoveDistance); ; // replace with screen edges
    }

    public float GetGameHeight()
    {
        return gameHeight;
    }
}
