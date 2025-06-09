using UnityEngine;

[System.Serializable]
public class FillInTheBlankQuestion
{
    [TextArea] public string baseText; // пример: "The ___ fox jumps over the ___ dog."
    public string[] correctWords;      // ["quick", "lazy"]
}
