using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using FMOD.Studio;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private static readonly int Moving = Animator.StringToHash("Moving");
    private GameController gameController;
    
    [SerializeField] private float speed = 5f;

    private Vector2 moveInput;
    private Vector2 mousePosition;
    
    private PlayerInput playerInput;

    // audio
    private EventInstance playerFootsteps;
    
    //TODO: TOWER STUFF
    [SerializeField] private SpriteRenderer heldTower;
    
    [SerializeField] private List<TowerSO> towerConfigs;
    private TowerSO currentTowerConfig;
    
    private bool canPlaceTower;
    private int selectedTower;
    private Camera mainCamera;

    private Animator animator;

    private Transform spriteTransform;

    private bool isMoving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        selectedTower = -1;
        
        gameController = FindAnyObjectByType<GameController>();

        animator = GetComponentInChildren<Animator>();

        spriteTransform = transform.GetChild(0);
        
        mainCamera = Camera.main;
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        
        playerInput.Player._1.performed += ctx => SelectTower(0);
        playerInput.Player._2.performed += ctx => SelectTower(1);
        playerInput.Player._3.performed += ctx => SelectTower(2);
        playerInput.Player._4.performed += ctx => SelectTower(3);
        playerInput.Player._5.performed += ctx => SelectTower(4);
        
        playerInput.Player.PlaceTower.performed += ctx => PlaceTower();
        
        playerInput.Player.Escape.performed += ctx => gameController.PauseGame();
    }
    
    private void OnDestroy()
    {
        playerInput.Player.Disable();
        playerInput.Dispose();
        
        playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        playerFootsteps.release();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        mousePosition = playerInput.Player.MousePosition.ReadValue<Vector2>();

        if(moveInput != Vector2.zero && !isMoving)
        {
            isMoving = true;
            animator.SetBool(Moving, true);
            // get the playback state
            PLAYBACK_STATE playbackState;
            playerFootsteps.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                playerFootsteps.start();
            }
        }
        else if(moveInput == Vector2.zero && isMoving)
        {
            isMoving = false;
            animator.SetBool(Moving, false);
            playerFootsteps.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        
        Move(moveInput);

        // Convert to world position
        RotateHeldTower();

        if(selectedTower != -1)
            CheckHeldTower();
    }

    private void CheckHeldTower()
    {
        string areaName = "Walkable";
        var targetAreaMask = 1 << NavMesh.GetAreaFromName(areaName);
        if (NavMesh.SamplePosition(heldTower.transform.position, out NavMeshHit navMeshHit, 5f, NavMesh.AllAreas))
        {
            int areaIndex = navMeshHit.mask;

            // Compare the area
            if ((areaIndex & targetAreaMask) != 0)
            {
                heldTower.color = Color.red;
                canPlaceTower = false;
            }
            else
            {
                heldTower.color = Color.white;
                canPlaceTower = true;
            }
        }
    }
    
    private void PlaceTower()
    {
        if (canPlaceTower && currentTowerConfig)
        {
            // Create the tower at the held position
            gameController.SpawnTower(currentTowerConfig, heldTower.transform.position);
            currentTowerConfig = null;
            heldTower.sprite = null;
            selectedTower = -1;
        }
    }

    private void RotateHeldTower()
    {
        // Convert to world position
        Vector3 pointerWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Vector3.Distance(transform.position, mainCamera.transform.position)));

        // Get direction from player to mouse
        Vector3 direction = (pointerWorldPos - transform.position).normalized;

        // Position the object at fixed distance in that direction
        heldTower.transform.position = new Vector3(transform.position.x + direction.x * 6, transform.position.y, transform.position.z + direction.y * 6);
    }

    private void Move(Vector2 direction)
    {
        Vector3 move = new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
        transform.Translate(move);

        if(direction.x > 0)
        {
            spriteTransform.localScale = new Vector3(1, 1, 1);
        }
        else if(direction.x < 0)
        {
            spriteTransform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void SelectTower(int i)
    {
        if(i == selectedTower)
        {
            currentTowerConfig = null;
            heldTower.sprite = null;
            selectedTower = -1;
            canPlaceTower = false;
            return;
        }
        
        selectedTower = i;
        
        currentTowerConfig = towerConfigs[selectedTower];
        heldTower.sprite = currentTowerConfig.towerSprite;
    }


    private void UpdateSound()
    {
        // start footsteps event if the player has an x velocity and is on the ground
        if (moveInput.x != 0)
        {

        }
        // otherwise, stop the footsteps event
        else 
        {
        }
    }
}
