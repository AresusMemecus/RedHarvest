using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class Door : SelectableBase, IInteractable
{
    public bool isActivated = false;
    public Vector3 Teleport;
    
    public void Interact() { }

    public override void OnSelect()
    {
        if (isActivated)
        {
            ReplicaData data = ReplicaLoader.LoadReplica("Replics/doorOpen");

            if (data != null)
            {
                SetReplicaLines(data.replica);
                Replica("Talk");
            }
        }
        else
        {
            ReplicaData data = ReplicaLoader.LoadReplica("Replics/doorNotOpen");

            if (data != null)
            {
                SetReplicaLines(data.replica);
                Replica("Talk");
            }
        }
    }

    public override void OnReplicaComplete()
    {
        CharacterController character = Charachter.GetComponent<CharacterController>();
        if (isActivated)
        {
            character.enabled = false;
            Charachter.transform.position = Teleport;
            character.enabled = true;
        }
    }
}
