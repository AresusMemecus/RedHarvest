using TMPro;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class Cloth : SelectableBase, IInteractable
{
    Item cloth;
    public Inventory inventory;

    public void Interact(){}

    public void Start()
    {
        cloth = new Item("Одежда", "Одежда", "Sprites/cloth", false, 1, 1, Item.ItemType.Armor);
        
    }

    public override void OnSelect()
    {
        ReplicaData data = ReplicaLoader.LoadReplica("Replics/cloth");

        if (data != null)
        {
            SetReplicaLines(data.replica);
            Replica("SitAndTalk");
        }
    }

    public override void OnReplicaComplete(){
        inventory.AddItem(cloth);
        Destroy(gameObject);
    }
}
