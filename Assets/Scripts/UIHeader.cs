using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIHeader : MonoBehaviour, IUpdateUI
    {
        private UIObject getUI;
        private Dictionary<SubModeUITag, string> subModeName;

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
                if (GetComponent<HeaderAction>().CurrentMode == SortedHeader[i])
                {
                    header[i] = subModeName[SortedHeader[i]];
                }
                else
                {
                    header[i] = GetComponent<GameColor>().GetColorfulText(
                        subModeName[SortedHeader[i]], ColorName.Grey);
                }
            }

            joined = string.Join(" | ", header);
            joined = "[ " + joined + " ]";
            getUI(UITag.SubModeHeader).GetComponent<Text>().text = joined;
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

            subModeName = new Dictionary<SubModeUITag, string>
            {
                { SubModeUITag.Power, "Power" },
                { SubModeUITag.Log, "Log" },
                { SubModeUITag.Help, "Help" },
                { SubModeUITag.Setting, "Setting" }
            };
        }
    }
}
