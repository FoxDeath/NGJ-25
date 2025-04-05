using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemySO", order = 1)]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public RuntimeAnimatorController animator;
    
    [SerializeField]
    public EnemyAttributes attributes;
}

[Serializable]
public class EnemyAttributes
{
    public int health;
    public float speed;
    public int reward;
}