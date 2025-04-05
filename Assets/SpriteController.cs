using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private Animator animator;
    private Camera mainCamera;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
    }

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the sprite to face the camera
        Vector3 direction = -mainCamera.transform.forward;
        direction.y = 0; // Keep the rotation on the Y axis
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.localPosition = Vector3.zero;
        transform.rotation = rotation;
    }
}
