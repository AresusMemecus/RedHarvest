using UnityEngine;

public class Cheese : SelectableBase, IInteractable
{
    [SerializeField] private string spritePath = "Sprites/Cheese"; // Путь к спрайту в папке Resources
    private Item item;

    private void Awake()
    {
        // Создаем предмет с путем к иконке
        item = new Item("Сыр", "Кусок вкусного сыра", spritePath, true, 5, 1, Item.ItemType.Consumable);
    }
    
    public void Interact()
    {
        Debug.Log("Интеракция с сыром выполнена!");
        
        Inventory inventory = FindFirstObjectByType<Inventory>();
        if (inventory != null)
        {
            bool added = inventory.AddItem(item);
            if (added)
            {
                Debug.Log("Сыр добавлен в инвентарь!");
            }
            else
            {
                Debug.Log("Не удалось добавить сыр в инвентарь!");
            }
        }
        Destroy(gameObject);
    }
}
