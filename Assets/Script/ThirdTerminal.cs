using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ThirdTerminal : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public GridLayoutGroup optionsGrid; // <-- объект с GridLayoutGroup
    public GameObject optionButtonPrefab;
    public GameObject DenyMark, CloseLock, AcceptMark, OpenLock;
    public FillInTheBlankQuestion question;

    private List<string> currentAnswers = new List<string>();
    private string[] splitText;
    public bool isActivated = false;

    void Start()
    {
        SetupQuestion();
    }

    void SetupQuestion()
    {
        currentAnswers.Clear();
        foreach (Transform child in optionsGrid.transform)
        {
            Destroy(child.gameObject);
        }


        // Разделяем текст по пропускам
        splitText = question.baseText.Split(new string[] { "___" }, System.StringSplitOptions.None);
        currentAnswers = new List<string>(new string[question.correctWords.Length]);

        // Слова, включая ложные (можешь добавить свои)
        List<string> allWords = new List<string>(question.correctWords);
        // TODO: добавить ложные слова, если нужно
        Shuffle(allWords);

        // Создаём кнопки для выбора слов
        foreach (string word in allWords)
        {
            GameObject btnObj = Instantiate(optionButtonPrefab, optionsGrid.transform);
            btnObj.GetComponentInChildren<TextMeshProUGUI>().text = word;
            btnObj.GetComponent<Button>().onClick.AddListener(() => OnWordSelected(word));
        }

        UpdateQuestionText();
    }

    void OnWordSelected(string word)
    {
        int indexToFill = currentAnswers.FindIndex(a => string.IsNullOrEmpty(a));
        if (indexToFill != -1)
        {
            currentAnswers[indexToFill] = word;
            UpdateQuestionText();

            // Автоматическая проверка, если все слова заполнены
            if (!currentAnswers.Contains(null) && !currentAnswers.Contains(""))
            {
                CheckAnswer();
            }
        }
    }

    void UpdateQuestionText()
    {
        string fullText = "";
        for (int i = 0; i < splitText.Length; i++)
        {
            fullText += splitText[i];
            if (i < currentAnswers.Count)
            {
                fullText += $"<u>{(string.IsNullOrEmpty(currentAnswers[i]) ? "___" : currentAnswers[i])}</u>";
            }
        }

        questionText.text = fullText;
    }

    void CheckAnswer()
    {
        for (int i = 0; i < currentAnswers.Count; i++)
        {
            if (currentAnswers[i] != question.correctWords[i])
            {
                ResetAnswers();
                return;
            }
        }

        Activate();
        // Можешь тут вызвать анимации, переход, открыть замок и т.д.
    }

    void Activate()
    {
        DenyMark.SetActive(false);
        CloseLock.SetActive(false);
        AcceptMark.SetActive(true);
        OpenLock.SetActive(true);
        isActivated = true;
    }

    void ResetAnswers()
    {
        for (int i = 0; i < currentAnswers.Count; i++)
        {
            currentAnswers[i] = "";
        }
        UpdateQuestionText();
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
