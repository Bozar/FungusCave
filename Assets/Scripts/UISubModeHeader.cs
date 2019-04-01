using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UISubModeHeader : MonoBehaviour, IUpdateUI
    {
        private SubModeUITag currentMode;
        private UIObject getUI;
        private SubModeUITag[] sortedHeader;
        private Dictionary<SubModeUITag, string> subModeName;

        private delegate GameObject UIObject(UITag tag);

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
            string[] header = new string[sortedHeader.Length];
            string joined;

            // Change text color. Current mode is white. Others are grey.
            for (int i = 0; i < sortedHeader.Length; i++)
            {
                if (currentMode == sortedHeader[i])
                {
                    header[i] = subModeName[sortedHeader[i]];
                }
                else
                {
                    header[i] = GetComponent<GameColor>().GetColorfulText(
                        subModeName[sortedHeader[i]], ColorName.Grey);
                }
            }

            joined = string.Join(" | ", header);
            joined = "[ " + joined + " ]";
            getUI(UITag.SubModeHeader).GetComponent<Text>().text = joined;
        }

        public void SetMode(SubModeUITag mode)
        {
            currentMode = mode;
        }

        private void Start()
        {
            currentMode = SubModeUITag.Power;
            getUI = FindObjects.GetUIObject;

            sortedHeader = new SubModeUITag[]
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
