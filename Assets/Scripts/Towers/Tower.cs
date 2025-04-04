﻿using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerAttributes attributes;
    
    private float attackCooldown;
    
    GameController gameController;

    public void Initialize(TowerSO towerSO, Vector3 spawnPoint)
    {
        // Initialize the tower with the provided TowerSO and spawn point
        transform.position = spawnPoint;
        attributes = towerSO.attributes;
        gameController = FindAnyObjectByType<GameController>();

        // Set other properties based on towerSO
    }

    public void Update()
    {
        // Handle attack cooldown
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        else
        {
            Attack();
            attackCooldown = attributes.attackSpeed;
        }
    }

    private void Attack()
    {
        if(!gameController || gameController.enemies.Count == 0)
        {
            return;
        }
        
        // Check if there are any enemies in range
        List<Enemy> enemiesInRange = new List<Enemy>();
        
        foreach (var enemy in gameController.enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= attributes.attackRange)
            {
                enemiesInRange.Add(enemy);
            }
        }
        
        // Implement attack logic
        if (enemiesInRange.Count > 0)
        {
            // Find the closest enemy
            Enemy closestEnemy = enemiesInRange[0];
            float closestDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);
            
            foreach (var enemy in enemiesInRange)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            if(closestEnemy != null)
            {
                closestEnemy.TakeDamage(attributes.damage);
                
                if(attributes.projectile != null)
                {
                    // Instantiate projectile and set its target to the closest enemy
                    GameObject projectile = new GameObject(gameObject.name+ " Projectile");
                    projectile.transform.position = transform.position;
                    SpriteRenderer spriteRenderer = projectile.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = attributes.projectile;
                    projectile.AddComponent<Projectile>().Initialize(closestEnemy.transform, attributes.projectileSpeed);
                }
            }
            
            // Attack the closest enemy
            // Implement attack logic here
        }
    }

    private void OnDrawGizmos()
    {
        // Draw attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attributes.attackRange);
        
        // Draw tower position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}