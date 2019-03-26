﻿using Fungus.Actor.InputManager;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class InputSwitchSubMode : MonoBehaviour, IConvertInput
    {
        public Command Input2Command()
        {
            if (Input.GetKey(KeyCode.LeftShift)
                || Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    return Command.Previous;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                return Command.Next;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                return Command.Cancel;
            }
            return Command.INVALID;
        }
    }
}
