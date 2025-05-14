using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerText; // Имя говорящего
    public TextMeshProUGUI dialogueText; // Текст фразы
    public GameObject dialogueUI; // Весь UI диалога (панель)
    public Image speakerImageForFirstPerson; // Изображение говорящего
    public Image speakerImageForSecondPerson; // Изображение говорящего
    public GameObject HUD;
    double spriteDarknessForFirstPerson;
    double spriteDarknessForSecondPerson;

    private Dialogue currentDialogue;
    private int currentLineIndex;

    public bool isDialogueActive = false;

    private PlayerController PlayerController;

    
    void Start()
    {
        PlayerController = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (isDialogueActive && (Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.enterKey.wasPressedThisFrame))
        {
            NextLine();
        }
    }

    // Метод запуска диалога
    public void StartDialogue(string dialogueId)
    {
        Time.timeScale = 0f;

        HUD.SetActive(false);

        TextAsset jsonFile = Resources.Load<TextAsset>($"Dialogs/{dialogueId}");
        if (jsonFile == null)
        {
            Debug.LogError($"Диалог {dialogueId} не найден в папке Resources/Dialogs!");
            return;
        }

        Dialogue dialogue = JsonUtility.FromJson<Dialogue>(jsonFile.text);
        
        if (dialogue == null || dialogue.lines == null || dialogue.lines.Length == 0)
        {
            Debug.LogError($"Диалог {dialogueId} содержит ошибки или пустые строки.");
            return;
        }

        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialogueUI.SetActive(true);
        isDialogueActive = true;

        ShowLine(); 
    }

    public void ShowLine()
    {
        if (currentLineIndex < currentDialogue.lines.Length)
        {
            DialogueLine line = currentDialogue.lines[currentLineIndex];
            speakerText.text = line.speaker;
            dialogueText.text = line.text;
            spriteDarknessForFirstPerson = line.spriteDarknessForFirstPerson;
            spriteDarknessForSecondPerson = line.spriteDarknessForSecondPerson;

            // Загружаем спрайты
            Sprite firstPerson = Resources.Load<Sprite>($"Sprites/{line.spriteNameForFirstPerson}");
            Sprite secondPerson = null;

            // Проверяем, если имя спрайта для второго персонажа не пустое или null
            if (!string.IsNullOrEmpty(line.spriteNameForSecondPerson))
            {
                secondPerson = Resources.Load<Sprite>($"Sprites/{line.spriteNameForSecondPerson}");
            }

            // Устанавливаем спрайт для первого персонажа
            speakerImageForFirstPerson.sprite = firstPerson;
            ApplyDarkness(speakerImageForFirstPerson, spriteDarknessForFirstPerson);

            // Применяем затемнение для второго персонажа
            if (secondPerson != null)
            {
                speakerImageForSecondPerson.sprite = secondPerson;
                speakerImageForSecondPerson.gameObject.SetActive(true); // Показываем второй спрайт, если он есть
                ApplyDarkness(speakerImageForSecondPerson, spriteDarknessForSecondPerson);
            }
            else
            {
                speakerImageForSecondPerson.gameObject.SetActive(false); // Скрываем второй спрайт, если его нет
            }

            // Поворот первого персонажа (если нужно)
            speakerImageForFirstPerson.rectTransform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            EndDialogue();
        }
    }

    private void ApplyDarkness(Image image, double darkness)
    {
        float value = Mathf.Clamp01((float)darkness);
        image.color = new Color(value, value, value, 1f); // RGB затемнение, прозрачность не трогаем
    }

    // Перейти к следующей строке
    private void NextLine()
    {
        currentLineIndex++;
        ShowLine();
    }

    // Завершение диалога
    private void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);
        HUD.SetActive(true);
        Time.timeScale = 1f;
        PlayerController.GetComponent<PlayerController>().ResetMovement(); 

        Debug.Log("Диалог закончен!");
    }
}
