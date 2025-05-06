using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ActionTrigger : MonoBehaviour
{
    public Material outlineMaterial;
    private Collider previousClosestCollider = null;
    private Material previousMaterial = null;
    private bool isPlayerInRange = false;
    private IInteractable currentInteractable = null;
    private Collider closestCollider = null;
    private List<Collider> interactablesInRange = new List<Collider>();

    void Update()
    {
        // Очищаем список от уничтоженных объектов
        interactablesInRange.RemoveAll(col => col == null);

        if (interactablesInRange.Count > 0)
        {
            UpdateClosestInteractable();
        }
        else
        {
            currentInteractable = null;
            closestCollider = null;
            isPlayerInRange = false;
        }

        if (isPlayerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable?.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other != null && other.TryGetComponent(out IInteractable interactable))
        {
            interactablesInRange.Add(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other != null && interactablesInRange.Contains(other))
        {
            interactablesInRange.Remove(other);
        }
    }

    void UpdateClosestInteractable()
    {
        float closestDistance = float.MaxValue;
        Collider newClosest = null;

        foreach (var col in interactablesInRange)
        {
            if (col == null) continue;

            float distance = Vector3.Distance(transform.position, col.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                newClosest = col;
            }
        }

        // Если ближайший объект изменился
        if (newClosest != closestCollider)
        {
            // Вернуть старому объекту его оригинальный материал
            if (previousClosestCollider != null && previousMaterial != null)
            {
                var renderer = previousClosestCollider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = previousMaterial;
                }
            }

            previousClosestCollider = newClosest;
            closestCollider = newClosest;

            if (closestCollider != null && closestCollider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                isPlayerInRange = true;
            }
            else
            {
                currentInteractable = null;
                isPlayerInRange = false;
            }
        }
    }
}
