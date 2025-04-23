using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI speakerText; // Имя говорящего
    public TextMeshProUGUI dialogueText; // Текст фразы
    public GameObject dialogueUI; // Весь UI диалога (панель)
    public Image speakerImage; // Изображение говорящего

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

    // Показ текущей строки
    public void ShowLine()
    {
        if (currentLineIndex < currentDialogue.lines.Length)
        {
            DialogueLine line = currentDialogue.lines[currentLineIndex];
            speakerText.text = line.speaker;
            dialogueText.text = line.text;

            // Загружаем спрайт из папки Resources/Sprites/
            Sprite sprite = Resources.Load<Sprite>($"Sprites/{line.spriteName}");
            if (sprite != null)
            {
                speakerImage.sprite = sprite;
            }
            else
            {
                Debug.LogError($"Не удалось загрузить спрайт: {line.spriteName}");
            }
        }
        else
        {
            EndDialogue();
        }
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

        Time.timeScale = 1f;
        PlayerController.GetComponent<PlayerController>().ResetMovement(); 

        Debug.Log("Диалог закончен!");
    }
}
