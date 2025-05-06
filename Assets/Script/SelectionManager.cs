using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionManager : MonoBehaviour
{
    private SelectableBase lastHoveredSelectable;
    public LayerMask ignoreLayer; // Слой, который нужно игнорировать

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
        RaycastHit hit;

        // Выполняем рейкаст с фильтрацией по слоям
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayer))
        {
            SelectableBase selectable = hit.collider.GetComponent<SelectableBase>();

            if (selectable != null)
            {
                if (lastHoveredSelectable != selectable)
                {
                    if (lastHoveredSelectable != null)
                    {
                        lastHoveredSelectable.OnHoverExit();
                    }

                    selectable.OnHoverEnter();
                    lastHoveredSelectable = selectable;
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    selectable.OnSelect();
                }
            }
            else
            {
                if (lastHoveredSelectable != null)
                {
                    lastHoveredSelectable.OnHoverExit();
                    lastHoveredSelectable = null;
                }
            }
        }
        else
        {
            if (lastHoveredSelectable != null)
            {
                lastHoveredSelectable.OnHoverExit();
                lastHoveredSelectable = null;
            }
        }
    }
}
