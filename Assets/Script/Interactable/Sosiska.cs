using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Sosiska : SelectableBase, IInteractable
{
    [SerializeField] private string spritePath = "Sprites/Sosiska"; // Путь к спрайту в папке Resources
    private Item item;

    private void Awake()
    {
        // Создаем предмет с путем к иконке
        item = new Item("sosiska", "Вкусная сосиска", spritePath, true, 10, 1, Item.ItemType.Consumable);
    }
    
    public void Interact()
    {
        Debug.Log("Интеракция с сосиской выполнена!");
        
        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>();
        if (dialogueManager != null)
        {
            dialogueManager.StartDialogue("greeting");
        }
        
        Inventory inventory = FindFirstObjectByType<Inventory>();
        if (inventory != null)
        {
            bool added = inventory.AddItem(item);
            if (added)
            {
                Debug.Log("Сосиска добавлена в инвентарь!");
            }
            else
            {
                Debug.Log("Не удалось добавить сосиску в инвентарь!");
            }
        }
    }
}
