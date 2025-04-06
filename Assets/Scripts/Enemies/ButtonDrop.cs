using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDrop : MonoBehaviour
{
    [SerializeField] List<Sprite> sprites;
    
    private GameController gameController;
    private int attributesReward;

    private void Start()
    {
        gameController = FindAnyObjectByType<GameController>();
        
        // Set a random sprite from the list
        int randomIndex = UnityEngine.Random.Range(0, sprites.Count);
        GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        gameController.points += attributesReward;
        gameController.textSpawner.Spawn(transform.position, "+" + attributesReward);
        Destroy(gameObject);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.coinPick, this.transform.position);
    }

    public void SetValue(int attributesReward)
    {
        this.attributesReward = attributesReward;
    }
}
