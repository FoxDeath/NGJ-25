using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;
    private Animator animator;
    private EnemyAttributes attributes;
    private NavMeshAgent navMeshAgent;
    private GameController gameController;
    
    
    private Transform targetPoint;
    
    private int health;

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
        DamagePlaceholderAnimation().Forget();
        if (health <= 0)
        {
            Die();
            AudioManager.instance.PlayOneShot(this.enemySO.deathAudio, this.transform.position);
        }
    }
    
    private void Die()
    {
        gameController.enemies.Remove(this);
        // Handle enemy death
        Destroy(gameObject);
        // Give reward to player
        // tower.GiveReward(attributes.reward);
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