using System;
using UnityEngine;

[Serializable]
public class DialogueLine
{
    public string speaker; // Кто говорит
    public string text;    // Что говорит
    public string spriteNameForFirstPerson; // Имя спрайта (путь в папке Resources)
    public string spriteNameForSecondPerson;
    public double spriteDarknessForFirstPerson; // Имя спрайта (путь в папке Resources)
    public double spriteDarknessForSecondPerson;
}

[Serializable] 
public class Dialogue
{
    public string dialogueId; // Идентификатор диалога (на всякий случай)
    public DialogueLine[] lines; // Массив всех реплик
}
