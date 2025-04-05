using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemySO enemySO;
    private Animator animator;
    private EnemyAttributes attributes;

    public void Initialize(EnemySO enemySO)
    {
        this.enemySO = enemySO;
        attributes = enemySO.attributes;
        // Initialize other properties based on enemySO
    }

    public void Move()
    {
        // Implement movement logic
    }
}
