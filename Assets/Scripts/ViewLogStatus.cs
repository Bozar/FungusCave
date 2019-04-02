using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor
{
    public class ViewLogStatus : MonoBehaviour, ILoopSubMode
    {
        private HeaderStatus header;
        private SubMode mode;

        public bool IsActive { get; private set; }

        public SubModeUITag ModeName
        {
            get
            {
                return SubModeUITag.Log;
            }
        }

        public void EnterMode()
        {
            mode.SwitchModeLog(true);
        }

        public void ExitMode()
        {
            mode.SwitchModeLog(false);
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
