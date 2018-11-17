using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIModeline : MonoBehaviour
{
    private UIDict getUI;
    private Queue<string> inputText;
    private string newLine;

    private delegate GameObject UIDict(UITag tag);

    public void PrintText()
    {
        PrintText("");
    }

    public void PrintText(string text)
    {
        StoreText(text);
        CheckLineCount();

        getUI(UITag.Modeline).GetComponent<Text>().text = newLine;
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

    private void Start()
    {
        getUI = FindObjects.GetUIObject;
    }

    private void StoreText(string text)
    {
        inputText.Enqueue(text);
    }
}
