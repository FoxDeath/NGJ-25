using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    public TowerAttributes attributes;

    public TowerSO towerSO;
    
    private float attackCooldown;
    
    GameController gameController;
    
    private Transform spriteTransform;
    private Vector3 spriteTransformLocalScale;
    
    public Animator animator;

    private void Start()
    {
        spriteTransform = transform.GetChild(0);
        
        spriteTransformLocalScale = spriteTransform.localScale;
    }

    public void Initialize(TowerSO towerSO, Vector3 spawnPoint)
    {
        this.towerSO = towerSO;
        // Initialize the tower with the provided TowerSO and spawn point
        transform.position = spawnPoint;
        attributes = towerSO.attributes;
        gameController = FindAnyObjectByType<GameController>();
        AudioManager.instance.PlayOneShot(this.towerSO.placementAudio, this.transform.position);
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
                animator.SetTrigger(Attack1);

                // Play shot audio
                AudioManager.instance.PlayOneShot(this.towerSO.shotAudio, this.transform.position);
                
                if(spriteTransform)
                {
                    if(transform.position.x > closestEnemy.transform.position.x)
                    {
                        spriteTransform.localScale = new Vector3(-1 * spriteTransformLocalScale.x, spriteTransformLocalScale.y, spriteTransformLocalScale.z);
                    }
                    else
                    {
                        spriteTransform.localScale = spriteTransformLocalScale;
                    }
                }

                if(towerSO.towerType == TowerType.AOENormal)
                {
                    // Implement AOE attack logic here
                    foreach (var enemy in enemiesInRange)
                    {
                        enemy.TakeDamage(attributes.damage);
                    }
                }
                else if(towerSO.towerType == TowerType.Projectile)
                {
                    closestEnemy.TakeDamage(attributes.damage);

                    // Instantiate projectile and set its target to the closest enemy
                    GameObject projectile = new GameObject(gameObject.name+ " Projectile");
                    projectile.transform.position = transform.position + new Vector3(0f, 4f, 0f);
                    SpriteRenderer spriteRenderer = projectile.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = attributes.projectile;
                    projectile.AddComponent<Projectile>().Initialize(closestEnemy.transform, attributes.projectileSpeed);

                }
                else if(towerSO.towerType == TowerType.Laser)
                {
                    // Implement laser attack logic here
                    closestEnemy.TakeDamage(attributes.damage);
                }
                else if(towerSO.towerType == TowerType.AOEOnPoint)
                {
                    closestEnemy.TakeDamage(attributes.damage);
        
                    foreach (var enemy in gameController.enemies)
                    {
                        if(enemy == closestEnemy)
                        {
                            continue;
                        }
                        
                        float distance = Vector3.Distance(closestEnemy.transform.position, enemy.transform.position);
                        if(distance <= 5f)
                        {
                            enemy.TakeDamage(Mathf.RoundToInt(attributes.damage / 2f));
                        }
                    }    
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