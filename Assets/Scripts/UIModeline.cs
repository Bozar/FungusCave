using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIModeline : MonoBehaviour, IUpdateUI
    {
        private UIDict getUI;
        private Queue<string> inputText;
        private string newLine;

        private delegate GameObject UIDict(UITag tag);

        public void PrintStaticText()
        {
            PrintStaticText("");
        }

        public void PrintStaticText(string text)
        {
            StoreText(text);
            CheckLineCount();

            getUI(UITag.Modeline).GetComponent<Text>().text = newLine;
        }

        public void PrintText()
        {
            return;
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

            newLine = inputText.Count > 0 ? inputText.Dequeue() : "";
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
}
