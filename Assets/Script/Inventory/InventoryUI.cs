using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Linq;


public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;

    private Inventory inventory;
    private List<GameObject> itemSlots = new List<GameObject>();
    private Item selectedItem;

    private void Awake()
    {
        // Ищем инвентарь
        inventory = FindFirstObjectByType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory component not found!");
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        // Подписываемся на событие обновления инвентаря
        if (inventory != null)
        {
            UpdateInventoryUI();
        }
    }

    public void ToggleInventory()
    {
        if (inventoryPanel == null) return;

        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
        {
            UpdateInventoryUI();
        }
    }

    private void UpdateInventoryUI()
    {
        if (inventory == null || itemsContainer == null || itemSlotPrefab == null) return;

        // Очищаем старые слоты
        foreach (var slot in itemSlots)
        {
            if (slot != null)
            {
                Destroy(slot);
            }
        }
        itemSlots.Clear();

        // Создаем новые слоты
        foreach (var item in inventory.GetItems())
        {
            if (item == null) continue;

            GameObject slot = Instantiate(itemSlotPrefab, itemsContainer);
            if (slot == null) continue;

            itemSlots.Add(slot);

            // Настраиваем слот
            SetupItemSlot(slot, item);
        }
    }

    private void SetupItemSlot(GameObject slot, Item item)
    {
        // Получаем все необходимые компоненты
        Image itemIcon = slot.GetComponentsInChildren<Image>(true)
        .FirstOrDefault(img => img.gameObject.name == "itemIcon");
        TextMeshProUGUI itemCount = slot.GetComponentsInChildren<TextMeshProUGUI>(true)
        .FirstOrDefault(text => text.gameObject.name == "itemCount");
        Button slotButton = slot.GetComponent<Button>();
        // Настраиваем иконку
        if (itemIcon != null)
        {
                itemIcon.sprite = item.icon;
                itemIcon.enabled = true;
        }
        else
        {
            Debug.LogWarning($"[SetupItemSlot] Не найден компонент Image в слоте: {slot.name}");
        }


        // Настраиваем количество
        if (itemCount != null)
        {
            if (item.isStackable)
            {
                itemCount.text = item.currentStackSize.ToString();
                itemCount.gameObject.SetActive(true);
            }
            else
            {
                itemCount.gameObject.SetActive(false);
            }
        }

        // Настраиваем кнопку
        if (slotButton != null)
        {
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => SelectItem(item));
        }

        // Добавляем текст с названием предмета
        TextMeshProUGUI itemName = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (itemName != null)
        {
            itemName.text = item.itemName;
        }
    }

    private void SelectItem(Item item)
    {
        if (item == null || itemNameText == null || itemDescriptionText == null) return;

        selectedItem = item;
        itemNameText.text = item.itemName;
        itemDescriptionText.text = item.description;
    }

    public void UseSelectedItem()
    {
        if (selectedItem == null || inventory == null) return;

        inventory.UseItem(selectedItem);
        UpdateInventoryUI();
    }

    public void DropSelectedItem()
    {
        if (selectedItem == null || inventory == null) return;

        inventory.RemoveItem(selectedItem);
        UpdateInventoryUI();
    }
}