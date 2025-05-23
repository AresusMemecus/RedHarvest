using TMPro;
using UnityEngine;
using System.Collections;

public class Objectsmt : SelectableBase, IInteractable
{
    Item objectSmt;
    public Inventory inventory;

    public void Interact(){}

    public void Start()
    {
        objectSmt = new Item("Что-то", "тикет", "Sprites/Object", false, 1, 1, Item.ItemType.Armor);
        
    }
    public override void OnSelect()
    {
        pickItem();
    }
    
    public void pickItem()
    {
        inventory.AddItem(objectSmt);
        Destroy(gameObject);
    }
}
