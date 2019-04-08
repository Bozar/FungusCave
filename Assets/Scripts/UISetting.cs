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
            getUI(UITag.SettingCursor1).GetComponent<Text>().text
                = GetComponent<GameText>().GetSettingCursor();
            getUI(UITag.SettingOption1).GetComponent<Text>().text
                = GetComponent<GameText>().GetSettingOption(
                    GetComponent<GameSetting>().ShowOpening);
            getUI(UITag.SettingText1).GetComponent<Text>().text
                 = GetComponent<GameText>().GetSettingText("ShowOpening");
        }

        private void Start()
        {
            getUI = FindObjects.GetUIText;
        }
    }
}
