using System.Collections.Generic;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    private Stack<int> checkLength;
    private Stack<string> checkText;
    private Queue<int> inputLength;
    private Queue<string> inputText;
    private Queue<string> outputText;

    public void PrintText()
    {
        outputText = inputText;

        while (outputText.Count > 0)
        {
            Debug.Log(outputText.Dequeue());
        }
    }

    public void StoreText(string text)
    {
        StoreText(text, text.Length);
    }

    public void StoreText(string text, int length)
    {
        inputText.Enqueue(text);
        inputLength.Enqueue(length);
    }

    private void Awake()
    {
        inputLength = new Queue<int>();
        inputText = new Queue<string>();
        checkLength = new Stack<int>();
        checkText = new Stack<string>();
        outputText = new Queue<string>();
    }
}
