using Fungus.Actor.InputManager;
using Fungus.GameSystem.Data;
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
            getUI(UITag.SettingOption2).text
               = GetComponent<GameSetting>().IsRushMode
               ? GetComponent<GameText>().GetStringData(node, "SwitchOn")
               : GetComponent<GameText>().GetStringData(node, "SwitchOff");

            getUI(UITag.SettingText1).text = GetComponent<GameText>()
                .GetStringData(node, "ShowOpening");
            getUI(UITag.SettingText2).text = GetComponent<GameText>()
               .GetStringData(node, "RushMode");
        }

        private void Start()
        {
            getUI = FindObjects.GetUIText;
            GetComponent<UserInterface>().MovingCursor += UISetting_MovingCursor;
        }

        private void UISetting_MovingCursor(object sender, MoveCursorEventArgs e)
        {
            if (e.Commander != CommanderTag.UISetting)
            {
                return;
            }
            Debug.Log("UISetting");
            //cursorPosition = GetComponent<UserInterface>().MoveCursorUpDown(
            //    e.Direction, cursorPosition, orderedPower.Length - 1);
        }
    }
}
