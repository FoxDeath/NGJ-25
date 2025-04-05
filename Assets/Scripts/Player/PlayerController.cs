using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private TowerFactory towerFactory;
    
    [SerializeField] private float speed = 5f;

    private Vector2 moveInput;
    private Vector2 mousePosition;
    
    private PlayerInput playerInput;
    
    //TODO: TOWER STUFF
    [SerializeField] private SpriteRenderer heldTower;
    
    [SerializeField] private List<TowerSO> towerConfigs;
    private TowerSO currentTowerConfig;
    
    [SerializeField] private NavMeshSurface navMeshSurface;

    private int selectedTower;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        towerFactory = new TowerFactory();
        
        mainCamera = Camera.main;
        playerInput = new PlayerInput();
        playerInput.Player.Enable();
        
        playerInput.Player._1.performed += ctx => SelectTower(0);
        playerInput.Player._2.performed += ctx => SelectTower(1);
        playerInput.Player._3.performed += ctx => SelectTower(2);
        playerInput.Player._4.performed += ctx => SelectTower(3);
        playerInput.Player._5.performed += ctx => SelectTower(4);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = playerInput.Player.Move.ReadValue<Vector2>();
        mousePosition = playerInput.Player.MousePosition.ReadValue<Vector2>();
        
        Move(moveInput);

        // Convert to world position
        RotateHeldTower();

        CheckHeldTower();
    }

    private void CheckHeldTower()
    {
        
    }

    private void RotateHeldTower()
    {
        // Convert to world position
        Vector3 pointerWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Vector3.Distance(transform.position, mainCamera.transform.position)));

        // Get direction from player to mouse
        Vector3 direction = (pointerWorldPos - transform.position).normalized;

        // Position the object at fixed distance in that direction
        heldTower.transform.position = new Vector3(transform.position.x + direction.x * 3, transform.position.y, transform.position.z + direction.y * 3);
    }

    private void Move(Vector2 direction)
    {
        Vector3 move = new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
        transform.Translate(move);
    }

    private void SelectTower(int i)
    {
        if(i == selectedTower)
        {
            currentTowerConfig = null;
            heldTower.sprite = null;
            selectedTower = -1;
            return;
        }
        
        selectedTower = i;
        
        currentTowerConfig = towerConfigs[selectedTower];
        heldTower.sprite = currentTowerConfig.towerSprite;
    }
}
