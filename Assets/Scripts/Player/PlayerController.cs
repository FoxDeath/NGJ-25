using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private Vector2 moveInput;
    private Vector2 mousePosition;
    
    private PlayerInput playerInput;
    
    //TODO: TOWER STUFF
    [SerializeField] private SpriteRenderer heldTower;
    [SerializeField] private Sprite towerPrefab;
    [SerializeField] private NavMeshSurface navMeshSurface;
    private bool canPlaceTower;

    private int selectedTower;
    private Camera mainCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
    }

    private void SelectTower(int i)
    {
        if(i == selectedTower)
        {
            heldTower.sprite = null;
            selectedTower = -1;
            return;
        }
        
        selectedTower = i;
        
        Debug.Log("Selected tower: " + selectedTower);

        switch(selectedTower)
        {
            case 0:
                heldTower.sprite = towerPrefab;
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break; 
            case 4:
                break;
            default:
                return;
        }
    }
}
