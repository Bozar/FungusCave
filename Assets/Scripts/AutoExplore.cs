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
    // http://www.roguebasin.com/index.php?title=Dijkstra_Maps_Visualized
    public class AutoExplore : MonoBehaviour, ITurnCounter
    {
        private readonly int gridSize = 10;
        private readonly int notChecked = 9999;
        private ActorBoard actor;
        private int[,] board;
        private Stack<int[]> checkPosition;
        private ConvertCoordinates coord;
        private int countAutoExplore;
        private int[] currentPosition;
        private DungeonBoard dungeon;
        private FieldOfView fov;
        private bool isPC;
        private int maxCount;
        private int minDistance;
        private UIModeline modeline;
        private int[] newPosition;
        private RandomNumber random;
        private List<int[]> surround;
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
            MarkDistance();
            newPosition = GetNewPosition();

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
            surround = new List<int[]>();
            maxCount = 20;
        }

        private int GetMinDistance()
        {
            minDistance = board[surround[0][0], surround[0][1]];

            foreach (var pos in surround)
            {
                if (board[pos[0], pos[1]] < minDistance)
                {
                    minDistance = board[pos[0], pos[1]];
                }
            }

            minDistance = (minDistance == notChecked)
                ? 0 : (minDistance + gridSize);
            return minDistance;
        }

        private int[] GetNewPosition()
        {
            SeedTag tag = isPC ? SeedTag.AutoExplore : SeedTag.NPCAction;

            surround = coord.SurroundCoord(Surround.Diagonal, currentPosition);
            surround = dungeon.FilterPositions(surround);

            minDistance = board[surround[0][0], surround[0][1]];
            checkPosition.Clear();
            checkPosition.Push(surround[0]);

            foreach (var pos in surround)
            {
                if (board[pos[0], pos[1]] < minDistance)
                {
                    minDistance = board[pos[0], pos[1]];
                    checkPosition.Clear();
                    checkPosition.Push(pos);
                }
                else if (board[pos[0], pos[1]] == minDistance)
                {
                    checkPosition.Push(pos);
                }
            }

            surround = new List<int[]>();
            while (checkPosition.Count > 0)
            {
                surround.Add(checkPosition.Pop());
            }

            return (surround.Count > 1)
                ? surround[random.Next(tag, 0, surround.Count)]
                : surround[0];
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
                modeline.PrintText("There are enemies nearby.");
                return false;
            }

            GetUnknownPosition();

            if (checkPosition.Count < 1)
            {
                StopAutoExplore();
                modeline.PrintText("You have explored everywhere.");
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

        private void MarkDistance()
        {
            //int x, y, pcX, pcY;
            int x, y;
            bool isPassable;
            bool isVaildDistance;

            if (checkPosition.Count < 1)
            {
                return;
            }

            newPosition = checkPosition.Pop();
            x = newPosition[0];
            y = newPosition[1];

            surround = coord.SurroundCoord(Surround.Diagonal, newPosition);
            surround = dungeon.FilterPositions(surround);

            if (isPC)
            {
                board[x, y] = fov.CheckFOV(FOVStatus.Unknown, newPosition)
                    ? 0 : GetMinDistance();
            }
            else
            {
                //pcX = GetComponent<NPCMemory>().PCPosition[0];
                //pcY = GetComponent<NPCMemory>().PCPosition[1];

                //board[x, y] = ((x == pcX) && (y == pcY))
                //    ? 0 : GetMinDistance();

                board[x, y]
                    = actor.CheckActorTag(SubObjectTag.PC, actor.GetActor(x, y))
                    ? 0 : GetMinDistance();
            }

            foreach (var pos in surround)
            {
                isPassable = terrain.IsPassable(pos[0], pos[1]);
                isVaildDistance
                    = Math.Abs(board[x, y] - board[pos[0], pos[1]]) <= gridSize;

                if (isPassable && !isVaildDistance)
                {
                    checkPosition.Push(pos);
                }
            }

            MarkDistance();
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

        private void Start()
        {
            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();

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
