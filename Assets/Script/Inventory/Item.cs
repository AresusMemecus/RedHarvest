using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    [System.NonSerialized] // Спрайты не сериализуются в JSON
    public Sprite icon; 
    public string iconPath; // Путь к спрайту
    public bool isStackable;
    public int maxStackSize = 1;
    public int currentStackSize = 1;
    public ItemType itemType;

    public enum ItemType
    {
        Consumable,
        Weapon,
        Armor,
        Quest,
        Material
    }

    public Item(string name, string desc, string iconPath = "", bool stackable = false, int maxStack = 1, ItemType type = ItemType.Consumable)
    {
        itemName = name;
        description = desc;
        this.iconPath = iconPath;
        if (!string.IsNullOrEmpty(iconPath))
        {
            icon = Resources.Load<Sprite>(iconPath);
        }
        isStackable = stackable;
        maxStackSize = maxStack;
        itemType = type;
    }

    public void LoadIcon()
    {
        if (string.IsNullOrEmpty(iconPath))
        {
            Debug.LogWarning($"[LoadIcon] Путь до иконки не задан для предмета '{itemName}'!");
            return;
        }

        if (icon != null)
        {
            Debug.Log($"[LoadIcon] Иконка для '{itemName}' уже загружена, повторная загрузка не требуется.");
            return;
        }

        icon = Resources.Load<Sprite>(iconPath);

        if (icon != null)
        {
            Debug.Log($"[LoadIcon] Иконка успешно загружена для '{itemName}' по пути: Resources/{iconPath}");
        }
        else
        {
            Debug.LogError($"[LoadIcon] Не удалось загрузить иконку для '{itemName}' по пути: Resources/{iconPath}");
        }
    }

} 