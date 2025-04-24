using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public List<ItemData> items = new List<ItemData>();

    [System.Serializable]
    public class ItemData 
    {
        public string itemName;
        public string description;
        public string iconPath;
        public bool isStackable;
        public int maxStackSize;
        public int currentStackSize;
        public Item.ItemType itemType;

        public ItemData(Item item)
        {
            itemName = item.itemName;
            description = item.description;
            iconPath = item.iconPath;
            isStackable = item.isStackable;
            maxStackSize = item.maxStackSize;
            currentStackSize = item.currentStackSize;
            itemType = item.itemType;
        }

        public Item ToItem()
        {
            Item item = new Item(itemName, description, iconPath, isStackable, maxStackSize, itemType);
            item.currentStackSize = currentStackSize;
            return item;
        }
    }
} 