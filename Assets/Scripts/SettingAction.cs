using Fungus.Actor.InputManager;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class SettingAction : MonoBehaviour
    {
        private HeaderAction header;
        private SubMode mode;
        private GameSetting setting;
        private UserInterface ui;
        private UISetting uiSetting;

        private void Start()
        {
            mode = FindObjects.GameLogic.GetComponent<SubMode>();
            header = FindObjects.GameLogic.GetComponent<HeaderAction>();
            setting = FindObjects.GameLogic.GetComponent<GameSetting>();
            ui = FindObjects.GameLogic.GetComponent<UserInterface>();
            uiSetting = FindObjects.GameLogic.GetComponent<UISetting>();
        }

        private void SwitchSetting()
        {
            switch (uiSetting.HighlightedSetting)
            {
                case UITag.SettingCursor1:
                    setting.ShowOpening = !setting.ShowOpening;
                    break;

                case UITag.SettingCursor2:
                    setting.IsRushMode = !setting.IsRushMode;
                    break;
            }
        }

        private void Update()
        {
            Command cmd = GetComponent<PlayerInput>().GameCommand();
            switch (cmd)
            {
                case Command.Confirm:
                    SwitchSetting();
                    GetComponent<SettingStatus>().IsModified = true;
                    break;

                case Command.Cancel:
                    mode.SwitchModeSetting(false);
                    break;

                case Command.Down:
                case Command.Up:
                    ui.CommanderMoveCursor(new MoveCursorEventArgs
                    {
                        Commander = CommanderTag.UISetting,
                        Direction = cmd
                    });
                    break;

                case Command.Next:
                case Command.Previous:
                    header.SwitchMode(cmd);
                    break;
            }
        }
    }
}
