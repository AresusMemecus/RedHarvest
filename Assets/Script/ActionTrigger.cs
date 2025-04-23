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
        if (other.TryGetComponent(out IInteractable interactable))
        {
            interactablesInRange.Add(other);

        }
    }

void OnTriggerExit(Collider other)
{
    if (interactablesInRange.Contains(other))
    {
        interactablesInRange.Remove(other);

        // Если вышли из объекта, который был ближайшим
        if (other == closestCollider)
        {
            if (previousMaterial != null)
            {
                Renderer renderer = other.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = previousMaterial;
                }
            }

            previousClosestCollider = null;
            closestCollider = null;
            previousMaterial = null;
            currentInteractable = null;
            isPlayerInRange = false;
        }
    }
}


    void UpdateClosestInteractable()
    {
        float closestDistance = float.MaxValue;
        Collider newClosest = null;

        foreach (var col in interactablesInRange)
        {
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
                previousClosestCollider.GetComponent<Renderer>().material = previousMaterial;
            }

            // Сохранить текущий материал нового объекта
            if (newClosest != null)
            {
                Renderer renderer = newClosest.GetComponent<Renderer>();
                if (renderer != null)
                {
                    previousMaterial = renderer.material;
                    renderer.material = outlineMaterial;
                }
            }

            previousClosestCollider = newClosest;
            closestCollider = newClosest;

            if (closestCollider != null && closestCollider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                isPlayerInRange = true;
                Debug.Log($"Ближайший интеракт обновлён: {closestCollider.name}");
            }
        }
    }

}
