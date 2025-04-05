using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DamageNumbersPro;
using TMPro;
using UnityEngine;
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
    
    public List<EnemySO> enemySOs;
    
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

    private void Start()
    {
        enemyFactory = new EnemyFactory();
        towerFactory = new TowerFactory();
        
        points = startPoints;
        currentHealth = maxHealth;
        
        SpawnEnemiesTest().Forget();
    }

    private void Update()
    {
        pointsText.text = points.ToString();
        healthText.text = currentHealth.ToString();
        healthBar.value = (float)currentHealth / maxHealth;
    }

    private async UniTask SpawnEnemiesTest()
    {
        for (int i = 0; i < 10; i++)
        {
            Enemy enemy = enemyFactory.CreateEnemy(enemySOs[0], spawnPoints[Random.Range(0, spawnPoints.Count)], targetPoints[Random.Range(0, targetPoints.Count)], this);
            enemies.Add(enemy);
            await UniTask.Delay(1000, cancellationToken:destroyCancellationToken);
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
        
        Destroy(enemy.gameObject);
    }
    
    public void TakeDamage(Enemy enemy)
    {
        currentHealth -= enemy.attributes.damage;
        
        enemies.Remove(enemy);
        
        damageNumberBig.Spawn(enemy.transform.position, enemy.attributes.damage.ToString());
        if (currentHealth <= 0)
        {
            // Handle game over
            Debug.Log("Game Over");
        }
        
        Destroy(enemy.gameObject);
    }
}