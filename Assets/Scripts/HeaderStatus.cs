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
        private List<ILoopSubMode> subModes;

        public SubModeUITag CurrentModeName
        {
            get
            {
                foreach (ILoopSubMode sm in subModes)
                {
                    if (sm.IsActive)
                    {
                        return sm.ModeName;
                    }
                }
                return SubModeUITag.INVALID;
            }
        }

        public ILoopSubMode[] SubModes
        {
            get
            {
                return subModes.ToArray();
            }
        }

        public void AddMode(ILoopSubMode mode)
        {
            if (subModes.Contains(mode))
            {
                return;
            }
            subModes.Add(mode);
        }

        public void RemoveMode(ILoopSubMode mode)
        {
            subModes.Remove(mode);
        }

        private void Awake()
        {
            subModes = new List<ILoopSubMode>();
        }
    }
}
