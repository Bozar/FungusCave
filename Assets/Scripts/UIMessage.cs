using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIMessage : MonoBehaviour, IUpdateUI
    {
        private UIText getUI;
        private int maxHeight;
        private UITag[] sortedTags;

        private delegate Text UIText(UITag tag);

        public void PrintStaticText()
        {
            return;
        }

        public void PrintStaticText(string text)
        {
            return;
        }

        public void PrintText()
        {
            string[] msg = GetComponent<GameMessage>().GetMessageText(maxHeight);

            for (int i = 0; i < sortedTags.Length; i++)
            {
                if (i < msg.Length)
                {
                    getUI(sortedTags[i]).text = msg[i];
                }
                else
                {
                    getUI(sortedTags[i]).text = "";
                }
            }
        }

        // Use GameMessage.StoreText() when possible. This method is reserved for
        // backward compatibility.
        public void StoreText(string text)
        {
            GetComponent<GameMessage>().StoreText(text);
        }

        private void Awake()
        {
            maxHeight = 5;
        }

        private void Start()
        {
            getUI = FindObjects.GetUIText;

            sortedTags = new UITag[]
            {
                UITag.Message1, UITag.Message2, UITag.Message3,
                UITag.Message4, UITag.Message5
            };
        }
    }
}
