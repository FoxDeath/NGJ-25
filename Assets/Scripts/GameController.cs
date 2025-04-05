using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public EnemyFactory enemyFactory;
    
    public List<EnemySO> enemySOs;
    
    public Transform spawnPoint;
    public Transform targetPoint;
    
    private void Start()
    {
        enemyFactory = new EnemyFactory();
        
        SpawnEnemiesTest().Forget();
    }

    private async UniTask SpawnEnemiesTest()
    {
        for (int i = 0; i < 10; i++)
        {
            Enemy enemy = enemyFactory.CreateEnemy(enemySOs[0], spawnPoint, targetPoint);
            await UniTask.Delay(1000, cancellationToken:destroyCancellationToken);
        }
    }
}