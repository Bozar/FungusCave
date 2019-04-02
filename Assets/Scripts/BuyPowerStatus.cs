using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor
{
    public class BuyPowerStatus : MonoBehaviour, ILoopSubMode
    {
        private HeaderAction header;
        private SubMode mode;

        public bool IsActive { get; private set; }

        public SubModeUITag ModeName
        {
            get
            {
                return SubModeUITag.Power;
            }
        }

        public void EnterMode()
        {
            mode.SwitchModeBuyPower(true);
        }

        public void ExitMode()
        {
            mode.SwitchModeBuyPower(false);
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
            header = FindObjects.GameLogic.GetComponent<HeaderAction>();
            mode = FindObjects.GameLogic.GetComponent<SubMode>();
        }
    }
}
