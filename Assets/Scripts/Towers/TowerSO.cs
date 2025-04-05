using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "ScriptableObjects/TowerSO", order = 1)]
public class TowerSO : ScriptableObject
{
    public string towerName;
    public Sprite towerSprite;
    public RuntimeAnimatorController animator;
    
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
}
