using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UILog : MonoBehaviour, IUpdateUI
    {
        private UIObject getUI;
        private UITag[] tags;

        private delegate GameObject UIObject(UITag tag);

        public int LogHeight { get; private set; }

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
            string[] log = GetComponent<GameMessage>().GetText(LogHeight);

            for (int i = 0; i < tags.Length; i++)
            {
                if (i < log.Length)
                {
                    getUI(tags[i]).GetComponent<Text>().text = log[i];
                }
                else
                {
                    getUI(tags[i]).GetComponent<Text>().text = "";
                }
            }
        }

        private void Awake()
        {
            LogHeight = 18;
        }

        private void Start()
        {
            getUI = FindObjects.GetUIObject;

            tags = new UITag[]
            {
                UITag.Log1, UITag.Log2, UITag.Log3, UITag.Log4, UITag.Log5,
                UITag.Log6, UITag.Log7, UITag.Log8, UITag.Log9, UITag.Log10,
                UITag.Log11, UITag.Log12, UITag.Log13, UITag.Log14, UITag.Log15,
                UITag.Log16, UITag.Log17, UITag.Log18
            };
        }
    }
}
