using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class Enemy : MonoBehaviour
{
    private static readonly int Die1 = Animator.StringToHash("Die");
    public EnemySO enemySO;
    internal Animator animator;
    internal EnemyAttributes attributes;
    private NavMeshAgent navMeshAgent;
    private GameController gameController;
    
    
    private Transform targetPoint;
    
    private int health;

    private void Update()
    {
        if(Vector3.Distance(transform.position, targetPoint.position) < 5f)
        {
            // Handle enemy reaching the target point
            // e.g., damage player, etc.
            gameController.TakeDamage(this);
        }
    }

    public void Initialize(EnemySO enemySO, NavMeshAgent navMeshAgent, Transform spawnPoint, Transform targetPoint, GameController gameController)
    {
        this.enemySO = enemySO;
        this.navMeshAgent = navMeshAgent;
        attributes = enemySO.attributes;
        this.targetPoint = targetPoint;
        
        health = attributes.health;
        
        // AudioManager.instance.PlayOneShot(FMODEvents.instance.screa, this.transform.position);
        navMeshAgent.SetDestination(targetPoint.position);
        this.gameController = gameController;
        // Initialize other properties based on enemySO
    }

    public void TakeDamage(int damage)
    {

        // audio
        AudioManager.instance.PlayOneShot(this.enemySO.screamAudio, this.transform.position);
        health -= damage;
        gameController.damageNumber.Spawn(transform.position, damage);
        DamagePlaceholderAnimation().Forget();
        if (health <= 0)
        {
            navMeshAgent.isStopped = true;
            Die();
            AudioManager.instance.PlayOneShot(this.enemySO.deathAudio, this.transform.position);
        }
    }
    
    private void Die()
    {
        animator.SetTrigger(Die1);
        gameController.KillEnemy(this);

        if(enemySO.littleJerry != null)
        {
            SpawnLittleJerries().Forget();
        }
    }

    private async UniTask SpawnLittleJerries()
    {
        for(int i = 0; i < 5; i++)
        {
            gameController.enemyFactory.CreateEnemy(enemySO.littleJerry, transform, targetPoint, gameController);
            
            await UniTask.Delay(100, cancellationToken: destroyCancellationToken);
        }
    }

    private async UniTask DamagePlaceholderAnimation()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            await UniTask.Delay(100, cancellationToken: destroyCancellationToken);
            spriteRenderer.color = originalColor;
        }
    }
}