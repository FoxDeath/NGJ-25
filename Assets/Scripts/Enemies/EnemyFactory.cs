using System;
using UnityEngine;

public class EnemyFactory
{
    public EnemySO enemySO;

    public Enemy CreateEnemy()
    {
        GameObject enemyObject = new GameObject(enemySO.enemyName);
        Enemy enemy = enemyObject.AddComponent<Enemy>();
        GameObject spriteObject = new GameObject("Sprite");
        spriteObject.transform.SetParent(enemyObject.transform);
        SpriteController spriteController = spriteObject.AddComponent<SpriteController>();
        Animator animator = spriteObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = enemySO.animator;
        enemy.Initialize(enemySO);
        return enemy;
    }
           
}