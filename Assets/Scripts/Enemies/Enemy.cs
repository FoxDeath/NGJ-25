using UnityEngine;
using UnityEngine.AI;
using FMODUnity;

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
        
        AudioManager.instance.PlayOneShot(FMODEvents.instance.spiderScream, this.transform.position);
        // Initialize other properties based on enemySO
    }

    public void Move()
    {

        // audio
        AudioManager.instance.PlayOneShot(FMODEvents.instance.spiderScream, this.transform.position);
    }
}