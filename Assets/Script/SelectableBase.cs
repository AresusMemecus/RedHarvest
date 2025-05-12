using UnityEngine;

public abstract class SelectableBase : MonoBehaviour
{
    private Material originalMaterial;
    [SerializeField] private Material Outlined;

    private bool meshRendererAdded = false; // Флаг, добавили ли MeshRenderer вручную

    public virtual void OnHoverEnter()
    {
        Debug.Log($"[{gameObject.name}] Наведен курсор (по умолчанию)");

        var spriteRenderer = GetComponent<SpriteRenderer>();
        var meshRenderer = GetComponent<MeshRenderer>();

        if (spriteRenderer != null && Outlined != null)
        {
            originalMaterial = spriteRenderer.material;
            spriteRenderer.material = Outlined;
        }
        else if (meshRenderer != null && Outlined != null)
        {
            originalMaterial = meshRenderer.material;
            meshRenderer.materials = new Material[] { originalMaterial, Outlined };
        }
        else
        {
            // Если нет ни SpriteRenderer, ни MeshRenderer — добавляем MeshRenderer вручную
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRendererAdded = true;
            originalMaterial = null; // так как его не было
            meshRenderer.material = Outlined;

            Debug.Log($"[{gameObject.name}] Добавлен временный MeshRenderer.");
        }
    }

    public virtual void OnHoverExit()
    {
        Debug.Log($"[{gameObject.name}] Курсор ушёл (по умолчанию)");

        var spriteRenderer = GetComponent<SpriteRenderer>();
        var meshRenderer = GetComponent<MeshRenderer>();

        if (spriteRenderer != null && originalMaterial != null)
        {
            spriteRenderer.material = originalMaterial;
        }
        else if (meshRenderer != null && originalMaterial != null && !meshRendererAdded)
        {
            meshRenderer.materials = new Material[] { originalMaterial };
        }

        if (meshRendererAdded && meshRenderer != null)
        {
            Destroy(meshRenderer);
            meshRendererAdded = false;
            Debug.Log($"[{gameObject.name}] Удалён временный MeshRenderer.");
        }
    }

    public virtual void OnSelect()
    {
        Debug.Log($"[{gameObject.name}] Объект выбран (по умолчанию)");
    }
}
