using UnityEngine;

public class TowerFactory
{
    public void CreateTower(TowerSO towerSO, Vector3 spawnPoint)
    {
        GameObject towerObject = new GameObject(towerSO.towerName);
        Tower tower = towerObject.AddComponent<Tower>();
        
        GameObject towerSpriteObject = new GameObject("TowerSprite");
        towerSpriteObject.transform.SetParent(towerObject.transform);
        SpriteRenderer spriteRenderer = towerSpriteObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = towerSO.towerSprite;
        
        SpriteController spriteController = towerSpriteObject.AddComponent<SpriteController>();
        
        tower.Initialize(towerSO, spawnPoint);
    }
}