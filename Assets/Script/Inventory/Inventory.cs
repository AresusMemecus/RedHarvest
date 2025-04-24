using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxInventorySize = 20;
    private List<Item> items = new List<Item>();
    private string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        LoadInventory();
    }

    private void OnDestroy()
    {
        SaveInventory();
    } 

    public void SaveInventory()
    {
        var inventoryData = new InventoryData();
        foreach (var item in items)
        {
            inventoryData.items.Add(new InventoryData.ItemData(item));
        }

        string json = JsonUtility.ToJson(inventoryData, true);
        File.WriteAllText(savePath, json);
        Debug.Log($"Инвентарь сохранен в {savePath}");
    }

    public void LoadInventory()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);
            
            items.Clear();
            foreach (var itemData in inventoryData.items)
            {
                items.Add(itemData.ToItem());
            }
            Debug.Log("Инвентарь загружен");
        }
        else
        {
            Debug.Log("Файл инвентаря не найден, создан новый инвентарь");
        }
    }

    public bool AddItem(Item item)
    {
        if (items.Count >= maxInventorySize && !HasStackableItem(item))
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        if (item.isStackable)
        {
            Item existingItem = items.FirstOrDefault(i => i.itemName == item.itemName);
            if (existingItem != null)
            {
                if (existingItem.currentStackSize < existingItem.maxStackSize)
                {
                    existingItem.currentStackSize++;
                    SaveInventory(); // Сохраняем после изменения
                    return true;
                }
                else
                {
                    Debug.Log("Stack is full!");
                    return false;
                }
            }
        }

        items.Add(item);
        SaveInventory(); // Сохраняем после изменения
        return true;
    }

    public bool RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            if (item.isStackable && item.currentStackSize > 1)
            {
                item.currentStackSize--;
            }
            else
            {
                items.Remove(item);
            }
            SaveInventory(); // Сохраняем после изменения
            return true;
        }
        return false;
    }

    public bool UseItem(Item item)
    {
        if (!items.Contains(item))
        {
            Debug.Log("Item not found in inventory!");
            return false;
        }

        // Здесь можно добавить специфическую логику использования предмета
        switch (item.itemType)
        {
            case Item.ItemType.Consumable:
                // Добавить здоровье, ману и т.д.
                break;
            case Item.ItemType.Weapon:
                // Экипировать оружие
                break;
            case Item.ItemType.Armor:
                // Экипировать броню
                break;
            case Item.ItemType.Quest:
                // Обработать квестовый предмет
                break;
            case Item.ItemType.Material:
                // Обработать материал для крафта
                break;
        }

        return RemoveItem(item);
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }

    public bool HasItemByName(string itemName)
    {
        return items.Any(i => i.itemName == itemName);
    }

    private bool HasStackableItem(Item item)
    {
        if (!item.isStackable) return false;
        return items.Any(i => i.itemName == item.itemName && i.currentStackSize < i.maxStackSize);
    }

    public List<Item> GetItems()
    {
        return new List<Item>(items);
    }

    public int GetItemCount()
    {
        return items.Count;
    }

    public int GetMaxSize()
    {
        return maxInventorySize;
    }
}
