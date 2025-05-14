using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Сutscenes : MonoBehaviour
{

    public GameObject HUD;
    public GameObject BlackScreen;
    public GameObject Charachter;
    public GameObject Manager;
    public DialogueManager dialogue;
    private Image blackScreenImage;

    void Start()
    {
        StartCoroutine(startScene());
    }

    IEnumerator startScene()
    {
        HUD.SetActive(false);
        blackScreenImage = BlackScreen.GetComponentInChildren<Image>();
        BlackScreen.SetActive(true);
        
        var animator = Charachter.GetComponent<Animator>();
        var characterController = Charachter.GetComponent<CharacterController>();
        var playerController = Charachter.GetComponent<PlayerController>();
        var SelectionManager = Manager.GetComponent<SelectionManager>();
        
        characterController.enabled = false;
        playerController.isFrozen = true;
        animator.Play("Laying");
        SelectionManager.enabled = false;

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(BlackScreenFading(false, 3f));

        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(BlackScreenFading(true, 2f));

        yield return new WaitForSeconds(2f);

        animator.Play("Sit");

        yield return StartCoroutine(BlackScreenFading(false, 3f));

        yield return new WaitForSeconds(1f);

        dialogue.StartDialogue("Guide");
        SelectionManager.enabled = true;

    }

    IEnumerator BlackScreenFading(bool fadeOut, float duration)
    {
        float elapsed = 0f;
        Color color = blackScreenImage.color;
        float startAlpha = color.a;

        // Определяем целевое значение альфы
        float targetAlpha = fadeOut ? 1f : 0f;  // Если fadeOut true - альфа = 1 (полностью затемнить), иначе 0 (убрать затемнение)

        // Плавно изменяем альфу
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            blackScreenImage.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        // Убедимся, что альфа точно установлена в целевое значение
        blackScreenImage.color = new Color(color.r, color.g, color.b, targetAlpha);
    }


    public void SetBlackScreenAlpha(float alpha)
    {
        if (blackScreenImage != null)
        {
            Color color = blackScreenImage.color;
            color.a = Mathf.Clamp01(alpha);
            blackScreenImage.color = color;
        }
    }
}
