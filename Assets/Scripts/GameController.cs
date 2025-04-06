using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    private EnemyFactory enemyFactory;
    private TowerFactory towerFactory;

    [SerializeField] internal DamageNumber textSpawner;
    [SerializeField] internal DamageNumber damageNumber;
    [SerializeField] internal DamageNumber damageNumberBig;
    
    public List<Transform> spawnPoints;
    public List<Transform> targetPoints;
    
    internal List<Enemy> enemies = new List<Enemy>();

    [SerializeField] private int startPoints;
    internal int points = 0;
    
    [SerializeField] private GameObject buttonDropPrefab;
    
    [SerializeField] TextMeshProUGUI pointsText;

    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    
    [SerializeField] private List<EnemyWaveSO> enemyWaves;
    private int currentWaveIndex = 0;
    private float currentWaveTime = 0f;

    [SerializeField] private float maxTime;
    private float currentTime;
    
    [SerializeField] private TextMeshProUGUI timerText;

    [SerializeField] private GameObject hudCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject startCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject pauseCanvas;

    private bool gameStarted;
    private bool gameEnded;

    private void Start()
    {
        Time.timeScale = 0f;
        
        enemyFactory = new EnemyFactory();
        towerFactory = new TowerFactory();
        
        points = startPoints;
        currentHealth = maxHealth;
        
        currentTime = maxTime;
        
        SpawnWave(enemyWaves[currentWaveIndex]).Forget();
    }

    private void Update()
    {
        pointsText.text = points.ToString();
        healthText.text = currentHealth.ToString();
        healthBar.value = (float)currentHealth / maxHealth;
        
        currentWaveTime += Time.deltaTime;
        
        currentTime -= Time.deltaTime;

        if(currentTime > 0f)
        {
            timerText.text = currentTime.ToString("0.00");
        }
        else
        {
            timerText.text = "0.00";
            currentTime = 0f;
            
            gameEnded = true;
            hudCanvas.SetActive(false);
            winCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
        
        if(currentWaveTime > enemyWaves[currentWaveIndex].timeMax)
        {
            currentWaveTime = 0f;
            currentWaveIndex--;
            if(currentWaveIndex < 0)
            {
                currentWaveIndex = 0;
            }
            
            SpawnWave(enemyWaves[currentWaveIndex]).Forget();
            
            return;
        }

        if(enemies.Count == 0)
        {
            if(currentWaveTime < enemyWaves[currentWaveIndex].timeDifficultyUp)
            {
                currentWaveTime = 0f;
                currentWaveIndex++;
                if(currentWaveIndex >= enemyWaves.Count)
                {
                    currentWaveIndex = enemyWaves.Count - 1;
                }
                
                SpawnWave(enemyWaves[currentWaveIndex]).Forget();
            }
            else if(currentWaveTime > enemyWaves[currentWaveIndex].timeDifficultyDown)
            {
                currentWaveTime = 0f;
                currentWaveIndex--;
                if(currentWaveIndex < 0)
                {
                    currentWaveIndex = 0;
                }
                
                SpawnWave(enemyWaves[currentWaveIndex]).Forget();
            }
            else
            {
                currentWaveTime = 0f;
                
                SpawnWave(enemyWaves[currentWaveIndex]).Forget();
            }
        }
    }

    private async UniTask SpawnWave(EnemyWaveSO waveSo)
    {
        currentWaveTime = 0f;
        
        foreach(var enemiesToSpawn in waveSo.enemiesToSpawn)
        {
            for (int i = 0; i < enemiesToSpawn.amount; i++)
            {
                Enemy enemy = enemyFactory.CreateEnemy(enemiesToSpawn.enemy, spawnPoints[Random.Range(0, spawnPoints.Count)], targetPoints[Random.Range(0, targetPoints.Count)], this);
                enemies.Add(enemy);
                
                await UniTask.Delay((int)((enemiesToSpawn.spawnDelay + Random.Range(-enemiesToSpawn.spawnDelayVariance, enemiesToSpawn.spawnDelayVariance)) * 1000), cancellationToken:destroyCancellationToken);
            }
            
            await UniTask.Delay((int)((waveSo.spawnDelay + Random.Range(-waveSo.spawnDelayVariance, waveSo.spawnDelayVariance)) * 1000), cancellationToken:destroyCancellationToken);
        }
    }

    public void SpawnTower(TowerSO towerSo, Vector3 position)
    {
        if(points < towerSo.attributes.cost)
        {
            textSpawner.Spawn(position, "Not enough buttons");
            return;
        }
        
        points -= towerSo.attributes.cost;
        towerFactory.CreateTower(towerSo, position);
    }

    public void KillEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);

        GameObject buttonDrop = Instantiate(buttonDropPrefab, enemy.transform.position + new Vector3(0f, 2f, 0f), Quaternion.identity);
        buttonDrop.GetComponent<ButtonDrop>().SetValue(enemy.attributes.reward);
        
        Destroy(enemy.gameObject, 3f);
    }
    
    public void TakeDamage(Enemy enemy)
    {
        currentHealth -= enemy.attributes.damage;
        
        enemies.Remove(enemy);
        
        damageNumberBig.Spawn(enemy.transform.position, enemy.attributes.damage.ToString());
        if (currentHealth <= 0)
        {
            hudCanvas.SetActive(false);
            gameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
            gameEnded = true;
            // Handle game over
            Debug.Log("Game Over");
        }
        
        Destroy(enemy.gameObject);
    }
    
    public void PauseGame()
    {
        if(gameEnded || gameStarted == false)
        {
            return;
        }
        
        if(Time.timeScale == 0f)
        {
            ResumeGame();
            return;
        }
        
        Time.timeScale = 0f;
        
        hudCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        
        hudCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }
    
    public void StartGame()
    {
        Time.timeScale = 1f;
     
        gameStarted = true;
        startCanvas.SetActive(false);
        hudCanvas.SetActive(true);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void QuitApplication()
    {
        Application.Quit();
    }
}