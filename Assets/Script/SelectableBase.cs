using UnityEngine;

public abstract class SelectableBase : MonoBehaviour
{
    private Material originalMaterial;
    [SerializeField] private Material OutlinedSprite;

    public virtual void OnHoverEnter()
    {
        Debug.Log($"[{gameObject.name}] Наведен курсор (по умолчанию)");
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && OutlinedSprite != null)
        {
            // Сохраняем оригинальный материал перед изменением
            originalMaterial = spriteRenderer.material;
            // Применяем новый материал с контуром
            spriteRenderer.material = OutlinedSprite;
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] Нет SpriteRenderer или outlineMaterial!");
        }
    }
    public virtual void OnHoverExit()
    {
        Debug.Log($"[{gameObject.name}] Курсор ушёл (по умолчанию)");
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null && originalMaterial != null)
        {
            // Восстанавливаем оригинальный материал
            spriteRenderer.material = originalMaterial;
        }
        else
        {
            Debug.LogWarning($"[{gameObject.name}] Нет SpriteRenderer или оригинального материала!");
        }
    }

    public virtual void OnSelect()
    {
        Debug.Log($"[{gameObject.name}] Объект выбран (по умолчанию)");
    }
}
