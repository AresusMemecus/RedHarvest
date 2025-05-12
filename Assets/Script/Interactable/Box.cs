using TMPro;
using UnityEngine;
using UnityEditor.UI;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Collections;

public class Box : SelectableBase
{
    public GameObject PopUpCloud;
    public GameObject Text;
    public GameObject Charachter;

    public override void OnSelect(){
        Interaction();
    }

    public void Interaction(){
        if (PopUpCloud == null) return;
        bool isActive = !PopUpCloud.activeSelf;
        PopUpCloud.SetActive(isActive);
        

        var animator = Charachter.GetComponent<Animator>();
        var characterController = Charachter.GetComponent<CharacterController>();
        var playerController = Charachter.GetComponent<PlayerController>();
        TextMeshProUGUI dialuge = Text.GetComponent<TextMeshProUGUI>();

        characterController.enabled = false;
        playerController.isFrozen = true;

        dialuge.text = "ХУЙ";
        animator.Play("Talk");

        // Запускаем возврат через 3 реальных секунды
        StartCoroutine(UnfreezeAfterSeconds(3f));
    }

private IEnumerator UnfreezeAfterSeconds(float seconds)
{
    TextMeshProUGUI dialuge = Text.GetComponent<TextMeshProUGUI>();

    // ждём 1 секунду
    yield return new WaitForSecondsRealtime(1f);
    dialuge.text = "ПИЗДА";

    // ждём ещё 1 секунду (итого 2 секунды)
    yield return new WaitForSecondsRealtime(1f);
    dialuge.text = "ХУЙНЯ";

    // ждём последнюю 1 секунду (итого 3 секунды)
    yield return new WaitForSecondsRealtime(1f);


    var animator = Charachter.GetComponent<Animator>();
    var characterController = Charachter.GetComponent<CharacterController>();
    var playerController = Charachter.GetComponent<PlayerController>();

    animator.Play("Walk");
    PopUpCloud.SetActive(false);

    characterController.enabled = true;
    playerController.isFrozen = false;

    animator.enabled = true; // если ты ранее выключал animator.enabled
}

}
