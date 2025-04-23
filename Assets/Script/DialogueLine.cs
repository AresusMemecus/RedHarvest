using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    public string speaker; // Кто говорит
    public string text;    // Что говорит
    public string spriteName; // Имя спрайта (путь в папке Resources)
}

[Serializable] 
public class Dialogue
{
    public string dialogueId; // Идентификатор диалога (на всякий случай)
    public DialogueLine[] lines; // Массив всех реплик
}
