using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor
{
    public class SettingStatus : MonoBehaviour, ILoopSubMode
    {
        private HeaderStatus header;
        private SubMode mode;

        public bool IsActive { get; private set; }

        public bool IsModified { get; set; }

        public SubModeUITag ModeName
        {
            get
            {
                return SubModeUITag.Setting;
            }
        }

        public void EnterMode()
        {
            mode.SwitchModeSetting(true);
        }

        public void ExitMode()
        {
            mode.SwitchModeSetting(false);
        }

        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }

        private void Awake()
        {
            IsActive = false;
        }

        private void Start()
        {
            header = FindObjects.GameLogic.GetComponent<HeaderStatus>();
            mode = FindObjects.GameLogic.GetComponent<SubMode>();

            header.AddMode(this);
        }
    }
}
