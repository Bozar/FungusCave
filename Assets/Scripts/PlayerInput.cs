using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public enum Command
    {
        Left, Right, Up, Down,
        UpLeft, UpRight, DownLeft, DownRight,
        Wait,

        // Debug commands:
        EndTurn, Initialize,

        Confirm, Cancel, Invalid
    };

    public Command OutputCommand()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.H)
            || Input.GetKeyDown(KeyCode.Keypad4))
        {
            return Command.Left;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.J)
            || Input.GetKeyDown(KeyCode.Keypad2))
        {
            return Command.Down;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.K)
            || Input.GetKeyDown(KeyCode.Keypad8))
        {
            return Command.Up;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.L)
            || Input.GetKeyDown(KeyCode.Keypad6))
        {
            return Command.Right;
        }
        else if (Input.GetKeyDown(KeyCode.Y)
            || Input.GetKeyDown(KeyCode.Keypad7))
        {
            return Command.UpLeft;
        }
        else if (Input.GetKeyDown(KeyCode.U)
            || Input.GetKeyDown(KeyCode.Keypad9))
        {
            return Command.UpRight;
        }
        else if (Input.GetKeyDown(KeyCode.B)
            || Input.GetKeyDown(KeyCode.Keypad1))
        {
            return Command.DownLeft;
        }
        else if (Input.GetKeyDown(KeyCode.N)
            || Input.GetKeyDown(KeyCode.Keypad3))
        {
            return Command.DownRight;
        }
        // Test key combinations.
        else if (Input.GetKey(KeyCode.LeftControl)
            && Input.GetKeyDown(KeyCode.F))
        {
            return Command.Up;
        }
        else if (Input.GetKeyDown(KeyCode.End))
        {
            return Command.EndTurn;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            return Command.Initialize;
        }

        return Command.Invalid;
    }
}
