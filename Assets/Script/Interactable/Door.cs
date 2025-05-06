using JetBrains.Annotations;
using UnityEngine;

public class Door : SelectableBase, IInteractable
{
    public GameObject Charachter;
    public Vector3 TeleportTo;
    public void Interact()
    {
        var controller = Charachter.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;  // отключаем контроллер, чтобы разрешить изменение позиции
            Charachter.transform.position = TeleportTo;
            controller.enabled = true;   // включаем обратно
        }
        else
        {
            Charachter.transform.position = TeleportTo;
        }

    }
}
