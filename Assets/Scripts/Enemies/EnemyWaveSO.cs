using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "ScriptableObjects/EnemyWaveSO", order = 1)]
public class EnemyWaveSO : ScriptableObject
{
    public List<EnemiesToSpawn> enemiesToSpawn;
    
    public float spawnDelay;
    
    public float spawnDelayVariance;

    public float timeDifficultyUp;
    public float timeDifficultyDown;
    public float timeMax;
}

[Serializable]
public class EnemiesToSpawn
{
    public EnemySO enemy;
    public float spawnDelay;
    public float spawnDelayVariance;
    public int amount;
}