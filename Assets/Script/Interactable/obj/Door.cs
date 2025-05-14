using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Door : SelectableBase, IInteractable
{
    public bool isActivated = false;

    public void Interact(){}

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
        if (isActivated)
        {
            SceneManager.LoadScene("CoreScene");
        }
    }
}
