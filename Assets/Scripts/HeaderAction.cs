using Fungus.Actor.InputManager;
using UnityEngine;

// HeaderAction is NOT attached to a specific actor. It is called indirectly by
// an actor's method.
namespace Fungus.GameSystem
{
    public interface ILoopSubMode
    {
        SubModeUITag ModeName { get; }

        void EnterNewMode(Command direction);

        void ExitCurrentMode();
    }

    public class HeaderAction : MonoBehaviour
    {
        public SubModeUITag CurrentMode { get; private set; }

        public void SetMode(SubModeUITag mode)
        {
            CurrentMode = mode;
        }

        private void Start()
        {
            CurrentMode = SubModeUITag.Power;
        }
    }
}
