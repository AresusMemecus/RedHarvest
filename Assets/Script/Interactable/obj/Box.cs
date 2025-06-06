using TMPro;
using UnityEngine;
using System.Collections;

public class Box : SelectableBase, IInteractable
{
    public Inventory inventory;
    public GameObject terminal; 

    public void Interact() { }

    public override void OnSelect()
    {
        if (inventory.HasItemByName("Пропуск"))
        {
            terminal.SetActive(true);
            Freez();
        }
        else
        {
            ReplicaData data = ReplicaLoader.LoadReplica("Replics/box");

            if (data != null)
            {
                SetReplicaLines(data.replica);
                Replica("Talk");
            }
        }
    }
}
