using UnityEngine;

public class TowerFactory
{
    public void CreateTower(TowerSO towerSO, Vector3 spawnPoint)
    {
        GameObject towerObject = new GameObject(towerSO.towerName);
        Tower tower = towerObject.AddComponent<Tower>();
        
        GameObject spriteObject = Object.Instantiate(towerSO.sprite, towerObject.transform, false);
        SpriteController spriteController = spriteObject.AddComponent<SpriteController>();
        spriteController.offset = towerSO.spriteOffset;

        tower.animator = spriteObject.GetComponent<Animator>();
        
        tower.Initialize(towerSO, spawnPoint);
    }
}