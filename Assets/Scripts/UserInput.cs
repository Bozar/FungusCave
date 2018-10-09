using UnityEngine;

public class UserInput : MonoBehaviour
{
    public enum Command
    {
        Left, Right, Up, Down,
        UpLeft, UpRight, DownLeft, DownRight,
        Wait,
        Confirm, Cancel, Invalid
    };

    public int OutputCommand()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)
            || Input.GetKeyDown(KeyCode.H)
            || Input.GetKeyDown(KeyCode.Keypad4))
        {
            return (int)Command.Left;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.J)
            || Input.GetKeyDown(KeyCode.Keypad2))
        {
            return (int)Command.Down;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.K)
            || Input.GetKeyDown(KeyCode.Keypad8))
        {
            return (int)Command.Up;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)
            || Input.GetKeyDown(KeyCode.L)
            || Input.GetKeyDown(KeyCode.Keypad6))
        {
            return (int)Command.Right;
        }
        else if (Input.GetKeyDown(KeyCode.Y)
            || Input.GetKeyDown(KeyCode.Keypad7))
        {
            return (int)Command.UpLeft;
        }
        else if (Input.GetKeyDown(KeyCode.U)
            || Input.GetKeyDown(KeyCode.Keypad9))
        {
            return (int)Command.UpRight;
        }
        else if (Input.GetKeyDown(KeyCode.B)
            || Input.GetKeyDown(KeyCode.Keypad1))
        {
            return (int)Command.DownLeft;
        }
        else if (Input.GetKeyDown(KeyCode.N)
            || Input.GetKeyDown(KeyCode.Keypad3))
        {
            return (int)Command.DownRight;
        }
        //Test key combinations.
        else if (Input.GetKey(KeyCode.LeftControl)
            && Input.GetKeyDown(KeyCode.F))
        {
            return (int)Command.Up;
        }

        return (int)Command.Invalid;
    }
}
