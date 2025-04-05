using System;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerAttributes attributes;
    internal List<Enemy> enemiesInRange = new List<Enemy>();
    
    private float attackCooldown;
    
    public void Initialize(TowerSO towerSO, Vector3 spawnPoint)
    {
        // Initialize the tower with the provided TowerSO and spawn point
        transform.position = spawnPoint;
        attributes = towerSO.attributes;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Add(enemy);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    public void Attack()
    {
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
                closestEnemy.TakeDamage(attributes.damage, this);
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