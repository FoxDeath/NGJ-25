using UnityEngine;

public class TowerFactory
{
    public void CreateTower(TowerSO towerSO, Vector3 spawnPoint)
    {
        GameObject towerObject = new GameObject(towerSO.towerName);
        Tower tower = towerObject.AddComponent<Tower>();
        tower.Initialize(towerSO, spawnPoint);
    }
}