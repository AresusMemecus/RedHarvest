using TMPro;
using UnityEngine;
using System.Collections;

public class Box : SelectableBase, IInteractable
{
    public Inventory inventory;
    public Door door;

    public void Interact(){}

    public override void OnSelect()
    {
        if (inventory.HasItemByName("Пропуск"))
        { 
            door.isActivated = true;

            ReplicaData data = ReplicaLoader.LoadReplica("Replics/boxActivateDoor");

            if (data != null)
            {
                SetReplicaLines(data.replica);
                Replica("Talk");
            }
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
