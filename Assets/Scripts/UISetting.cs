using Fungus.Actor;
using Fungus.Actor.InputManager;
using Fungus.GameSystem.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UISetting : MonoBehaviour, IUpdateUI, IResetData
    {
        private readonly string node = "Setting";
        private int cursorPosition;
        private UIText getUI;
        private UITag[] orderedCursor;

        private delegate Text UIText(UITag tag);

        public UITag HighlightedSetting
        {
            get { return orderedCursor[cursorPosition]; }
        }

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

        public void Reset()
        {
            cursorPosition = 0;
        }

        private void Awake()
        {
            cursorPosition = 0;
        }

        private void PrintCursor()
        {
            for (int i = 0; i < orderedCursor.Length; i++)
            {
                if (i == cursorPosition)
                {
                    getUI(orderedCursor[i]).text = GetComponent<GameText>()
                        .GetStringData(node, "Cursor");
                }
                else
                {
                    getUI(orderedCursor[i]).text = "";
                }
            }
        }

        private void PrintDescription()
        {
            getUI(UITag.SettingText1).text = GetComponent<GameText>()
               .GetStringData(node, "ShowOpening");
            getUI(UITag.SettingText2).text = GetComponent<GameText>()
               .GetStringData(node, "RushMode");
        }

        private void PrintSetting()
        {
            PrintCursor();
            PrintStatus();
            PrintDescription();
        }

        private void PrintStatus()
        {
            getUI(UITag.SettingOption1).text
               = GetComponent<GameSetting>().ShowOpening
               ? GetComponent<GameText>().GetStringData(node, "SwitchOn")
               : GetComponent<GameText>().GetStringData(node, "SwitchOff");

            getUI(UITag.SettingOption2).text
               = GetComponent<GameSetting>().IsRushMode
               ? GetComponent<GameText>().GetStringData(node, "SwitchOn")
               : GetComponent<GameText>().GetStringData(node, "SwitchOff");
        }

        private void Start()
        {
            getUI = FindObjects.GetUIText;
            GetComponent<UserInterface>().MovingCursor += UISetting_MovingCursor;

            orderedCursor = new UITag[]
            { UITag.SettingCursor1, UITag.SettingCursor2 };
        }

        private void UISetting_MovingCursor(object sender, MoveCursorEventArgs e)
        {
            if (e.Commander != CommanderTag.UISetting)
            {
                return;
            }
            cursorPosition = GetComponent<UserInterface>().MoveCursorUpDown(
                e.Direction, cursorPosition, orderedCursor.Length - 1);
        }
    }
}
