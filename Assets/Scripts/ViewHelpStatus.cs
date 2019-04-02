using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor
{
    public class ViewHelpStatus : MonoBehaviour, ILoopSubMode
    {
        private HeaderStatus header;
        private SubMode mode;

        public bool IsActive { get; private set; }

        public SubModeUITag ModeName
        {
            get
            {
                return SubModeUITag.Help;
            }
        }

        public void EnterMode()
        {
            mode.SwitchModeHelp(true);
        }

        public void ExitMode()
        {
            mode.SwitchModeHelp(false);
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
