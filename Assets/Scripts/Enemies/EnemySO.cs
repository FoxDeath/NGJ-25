using System;
using UnityEngine;
using FMODUnity;


[CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/EnemySO", order = 1)]
public class EnemySO : ScriptableObject
{
    [field: SerializeField] public EventReference stepsAudio { get; private set; }
    [field: SerializeField] public EventReference screamAudio { get; private set; }
    [field: SerializeField] public EventReference deathAudio { get; private set; }
    public string enemyName;
    public GameObject sprite;
    public Vector3 spriteOffset;
    
    [SerializeField]
    public EnemyAttributes attributes;

    public EnemySO littleJerry;
}

[Serializable]
public class EnemyAttributes
{
    public int health;
    public float speed;
    public int reward;
    public int damage;
}