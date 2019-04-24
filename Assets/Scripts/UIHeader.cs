using Fungus.GameSystem.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIHeader : MonoBehaviour, IUpdateUI
    {
        private UIObject getUI;

        private delegate GameObject UIObject(UITag tag);

        public SubModeUITag[] SortedHeader { get; private set; }

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
            string[] header = new string[SortedHeader.Length];
            string joined;

            // Change text color. Current mode is white. Others are grey.
            for (int i = 0; i < SortedHeader.Length; i++)
            {
                if (GetComponent<HeaderStatus>().CurrentModeName
                    == SortedHeader[i])
                {
                    header[i] = GetSubModeName(SortedHeader[i]);
                }
                else
                {
                    header[i] = GetComponent<GameColor>().GetColorfulText(
                        GetSubModeName(SortedHeader[i]), ColorName.Grey);
                }
            }

            joined = string.Join(" | ", header);
            joined = "[ " + joined + " ]";
            getUI(UITag.SubModeHeader).GetComponent<Text>().text = joined;
        }

        private string GetSubModeName(SubModeUITag sm)
        {
            return GetComponent<GameText>().GetStringData("SubModeHeader", sm);
        }

        private void Start()
        {
            getUI = FindObjects.GetUIObject;

            SortedHeader = new SubModeUITag[]
            {
                SubModeUITag.Power,
                SubModeUITag.Log,
                SubModeUITag.Help,
                SubModeUITag.Setting
            };
        }
    }
}
