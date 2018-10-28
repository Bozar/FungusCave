using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeline : MonoBehaviour
{
    private Queue<string> inputText;
    private string newLine;

    public void PrintText()
    {
        PrintText("");
    }

    public void PrintText(string text)
    {
        StoreText(text);
        CheckLineCount();

        FindObjects.MainUIDict[(int)FindObjects.UITags.Modeline].
            GetComponent<Text>().text = newLine;
    }

    private void Awake()
    {
        inputText = new Queue<string>();
    }

    private void CheckLineCount()
    {
        while (inputText.Count > 1)
        {
            inputText.Dequeue();
        }

        newLine = inputText.Count > 0
            ? inputText.Dequeue()
            : "";
    }

    private void StoreText(string text)
    {
        inputText.Enqueue(text);
    }
}
