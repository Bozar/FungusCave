using Fungus.Actor.FOV;
using Fungus.Actor.ObjectManager;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public interface IAutoExplore
    {
        SeedTag GetSeedTag();

        bool GetStartPoint(out int[] startPoint);

        bool IsStartPoint(int[] position);
    }

    // http://www.roguebasin.com/index.php?title=Dijkstra_Maps_Visualized
    public class AutoExplore : MonoBehaviour, ITurnCounter
    {
        private readonly int gridSize = 10;
        private readonly int notChecked = 9999;
        private int[,] board;
        private Stack<int[]> checkPosition;
        private ConvertCoordinates coord;
        private int countAutoExplore;
        private int[] currentPosition;
        private DungeonBoard dungeon;
        private FieldOfView fov;
        private bool isPC;
        private int maxCount;
        private UIModeline modeline;
        private int[] newPosition;
        private RandomNumber random;
        private DungeonTerrain terrain;

        public bool ContinueAutoExplore { get { return countAutoExplore > 0; } }

        public void Count()
        {
            if (countAutoExplore > 0)
            {
                countAutoExplore--;
            }
        }

        public int[] GetDestination()
        {
            if (isPC)
            {
                if (!GetStartPointForPC())
                {
                    return null;
                }
            }
            else
            {
                if (!GetStartPointForNPC())
                {
                    return null;
                }
            }

            currentPosition = coord.Convert(transform.position);
            ResetBoard();
            SetDistance(checkPosition);
            int[] newPosition = GetNewPosition(currentPosition);

            return newPosition;
        }

        public void Initialize()
        {
            countAutoExplore = maxCount;
        }

        public void Trigger()
        {
        }

        private void Awake()
        {
            checkPosition = new Stack<int[]>();
            maxCount = 20;
        }

        private int GetDistance(List<int[]> surround)
        {
            int distance = board[surround[0][0], surround[0][1]];

            foreach (var pos in surround)
            {
                if (board[pos[0], pos[1]] < distance)
                {
                    distance = board[pos[0], pos[1]];
                }
            }

            distance = (distance == notChecked) ? 0 : (distance + gridSize);
            return distance;
        }

        private int[] GetNewPosition(int[] currentPosition)
        {
            SeedTag tag = GetComponent<IAutoExplore>().GetSeedTag();

            List<int[]> surround = coord.SurroundCoord(
                Surround.Diagonal, currentPosition);
            surround = dungeon.FilterPositions(surround);

            int minDistance = board[surround[0][0], surround[0][1]];
            List<int[]> destination = new List<int[]>();

            foreach (int[] pos in surround)
            {
                if (board[pos[0], pos[1]] < minDistance)
                {
                    minDistance = board[pos[0], pos[1]];
                    destination.Clear();
                    destination.Add(pos);
                }
                else if (board[pos[0], pos[1]] == minDistance)
                {
                    destination.Add(pos);
                }
            }

            return (destination.Count > 1)
                ? destination[random.Next(tag, 0, destination.Count)]
                : destination[0];
        }

        private bool GetStartPointForNPC()
        {
            //if (GetComponent<NPCMemory>().PCPosition == null)
            //{
            //    return false;
            //}

            newPosition = coord.Convert(FindObjects.PC.transform.position);
            //newPosition = GetComponent<NPCMemory>().PCPosition;
            checkPosition.Push(newPosition);
            return true;
        }

        private bool GetStartPointForPC()
        {
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

            GetUnknownPosition();

            if (checkPosition.Count < 1)
            {
                StopAutoExplore();
                modeline.PrintStaticText("You have explored everywhere.");
                return false;
            }
            return true;
        }

        private void GetUnknownPosition()
        {
            bool isUnknown;
            bool isPassable;

            for (int i = 0; i < dungeon.Width; i++)
            {
                for (int j = 0; j < dungeon.Height; j++)
                {
                    isUnknown = fov.CheckFOV(FOVStatus.Unknown, i, j);
                    isPassable = terrain.IsPassable(i, j);

                    if (isUnknown && isPassable)
                    {
                        newPosition = new int[] { i, j };
                        checkPosition.Push(newPosition);
                        return;
                    }
                }
            }

            newPosition = new int[2];
            checkPosition.Clear();
            return;
        }

        private void ResetBoard()
        {
            for (int i = 0; i < dungeon.Width; i++)
            {
                for (int j = 0; j < dungeon.Height; j++)
                {
                    board[i, j] = notChecked;
                }
            }
        }

        private void SetDistance(Stack<int[]> uncheckedGrids)
        {
            if (uncheckedGrids.Count < 1)
            {
                return;
            }

            int[] position = uncheckedGrids.Pop();
            int x = position[0];
            int y = position[1];

            List<int[]> surround = coord.SurroundCoord(
                Surround.Diagonal, position);
            surround = dungeon.FilterPositions(surround);

            board[x, y] = GetComponent<IAutoExplore>().IsStartPoint(position)
                  ? 0 : GetDistance(surround);

            foreach (var pos in surround)
            {
                bool isPassable = terrain.IsPassable(pos[0], pos[1]);
                bool isVaildDistance
                    = Math.Abs(board[x, y] - board[pos[0], pos[1]])
                    <= gridSize;

                if (isPassable && !isVaildDistance)
                {
                    uncheckedGrids.Push(pos);
                }
            }

            SetDistance(uncheckedGrids);
        }

        private void Start()
        {
            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();

            fov = GetComponent<FieldOfView>();

            isPC = GetComponent<ObjectMetaInfo>().SubTag == SubObjectTag.PC;
            board = new int[dungeon.Width, dungeon.Height];
        }

        private void StopAutoExplore()
        {
            countAutoExplore = 0;
        }
    }
}
