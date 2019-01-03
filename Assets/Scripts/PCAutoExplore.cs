using Fungus.Actor.FOV;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class PCAutoExplore : MonoBehaviour, IAutoExplore, ITurnCounter
    {
        private int countAutoExplore;
        private DungeonBoard dungeon;
        private FieldOfView fov;
        private int maxCount;
        private UIModeline modeline;
        private DungeonTerrain terrain;

        public bool ContinueAutoExplore { get { return countAutoExplore > 0; } }

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

        public bool GetStartPoint(out Stack<int[]> startPoint)
        {
            startPoint = new Stack<int[]>();

            if (countAutoExplore < 1)
            {
                StopAutoExplore();
                return false;
            }
            else if (GetComponent<AIVision>().CanSeeTarget(MainObjectTag.Actor))
            {
                StopAutoExplore();
                modeline.PrintStaticText("There are enemies nearby.");
                return false;
            }

            startPoint = GetUnknownPosition();

            if (startPoint.Count < 1)
            {
                StopAutoExplore();
                modeline.PrintStaticText("You have explored everywhere.");
                return false;
            }
            return true;
        }

        public bool IsStartPoint(int[] position)
        {
            return GetComponent<FieldOfView>().CheckFOV(
                FOVStatus.Unknown, position);
        }

        public void Trigger()
        {
            countAutoExplore = maxCount;
        }

        private void Awake()
        {
            maxCount = 20;
        }

        private Stack<int[]> GetUnknownPosition()
        {
            Stack<int[]> position = new Stack<int[]>();

            for (int i = 0; i < dungeon.Width; i++)
            {
                for (int j = 0; j < dungeon.Height; j++)
                {
                    if (fov.CheckFOV(FOVStatus.Unknown, i, j)
                        && terrain.IsPassable(i, j))
                    {
                        position.Push(new int[] { i, j });
                        return position;
                    }
                }
            }
            return position;
        }

        private void Start()
        {
            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();

            fov = GetComponent<FieldOfView>();
        }

        private void StopAutoExplore()
        {
            countAutoExplore = 0;
        }
    }
}
