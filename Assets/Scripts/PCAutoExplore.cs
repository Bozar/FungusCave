using Fungus.Actor.FOV;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class PCAutoExplore : MonoBehaviour, IAutoExplore, ITurnCounter
    {
        private DungeonBoard board;
        private ConvertCoordinates coord;
        private int countAutoExplore;
        private UIModeline modeline;
        private Stack<int[]> previousPosition;
        private GameSetting setting;

        public bool ContinueAutoExplore()
        {
            bool count = countAutoExplore > 0;

            if (count && !FindEnemy() && FindUnknownGrid())
            {
                return true;
            }
            countAutoExplore = 0;
            return false;
        }

        public void Count()
        {
            if (countAutoExplore > 0)
            {
                countAutoExplore--;
            }
        }

        public SeedTag GetSeedTag()
        {
            return SeedTag.AutoExplore;
        }

        public bool IsStartPoint(int x, int y)
        {
            return GetComponent<FieldOfView>().CheckFOV(FOVStatus.Unknown,
                new int[] { x, y });
        }

        public bool IsValidDestination(int[] check)
        {
            int[] previous = previousPosition.Peek();

            if ((check[0] == previous[0]) && (check[1] == previous[1]))
            {
                countAutoExplore = 0;
                modeline.PrintStaticText("Please move manually.");
                return false;
            }
            previousPosition.Push(coord.Convert(transform.position));
            return true;
        }

        public void Trigger()
        {
            countAutoExplore = setting.AutoExploreStep;
            previousPosition = new Stack<int[]>();
            previousPosition.Push(coord.Convert(transform.position));
        }

        private bool FindEnemy()
        {
            if (GetComponent<AIVision>().CanSeeTarget(MainObjectTag.Actor))
            {
                modeline.PrintStaticText("You see an enemy.");
                return true;
            }
            return false;
        }

        private bool FindUnknownGrid()
        {
            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    if (GetComponent<FieldOfView>().CheckFOV(FOVStatus.Unknown,
                        i, j))
                    {
                        return true;
                    }
                }
            }
            modeline.PrintStaticText("You have explored everywhere.");
            return false;
        }

        private void Start()
        {
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            setting = FindObjects.GameLogic.GetComponent<GameSetting>();
        }
    }
}
