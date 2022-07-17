using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AnimationCurve SpawningCurve;
    public float waveTime;
    float spawnMultiplier =1f;
    public float enemyCount=1f;
    public List<BaseEnemy> EnemyList;
    public List<BaseEnemy> SpawnedEnemies = new List<BaseEnemy>();

    bool enemySpawned;
    float UpgradeSpawnTimer;
    float upgradeMult;
    int levelMult;
    public bool isPlayerAlive;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI endScoreText;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI shotsFiredText;
    public TextMeshProUGUI timeElapsedText;

    public int CashMoney=0;
    public int shotsfired = 0;
    public int enemiesKilled = 0;
    public bool isGameOver;

    public Transform[] Spawnpoints;

    float nextTimeToSpawnEnemies;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            waveTime += Time.deltaTime / 5f;
            UpgradeSpawnTimer += Time.deltaTime;
            if (waveTime >= 1f)
            {
                levelMult++;
                if (levelMult >= EnemyList.Count) levelMult = EnemyList.Count;
                if (!enemySpawned)
                {
                    enemyCount = SpawningCurve.Evaluate(1) * spawnMultiplier;
                    if (enemyCount >= 10) enemyCount = 10;
                    if (enemyCount > 0f && SpawnedEnemies.Count <= 15)
                    {
                        for (int i = 0; i < Mathf.FloorToInt(enemyCount); ++i)
                        {
                            SpawnEnemy();
                        }
                        enemySpawned = true;
                    }
                }
                waveTime = 0;
                spawnMultiplier++;
                enemySpawned = false;
                if (UpgradeSpawnTimer >= upgradeMult * 5f)
                {
                    SpawnEnemy(true);
                    UpgradeSpawnTimer = 0f;
                    if (upgradeMult <= 6f)    //capped to 30 sec max between each upgrade
                        upgradeMult++;
                }
            }
        }
        scoreText.text = "Score : $" + CashMoney.ToString(); 
        if(isGameOver)
        {
            endScoreText.text = "Score : \n$" + CashMoney.ToString();
            enemiesKilledText.text = "Enemies Killed :\n" + enemiesKilled.ToString();
            shotsFiredText.text = "Shots fired:\n" + shotsfired.ToString();
            timeElapsedText.text = "Time Spent:\n" + Mathf.FloorToInt(Time.timeSinceLevelLoad).ToString() + " seconds";
        }
        //Debug.Log(Mathf.RoundToInt(enemyCount) - Mathf.FloorToInt(enemyCount));
        //if((Mathf.RoundToInt(enemyCount)-Mathf.FloorToInt(enemyCount))>=1 && !enemySpawned)
        //{
        //    SpawnEnemy();
        //    enemyCount--;
        //}
        //spawn enemy
        //subscribe onEnemyDeathFunction to spawnedEnemy.deathEvent += function
    }
    [Button]
    public void SpawnEnemy(bool willDropUpgrade = false)
    {
        BaseEnemy enemyClone;
        enemySpawned = true;
        //max left = -28
        //max right = 29
        //max top = 15
        //max bot = -15

        Vector2 spawnPos = Spawnpoints[Random.Range(0, Spawnpoints.Length)].position;


        //float RightX = Camera.main.ViewportToWorldPoint(Vector2.one).x + Random.Range(0,5);
        //float LeftX = RightX * -1; 
        //Vector2 RightSpawnPos = new Vector2(RightX, Random.Range(-3f, 3f));
        //Vector2 LeftSpawnPos = new Vector2(LeftX, Random.Range(-3f, 3f));
        enemyClone= Instantiate(EnemyList[Random.Range(0,levelMult)], spawnPos, Quaternion.identity);
        enemyClone.willDropUpgrade = willDropUpgrade;
        SpawnedEnemies.Add(enemyClone);
        enemyClone.deathEvent += OnEnemyDeath;
    }
    void OnEnemyDeath()
    {
        //stuff to do when enemy dies
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QUIT()
    {
        Application.Quit();

    }
}
