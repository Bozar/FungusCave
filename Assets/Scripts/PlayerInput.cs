using UnityEngine;

namespace Fungus.Actor.InputManager
{
    public enum Command
    {
        INVALID,
        Left, Right, Up, Down,
        UpLeft, UpRight, DownLeft, DownRight,
        Wait, AutoExplore, Examine, ViewHelp, BuyPower, ViewLog,
        Confirm, Cancel,
        Next, Previous,
        Save,

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
            if (GetComponent<IConvertInput>() != null)
            {
                IConvertInput[] iciComp = GetComponents<IConvertInput>();
                Command cmd;

                // A gameobject can have one or more IConvertInput, but all the
                // key-command pairs must be unique.
                foreach (IConvertInput ici in iciComp)
                {
                    cmd = ici.Input2Command();
                    if (cmd != Command.INVALID)
                    {
                        return cmd;
                    }
                }
            }
            return Input2Move();
        }

        private Command Input2Move()
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

            bool upLeft = Input.GetKeyDown(KeyCode.Y)
               || Input.GetKeyDown(KeyCode.Keypad7);

            bool upRight = Input.GetKeyDown(KeyCode.U)
                || Input.GetKeyDown(KeyCode.Keypad9);

            bool downLeft = Input.GetKeyDown(KeyCode.B)
                || Input.GetKeyDown(KeyCode.Keypad1);

            bool downRight = Input.GetKeyDown(KeyCode.N)
                || Input.GetKeyDown(KeyCode.Keypad3);

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
            else if (upLeft)
            {
                return Command.UpLeft;
            }
            else if (upRight)
            {
                return Command.UpRight;
            }
            else if (downLeft)
            {
                return Command.DownLeft;
            }
            else if (downRight)
            {
                return Command.DownRight;
            }
            return Command.INVALID;
        }
    }
}
