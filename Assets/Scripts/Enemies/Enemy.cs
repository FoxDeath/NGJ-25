using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;
    private Animator animator;
    private EnemyAttributes attributes;
    private NavMeshAgent navMeshAgent;
    
    
    private Transform targetPoint;

    public void Initialize(EnemySO enemySO, NavMeshAgent navMeshAgent, Transform spawnPoint, Transform targetPoint)
    {
        this.enemySO = enemySO;
        this.navMeshAgent = navMeshAgent;
        attributes = enemySO.attributes;
        this.targetPoint = targetPoint;
        
        navMeshAgent.SetDestination(targetPoint.position);
        
        // Initialize other properties based on enemySO
    }

    public void TakeDamage(int damage, Tower tower)
    {
        attributes.health -= damage;
        if (attributes.health <= 0)
        {
            Die(tower);
        }
    }
    
    private void Die(Tower tower)
    {
        tower.enemiesInRange.Remove(this);
        // Handle enemy death
        Destroy(gameObject);
        // Give reward to player
        // tower.GiveReward(attributes.reward);
    }
}