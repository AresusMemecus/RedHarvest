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
    public Sprite frontSideSprite;
    public Sprite backSideSprite;
    public Transform shadowTransform;
    public float minShadowScale = 0.4f;   // Минимальный размер тени (в прыжке)
    public float maxShadowScale = 1f; 
    private DialogueManager dialogueManager;
    [SerializeField] private InventoryUI inventoryUI;


    void Start()
    {
        Debug.Log("PlayerController Start method called");
        Time.timeScale = 1;
        controller = GetComponent<CharacterController>();
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

        // --- ПРИМЕНЯЕМ ГРАВИТАЦИЮ И ДВИЖЕНИЕ ---
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -1f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move((movement + velocity) * Time.deltaTime);

        // --- ОБНОВЛЯЕМ СПРАЙТ ---
        if (moveInput.magnitude > 0.1f)
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

        
        if (shadowTransform != null)
        {
            // Ищем поверхность под персонажем
            Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 10f))
            {
                // Ставим тень на найденную точку
                Vector3 shadowPos = hit.point;
                shadowPos.y += 0.01f; // немного выше, чтобы не мерцала
                shadowTransform.position = shadowPos;

                // Высота над землёй
                float height = transform.position.y - hit.point.y;
                float t = Mathf.InverseLerp(0f, 2f, height);
                float scale = Mathf.Lerp(maxShadowScale, minShadowScale, t);
                shadowTransform.localScale = new Vector3(scale, scale, scale);
            }
        }
    }

    public void OnMoved(InputValue value)
    {
        Debug.Log("OnMove called"); // Логируем, вызывается ли метод
        if (dialogueManager == null)
        {
            Debug.LogWarning("DialogueManager is null!");
            moveInput = value.Get<Vector2>();
            return;
        }
        
        if (dialogueManager.isDialogueActive)
        {
            Debug.Log("Movement blocked by active dialogue");
            return;
        }

        moveInput = value.Get<Vector2>();
        Debug.Log("MoveInput: " + moveInput);
    }

    public void OnJump(InputValue value)
    {
        // Блокируем прыжок, если диалог активен
        if (dialogueManager.isDialogueActive) return;

        // Если персонаж на земле, выполняем прыжок
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
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
