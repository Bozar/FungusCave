using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UISetting : MonoBehaviour, IUpdateUI
    {
        private UIText getUI;

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
            PrintSetting();
        }

        private void PrintSetting()
        {
            string node = "Setting";

            getUI(UITag.SettingCursor1).text = GetComponent<GameText>()
                .GetStringData(node, "Cursor");

            getUI(UITag.SettingOption1).text
                = GetComponent<GameSetting>().ShowOpening
                ? GetComponent<GameText>().GetStringData(node, "SwitchOn")
                : GetComponent<GameText>().GetStringData(node, "SwitchOff");

            getUI(UITag.SettingText1).text = GetComponent<GameText>()
                .GetStringData(node, "ShowOpening");
        }

        private void Start()
        {
            getUI = FindObjects.GetUIText;
        }
    }
}
