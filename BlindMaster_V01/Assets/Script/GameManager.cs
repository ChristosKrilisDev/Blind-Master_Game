using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [Header("Enemy Prefabs")]
    public Enemy[] enemies;
    public Vector3 enemySpawnPos;

    private List<Enemy> activeEnemies = new List<Enemy>();
    private List<Enemy> pooledEnemies = new List<Enemy>();
    private int firstPoolIndex = 1;

    [Header("Level Up Members")]
    private int maxEnemiesAmount = 1;
    private int currentEnemiesAmount = 0;
    private int nextScoreTrigget = 5;

    [Header("Score")]
    public Vector3 pointsPos;
    public GameObject pointDisplayPref;
    public Text scoreTxt;
    private int gameScore = 0;

    [Header("MENU")]
    public GameObject pauseMenu;
    public GameObject theEndPanel;
    public GameObject[] stars;
    public Text finalScoreTxt;
    [HideInInspector] public bool isMenuActive { get; private set; }
    public GameObject pressToStart;
    private bool canSpawn = false;


    //New 
    PlayerController player;

    #region SINGLETON

    public static GameManager instance;

    #endregion


    private void Awake()
    {
        //Make sure that the time scale is actually running
        Time.timeScale = 1f;

        player = FindObjectOfType<PlayerController>();

        if(instance != null)
        {
            return;
        }
        instance = this;

    }

    void Update()
    {
        if(Input.anyKeyDown && !canSpawn)
        {
            pressToStart.SetActive(false);
            canSpawn = true;
        }

        if(!canSpawn)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            PauseMenuActivity(!pauseMenu.activeSelf);
        }

        EnemyLevelSpawn();

    }


    IEnumerator SpawnEnemies()
    {
        PoolEnemy();

        float randT = Random.Range(0.8f , 2f);
        yield return new WaitForSeconds(randT);
    }

    #region Main

    public void CheckPlayerInput( KeyCode playerKey )
    {
        //Player input
        if(activeEnemies.Count == 0)
        {
            //Debug.Log("Non enemy is active");
            return;
        }


        //Search if there is any active enemy with this keyCode
        //if there is , destroy() the first

        foreach(Enemy actEnemy in activeEnemies)
        {
            //Menu key presssed
            if(playerKey == KeyCode.Space)
                return;

            //If enemy is inside the player view and enemys key is pressed
            if(actEnemy.GetKey() == playerKey && actEnemy.isReadyToDie)
            {
                //Debug.Log("Enemy destroyed and removed from the active list");

                //Destroy
                actEnemy.Kill();

                currentEnemiesAmount--;
                //Remove it from the active list
                activeEnemies.Remove(actEnemy);

                //Add points score etc
                gameScore++;

                scoreTxt.text = gameScore.ToString();

                //Instatiate new display for points
                //May this change in the feature for better control about the point 
                //or fx etc.

                GameObject newPointPref = Instantiate(pointDisplayPref);
                newPointPref.transform.position = pointsPos;


                return;
            }
            else
            {
                //Player hit a wrong key 
                //Debug.Log("Wrong key you fker");

                player.TakeDamage(1);
            }
        }
    }

    public void OnDeath()
    {

        int nStars = ScoreManager.CalculateScore(gameScore , 0);
        //Open score panel
        theEndPanel.SetActive(true);


        finalScoreTxt.text = ScoreManager.finalScore.ToString();

        //Activate Stars One by One
        StartCoroutine(ActivateStars(nStars));


        //Create a local list to destoy all the enemiess
        List<Enemy> enemyToDestroy = new List<Enemy>();

        //Freeze all enemies
        foreach(Enemy actEnemy in activeEnemies)
        {

            actEnemy.speed = 0;
            enemyToDestroy.Add(actEnemy);
        }

        StartCoroutine(CreateDealay(10f));

        foreach(Enemy actEnemy in enemyToDestroy)
        {
            actEnemy.Kill();

        }
        //Stop all creation functions
    }

    IEnumerator CreateDealay( float time )
    {
        Debug.Log("Waiting..");
        yield return new WaitForSeconds(10f);
    }

    IEnumerator ActivateStars( int nStars )
    {
        yield return new WaitForSeconds(0.7f);

        for(int i = 0; i < nStars; i++)
        {
            stars[i].SetActive(true);
            yield return new WaitForSeconds(.3f);
        }
    }

    public void PauseMenuActivity( bool isOpen )
    {

        pauseMenu.gameObject.SetActive(isOpen);
        if(isOpen)
        {
            Time.timeScale = 0f;
            isMenuActive = true;
        } else
        {
            Time.timeScale = 1f;
            isMenuActive = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    #endregion


    #region PooledObject

    //pool pattern

    void EnemyLevelSpawn()
    {
        //If current score less that tr , spawn enemies
        if(gameScore < nextScoreTrigget)
        {
            if(currentEnemiesAmount < maxEnemiesAmount)
            {
                StartCoroutine(SpawnEnemies());
            }
        }
        //Check score if > scoreTrigger , increase stats
        else if(gameScore == nextScoreTrigget)
        {
            //Increase max score trigger + max mount
            nextScoreTrigget += Mathf.RoundToInt(maxEnemiesAmount * 3);

            //Increase max amount +N
            maxEnemiesAmount += 2;
        }
    }


    void PoolEnemy()
    {

        Debug.Log("Pooled object Method called");
        //This will run at the begining , it will run only one time
        if(pooledEnemies.Count == 0)
        {
            for(int i = 0; i < firstPoolIndex; i++)
            {
                Debug.Log("First Time Inside Poooled");
                StartCoroutine(CreateEnemyWithDealy());

            }
            return;
        }

        //if there is di-active enemy , activate him , else create new
        //Check if there is any available enemy
        foreach(Enemy pooledEnemy in pooledEnemies)
        {
            if(!pooledEnemy.gameObject.activeSelf)
            {
                Debug.Log("Available enemy already exist with name :" + pooledEnemy);
                //This one is available
                currentEnemiesAmount++;
                ResetEnemy(pooledEnemy);
                return;
            }
        }

        //Else Create a new one
        Debug.Log("didnt find available enemy. creating new one");
        StartCoroutine(CreateEnemyWithDealy());
    }

    IEnumerator CreateEnemyWithDealy()
    {
        currentEnemiesAmount++;
        CreateEnemy();

        float randT = Random.Range(1f , 2.5f);
        yield return new WaitForSeconds(randT);
    }

    void ResetEnemy( Enemy reusedEnemy )
    {
        Debug.Log("Reseting a new enemy");

        int randEnemy = Random.Range(0 , enemies.Length);

        int randKey = Random.Range(0 , PlayerInputs.GetDictionaryCount());

        reusedEnemy.isReadyToDie = false;
        //Add him to the activeEnemy List
        activeEnemies.Add(reusedEnemy);

        //Give him a key ,
        //the following vars will change inside the external function
        KeyCode newKey = PlayerInputs.GetKeyCode(randKey);
        string newLetter = PlayerInputs.GetStringCode(randKey);


        //Set the values
        reusedEnemy.SetKey(newKey);
        reusedEnemy.keyText.text = newLetter.ToString();

        //Set activity and pos
        reusedEnemy.transform.position = enemySpawnPos;
        reusedEnemy.gameObject.SetActive(true);
    }


    void CreateEnemy()
    {
        Debug.Log("Creating a new enemy");

        int randEnemy = Random.Range(0 , enemies.Length);

        int randKey = Random.Range(0 , PlayerInputs.GetDictionaryCount());

        //Create a random enemy 
        Enemy newEnemy = Instantiate(enemies[randEnemy]);
        newEnemy.isReadyToDie = false;

        //Add him to the pooled list
        pooledEnemies.Add(newEnemy);

        //Add him to the activeEnemy List
        activeEnemies.Add(newEnemy);


        //Give him a key ,
        //the following vars will change inside the external function
        KeyCode newKey = PlayerInputs.GetKeyCode(randKey);
        string newLetter = PlayerInputs.GetStringCode(randKey);


        //Set the values
        newEnemy.SetKey(newKey);
        newEnemy.keyText.text = newLetter.ToString();

        //Set activity and pos
        newEnemy.transform.position = enemySpawnPos;
        newEnemy.gameObject.SetActive(true);

    }


    public void ResetEnemyOnDefaultDeath( Enemy enemy )
    {
        currentEnemiesAmount--;
        activeEnemies.Remove(enemy);
    }

    #endregion
}
