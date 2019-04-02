using System.Collections.Generic;
using UnityEngine;

namespace Fungus.GameSystem
{
    public interface ILoopSubMode
    {
        bool IsActive { get; }
        SubModeUITag ModeName { get; }

        void EnterMode();

        void ExitMode();

        void SetIsActive(bool isActive);
    }

    public class HeaderStatus : MonoBehaviour
    {
        private List<ILoopSubMode> modes;

        public SubModeUITag CurrentModeName
        {
            get
            {
                foreach (ILoopSubMode sm in modes)
                {
                    if (sm.IsActive)
                    {
                        return sm.ModeName;
                    }
                }
                return SubModeUITag.INVALID;
            }
        }

        private void Awake()
        {
            modes = new List<ILoopSubMode>();
        }
    }
}
