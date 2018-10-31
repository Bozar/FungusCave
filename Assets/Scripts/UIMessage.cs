using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    private int accumulatedHeight;
    private Stack<int> checkLength;
    private Stack<string> checkText;
    private Queue<int> inputLength;
    private Queue<string> inputText;
    private int lineHeight;
    private int lineLength;
    private int maxHeight;
    private int maxWidth;
    private string newLine;
    private Queue<string> outputText;

    public void PrintText()
    {
        CheckLineCount();
        CheckLineHeight();

        FindObjects.MainUIDict[(int)UITags.Message].
            GetComponent<Text>().text = "";

        while (outputText.Count > 0)
        {
            newLine = outputText.Dequeue();
            if (outputText.Count > 0)
            {
                newLine += "\n";
            }

            FindObjects.MainUIDict[(int)UITags.Message].
                GetComponent<Text>().text += newLine;
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
        maxHeight = 6;
        maxWidth = 53;

        inputLength = new Queue<int>();
        inputText = new Queue<string>();
        checkLength = new Stack<int>();
        checkText = new Stack<string>();
        outputText = new Queue<string>();
    }

    private void CheckLineCount()
    {
        while (inputText.Count > 0)
        {
            if (inputText.Count > maxHeight)
            {
                inputText.Dequeue();
                inputLength.Dequeue();
            }
            else
            {
                checkText.Push(inputText.Dequeue());
                checkLength.Push(inputLength.Dequeue());
            }
        }
    }

    private void CheckLineHeight()
    {
        accumulatedHeight = 0;

        while (checkLength.Count > 0)
        {
            lineLength = checkLength.Pop();
            newLine = checkText.Pop();
            lineHeight = (int)System.Math.Ceiling((float)lineLength / maxWidth);

            if (accumulatedHeight + lineHeight > maxHeight)
            {
                checkLength.Clear();
                checkText.Clear();
                break;
            }

            accumulatedHeight += lineHeight;
            outputText.Enqueue(newLine);

            inputLength.Enqueue(lineLength);
            inputText.Enqueue(newLine);
        }

        outputText = new Queue<string>(outputText.Reverse());

        inputText = new Queue<string>(inputText.Reverse());
        inputLength = new Queue<int>(inputLength.Reverse());
    }
}
