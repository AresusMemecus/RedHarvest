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

    public SpriteRenderer spriteRenderer;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite sideSprite;
    public Sprite frontSideSprite;
    public Sprite backSideSprite;
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

        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);

        // --- ПРИМЕНЯЕМ ГРАВИТАЦИЮ И ДВИЖЕНИЕ ---
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -1f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move((movement + velocity) * Time.deltaTime);

        // --- ОБНОВЛЯЕМ СПРАЙТ ---
        if (moveInput.magnitude > 0.1f)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                // Движение вбок
                spriteRenderer.sprite = sideSprite;
                spriteRenderer.flipX = moveInput.x > 0;  // зеркалим, если влево
            }
            else
            {
                if (moveInput.y > 0.1f) // Движение вперёд (назад в камеру)
                {
                    if (moveInput.x > 0.1f) // Вперёд + вправо
                    {
                        spriteRenderer.sprite = backSideSprite;
                        spriteRenderer.flipX = true;
                    }
                    else if (moveInput.x < -0.1f) // Вперёд + влево
                    {
                        spriteRenderer.sprite = backSideSprite;
                        spriteRenderer.flipX = false;
                    }
                    else // Чисто вперёд
                    {
                        spriteRenderer.sprite = backSprite;
                        spriteRenderer.flipX = false;
                    }
                }
                else // Движение назад (в сторону камеры)
                {
                    if (moveInput.x > 0.1f) // Назад + вправо
                    {
                        spriteRenderer.sprite = frontSideSprite;
                        spriteRenderer.flipX = true;
                    }
                    else if (moveInput.x < -0.1f) // Назад + влево
                    {
                        spriteRenderer.sprite = frontSideSprite;
                        spriteRenderer.flipX = false;
                    }
                    else // Чисто назад
                    {
                        spriteRenderer.sprite = frontSprite;
                        spriteRenderer.flipX = false;
                    }
                }
            }
        }
        Debug.Log(moveInput.x + " " + moveInput.y);
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
