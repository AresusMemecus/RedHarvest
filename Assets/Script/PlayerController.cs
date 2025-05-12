using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1f;
    private CharacterController controller;
    private Vector2 moveInput;
    private Vector3 velocity; 
    public bool isFrozen = false;
    private DialogueManager dialogueManager;
    [SerializeField] private InventoryUI inventoryUI;
    Animator animator;


    void Start()
    {
        Debug.Log("PlayerController Start method called");
        Time.timeScale = 1;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        dialogueManager = FindFirstObjectByType<DialogueManager>();

        // Reset movement input
        moveInput = Vector2.zero;
        
        if (inventoryUI == null)
        {
            inventoryUI = FindFirstObjectByType<InventoryUI>();
            if (inventoryUI == null)
            {
                Debug.LogWarning("InventoryUI component not found! Please assign it in the inspector or make sure it exists in the scene.");
            }
        }
    }

    void Update()
    {   
        // Изометрическое направление
        Vector3 forward = new Vector3(1, 0, 1).normalized;
        Vector3 right = new Vector3(1, 0, -1).normalized;
        Vector3 movement = (forward * moveInput.y + right * moveInput.x) * moveSpeed;

        if (!isFrozen)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);
        }

        // --- ПРИМЕНЯЕМ ГРАВИТАЦИЮ И ДВИЖЕНИЕ ---
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -1f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move((movement + velocity) * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    public void ResetMovement()
    {
        moveInput = Vector2.zero; // Очищаем ввод движения
    }

    public void OnInventory(InputValue value)
    {
        if (value.isPressed)
        {
            inventoryUI?.ToggleInventory();
        }
    }
}
