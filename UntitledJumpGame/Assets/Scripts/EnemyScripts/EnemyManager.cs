using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager _instance;

    //variables used for calculating screen area and spawning in enemies
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
        //set this to the only instance of this object
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        //get the camera
        _camera = Camera.main;
        //get a random distance for which the platforms must move before an enemy spawns
        spawnEnemyAt = platformSpawner.transform.position.y - GetRandomSpawnDistance();


        //instantiate enemies
        for (int i = 0; i < numOfEachPooledEnemies; i ++)
        {
            EnemyBaseScript basic = Instantiate(basicEnemy, platformSpawner.transform);
            basic.SetTarget(target);
            basic.gameObject.SetActive(false);
            deadBasicEnemies.AddLast(basic);

            EnemyBaseScript tank = Instantiate(tankEnemy, platformSpawner.transform);
            tank.SetTarget(target);
            tank.gameObject.SetActive(false);
            deadTankEnemies.AddLast(tank);
        }

        //get screen space positions
        float distance = Vector3.Distance(platformSpawner.transform.position, _camera!.transform.position);
        Vector3 viewBottomLeft = _camera.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 viewTopRight = _camera.ViewportToWorldPoint(new Vector3(1, 1, distance));

        //get game view width and height
        gameWidth = (viewTopRight.x - viewBottomLeft.x);
        gameHeight = viewTopRight.y - viewBottomLeft.y;
    }

    private void FixedUpdate()
    {
        //check if the platforms have moved far enough to spawn an enemy
        if (spawnEnemyAt > platformSpawner.transform.position.y)
        {
            //spawn an enemy
            SpawnSelector();
            //minus 1 from the max distance to slowly reduce the time between enemies to increas difficulty
            if (maxMoveDistance > minMoveDistance)
            {
                maxMoveDistance--;
            }
            //set a new spawn distance value
            spawnEnemyAt = platformSpawner.transform.position.y - GetRandomSpawnDistance();
        }
    }

    /// <summary>
    /// checks if the enemy is a basic enemy or a tank enemy and removes them from the respective alive linked list
    /// </summary>
    /// <param name="enemy"></param>
    public void DeactivateEnemy(EnemyBaseScript enemy)
    {
        //check if each list contains the enemy
        //remove that enemy and add it to the respective dead list
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

    /// <summary>
    /// Selects a spawn type for each enemy
    /// </summary>
    public void SpawnSelector()
    {
        //get a random x position within the screen and a y position above the screen
        Vector3 position = new Vector3(GetRandomXInScreen(), gameHeight, 0);

        //Select a random spawn type for an enemy
        int selection = Random.Range(0, 4);
        switch(selection){
            //blank case 0 to increase default enemy spawn rate
            case 0:
            case 1:
                //spawn a basic enemy in a random spot with random movement positions
                SpawnBasic(position);
                break;
            case 2:
                //spawn a tank enemy in a random spot with random movement postisions
                SpawnTank(position);
                break;
            case 3:
                //spawn 2 tank enemies one of half of the screen
                SpawnDoubleTank();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// spawns a basic enemy at the input position
    /// </summary>
    /// <param name="pos">vector 3 position to spawn an enemy at</param>
    public void SpawnBasic(Vector3 pos)
    {
        EnemyBaseScript enemy = null;
        //check if the dead list contains atleast 1 enemy to use
        if (deadBasicEnemies.Count != 0)
        {
            //get the first enemy in the dead list
            enemy = deadBasicEnemies.First.Value;
            //remove the enemy and move it to the alive list
            deadBasicEnemies.Remove(enemy);
            aliveBasicEnemies.AddLast(enemy);
            //set the left and right positions in the side movement to random positions
            enemy.SetMovementEdges(GetRandomXInScreen(), GetRandomXInScreen());
            //reActivate the enemy at the spawn point
            enemy.Spawn(pos);
        }
    }

    /// <summary>
    /// spawns a tank enemy at the input position
    /// </summary>
    /// <param name="pos">vector 3 position to spawn an enemy at</param>
    public void SpawnTank(Vector3 pos)
    {
        EnemyBaseScript enemy = null;
        //check the dead list has atleast 1 enemy to use
        if (deadTankEnemies.Count != 0)
        {
            //get the first enemy in the dead list
            enemy = deadTankEnemies.First.Value;
            //move the enemy from the dead to alive list
            deadTankEnemies.Remove(enemy);
            aliveTankEnemies.AddLast(enemy);
            //set the movement positions
            enemy.SetMovementEdges(GetRandomXInScreen(), GetRandomXInScreen());
            //reset the movement speed as the double tank will change this to 0
            enemy.SetMovementSpeed();
            //ReActivate the enemy
            enemy.Spawn(pos);
            
        }
    }

    /// <summary>
    /// Spawn 2 tanks, one in each half of the screen
    /// </summary>
    public void SpawnDoubleTank()
    {
        SideSpawner(true);
        SideSpawner(false);
    }

    /// <summary>
    /// use a boolean to determine if the tank enemy spawns on the left side(true) or the right side (false)
    /// </summary>
    /// <param name="left">bollean to determine if the oenemy will spawn on the left or right of the screen</param>
    public void SideSpawner(bool left)
    {
        //set a side to positive or negative 1
        float side = 1;
        if (left)
        {
            side *= -1;
        }

        EnemyBaseScript enemy = null;
        //get a spawn position above the screen in the middel of the left or right half of the screen
        Vector3 pos = new Vector3(0 + gameWidth * side * 0.25f, gameHeight, 0);
        //check if the dead enemies have atleaast one enemy
        if (deadTankEnemies.Count > 0)
        {
            //get the first enemy
            enemy = deadTankEnemies.First.Value;
            //swap the list the enemy is within
            deadTankEnemies.Remove(enemy);
            aliveTankEnemies.AddLast(enemy);
            //spawn the enemy at the position
            enemy.Spawn(pos);
            //set the movement speed to 0 to stop the enemy from moving left and right
            enemy.SetMovementSpeed(0);
        }
    }

    /// <summary>
    /// returns a random x value within the screen space
    /// </summary>
    /// <returns>a random float within the screen space on the x axis</returns>
    public float GetRandomXInScreen()
    {
        return Random.Range(-gameWidth * 0.5f + enemyWidth, gameWidth * 0.5f - enemyWidth); // replace with screen edges
    }

    /// <summary>
    /// gets a random value the platforms must move before an enemy is spawned
    /// </summary>
    /// <returns>a random distance the platforms will move</returns>
    private float GetRandomSpawnDistance()
    {
        return Random.Range(minMoveDistance, maxMoveDistance); ; // replace with screen edges
    }

    /// <summary>
    /// returns the current height of the game screen
    /// </summary>
    /// <returns>float of the current height of the gaem screen</returns>
    public float GetGameHeight()
    {
        return gameHeight;
    }
}
