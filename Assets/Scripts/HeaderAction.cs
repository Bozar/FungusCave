using Fungus.Actor.InputManager;
using Fungus.GameSystem.Render;
using System;
using UnityEngine;

// HeaderAction is NOT attached to a specific actor. It is called indirectly by
// an actor's method.
namespace Fungus.GameSystem
{
    public class HeaderAction : MonoBehaviour
    {
        public void SwitchMode(Command direction)
        {
            ILoopSubMode[] subModes = GetComponent<HeaderStatus>().SubModes;
            ILoopSubMode currentMode = null;
            ILoopSubMode newMode;

            foreach (ILoopSubMode sm in subModes)
            {
                if (sm.IsActive)
                {
                    currentMode = sm;
                    break;
                }
            }
            if (currentMode == null)
            {
                return;
            }

            currentMode.ExitMode();

            newMode = GetNewMode(direction, currentMode, subModes);
            newMode.EnterMode();
        }

        private ILoopSubMode GetNewMode(
            Command direction, ILoopSubMode current, ILoopSubMode[] modes)
        {
            SubModeUITag[] header = GetComponent<UIHeader>().SortedHeader;

            int currentIndex = Array.IndexOf(header, current.ModeName);
            int maxIndex = header.Length - 1;
            int newIndex;

            switch (direction)
            {
                case Command.Next:
                    newIndex = ((currentIndex + 1) > maxIndex)
                        ? 0 : (currentIndex + 1);
                    break;

                case Command.Previous:
                    newIndex = ((currentIndex - 1) < 0)
                       ? maxIndex : (currentIndex - 1);
                    break;

                default:
                    newIndex = currentIndex;
                    break;
            }

            foreach (ILoopSubMode sm in modes)
            {
                if (sm.ModeName == header[newIndex])
                {
                    return sm;
                }
            }
            return current;
        }
    }
}
