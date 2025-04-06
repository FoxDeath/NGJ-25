using System;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/TowerSO", order = 1)]
public class TowerSO : ScriptableObject
{
    public string towerName;
    public Sprite towerSprite;
    public GameObject sprite;
    public Vector3 spriteOffset;

    [field: SerializeField] public EventReference shotAudio { get; private set; } 
    [field: SerializeField] public EventReference placementAudio { get; private set; } 
    
    [SerializeField]
    public TowerAttributes attributes;
}

[Serializable]
public class TowerAttributes
{
    public int cost;
    public float attackRange;
    public float attackSpeed;
    public int damage;

    public Sprite projectile;
    public float projectileSpeed;
}
