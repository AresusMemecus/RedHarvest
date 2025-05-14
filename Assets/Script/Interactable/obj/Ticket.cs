using TMPro;
using UnityEngine;
using System.Collections;

public class Ticket : SelectableBase, IInteractable
{
    Item ticket;
    public Inventory inventory;

    public void Interact(){}

    public void Start()
    {
        ticket = new Item("Пропуск", "тикет", "Sprites/Ticket", false, 1, 1, Item.ItemType.Armor);
        
    }
    public override void OnSelect()
    {
        ReplicaData data = ReplicaLoader.LoadReplica("Replics/ticket");

        if (data != null)
        {
            SetReplicaLines(data.replica);
            Replica("Talk");
        }
    }
    public override void OnReplicaComplete(){
        inventory.AddItem(ticket);
        Destroy(gameObject);
    }
}
