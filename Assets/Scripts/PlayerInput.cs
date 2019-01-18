using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public enum Command
    {
        INVALID,
        Left, Right, Up, Down,
        UpLeft, UpRight, DownLeft, DownRight,
        Wait, AutoExplore, Examine,
        Confirm, Cancel,
        Next, Previous,

        // NPC actions:

        Approach, Attack,

        // Debug commands:

        EndTurn, Initialize, RenderAll, PrintEnergy, AddEnergy, PrintSchedule,
        GainHP, LoseHP, DrinkPotion, PrintEnergyCost
    };

    public interface IConvertInput { Command Input2Command(); }

    //* PlayerInput
    //* -> PCActions
    //* -> Move, Attack, etc.
    public class PlayerInput : MonoBehaviour
    {
        public Command GameCommand()
        {
            bool left = Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.H)
                || Input.GetKeyDown(KeyCode.Keypad4);

            bool down = Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.J)
                || Input.GetKeyDown(KeyCode.Keypad2);

            bool up = Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.K)
                || Input.GetKeyDown(KeyCode.Keypad8);

            bool right = Input.GetKeyDown(KeyCode.RightArrow)
                || Input.GetKeyDown(KeyCode.L)
                || Input.GetKeyDown(KeyCode.Keypad6);

            if (left)
            {
                return Command.Left;
            }
            else if (down)
            {
                return Command.Down;
            }
            else if (up)
            {
                return Command.Up;
            }
            else if (right)
            {
                return Command.Right;
            }
            else if (GetComponent<IConvertInput>() != null)
            {
                return GetComponent<IConvertInput>().Input2Command();
            }
            return Command.INVALID;
        }
    }
}
