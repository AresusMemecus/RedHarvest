using UnityEngine;

public class Sosiska : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Интеракция с сосиской1 выполнена!");
        FindObjectOfType<DialogueManager>().StartDialogue("greeting");


    }
}
