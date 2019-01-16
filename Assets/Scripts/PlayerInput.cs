﻿using UnityEngine;

namespace Fungus.Actor
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

    //* PlayerInput
    //* -> PCActions
    //* -> Move, Attack, etc.
    public class PlayerInput : MonoBehaviour
    {
        private bool down;
        private bool downLeft;
        private bool downRight;
        private bool left;
        private bool next;
        private bool previous;
        private bool right;
        private bool up;
        private bool upLeft;
        private bool upRight;
        private bool wait;

        public Command GameCommand()
        {
            left = Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.H)
                || Input.GetKeyDown(KeyCode.Keypad4);

            down = Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.J)
                || Input.GetKeyDown(KeyCode.Keypad2);

            up = Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.K)
                || Input.GetKeyDown(KeyCode.Keypad8);

            right = Input.GetKeyDown(KeyCode.RightArrow)
                || Input.GetKeyDown(KeyCode.L)
                || Input.GetKeyDown(KeyCode.Keypad6);

            upLeft = Input.GetKeyDown(KeyCode.Y)
                || Input.GetKeyDown(KeyCode.Keypad7);

            upRight = Input.GetKeyDown(KeyCode.U)
                || Input.GetKeyDown(KeyCode.Keypad9);

            downLeft = Input.GetKeyDown(KeyCode.B)
                || Input.GetKeyDown(KeyCode.Keypad1);

            downRight = Input.GetKeyDown(KeyCode.N)
                || Input.GetKeyDown(KeyCode.Keypad3);

            wait = Input.GetKeyDown(KeyCode.Period);

            next = Input.GetKeyDown(KeyCode.PageDown)
                || Input.GetKeyDown(KeyCode.Equals);

            previous = Input.GetKeyDown(KeyCode.PageUp)
                || Input.GetKeyDown(KeyCode.Minus);

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
            else if (wait)
            {
                return Command.Wait;
            }
            else if (next)
            {
                return Command.Next;
            }
            else if (previous)
            {
                return Command.Previous;
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                return Command.AutoExplore;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                return Command.Examine;
            }
            // Test key combinations.
            else if (Input.GetKey(KeyCode.LeftControl)
                && Input.GetKeyDown(KeyCode.F))
            {
                return Command.Up;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                return Command.Cancel;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                return Command.Initialize;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                return Command.RenderAll;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                return Command.PrintEnergy;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                return Command.AddEnergy;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                return Command.PrintSchedule;
            }
            else if (Input.GetKeyDown(KeyCode.Equals))
            {
                return Command.GainHP;
            }
            else if (Input.GetKeyDown(KeyCode.Minus))
            {
                return Command.LoseHP;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                return Command.DrinkPotion;
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                return Command.PrintEnergyCost;
            }

            return Command.INVALID;
        }
    }
}
