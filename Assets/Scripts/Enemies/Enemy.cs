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

    public void Move()
    {
    }
}