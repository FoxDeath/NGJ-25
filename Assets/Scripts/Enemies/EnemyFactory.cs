using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFactory
{
    public Enemy CreateEnemy(EnemySO enemySO, Transform spawnPoint, Transform targetPoint, GameController gameController)
    {
        GameObject enemyObject = new GameObject(enemySO.enemyName);
        enemyObject.transform.position = spawnPoint.position;
        enemyObject.tag = "Enemy";

        Enemy enemy = enemyObject.AddComponent<Enemy>();
        
        NavMeshAgent navMeshAgent = enemyObject.AddComponent<NavMeshAgent>();
        navMeshAgent.speed = enemySO.attributes.speed;
        navMeshAgent.acceleration = 100f;
        navMeshAgent.angularSpeed = 100f;
        navMeshAgent.areaMask = 1 << NavMesh.GetAreaFromName("Walkable");
        
        CapsuleCollider capsuleCollider = enemyObject.AddComponent<CapsuleCollider>();
        capsuleCollider.radius = 0.5f;
        capsuleCollider.height = 2f;
        
        GameObject spriteObject = new GameObject("Sprite");
        spriteObject.transform.SetParent(enemyObject.transform);
        SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
        SpriteController spriteController = spriteObject.AddComponent<SpriteController>();
        Animator animator = spriteObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = enemySO.animator;
        
        enemy.Initialize(enemySO, navMeshAgent, spawnPoint, targetPoint, gameController);
        return enemy;
    }
           
}