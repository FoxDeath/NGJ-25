using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference bunnyFootsteps { get; private set; }


    [field: Header("Enemy SFX")]
    [field: SerializeField] public EventReference toySoldiersShot { get; private set; } 
    [field: SerializeField] public EventReference spiderSteps { get; private set; }
    [field: SerializeField] public EventReference spiderScream { get; private set; }
    [field: SerializeField] public EventReference spiderDeath { get; private set; }
    
    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference hpLoss { get; private set; }
    [field: SerializeField] public EventReference endGameWin { get; private set; }
    [field: SerializeField] public EventReference endGameLose { get; private set; }


    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}
