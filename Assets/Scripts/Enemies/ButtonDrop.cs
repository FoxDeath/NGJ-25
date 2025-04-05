using System;
using UnityEngine;

public class ButtonDrop : MonoBehaviour
{
    private GameController gameController;
    private int attributesReward;

    private void Start()
    {
        gameController = FindAnyObjectByType<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        gameController.points += attributesReward;
        gameController.textSpawner.Spawn(transform.position, "+" + attributesReward);
        Destroy(gameObject);
    }

    public void SetValue(int attributesReward)
    {
        this.attributesReward = attributesReward;
    }
}
