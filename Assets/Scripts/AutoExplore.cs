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
        private Move move;
        private int[] newPosition;
        private RandomNumber random;
        private List<int[]> surround;

        public bool ContinueAutoExplore { get; private set; }

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
                GetStartPointForNPC();
            }

            currentPosition = coord.Convert(transform.position);
            ResetBoard();
            MarkDistance();
            newPosition = ChooseNextGrid();

            return newPosition;
        }

        public void Initialize()
        {
            countAutoExplore = maxCount;
            ContinueAutoExplore = true;
        }

        public void Trigger()
        {
        }

        private void Awake()
        {
            checkPosition = new Stack<int[]>();
            surround = new List<int[]>();
            maxCount = 20;

            ContinueAutoExplore = false;
        }

        private int[] ChooseNextGrid()
        {
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

            // TODO: Use another RNG for NPC.
            return (surround.Count > 1)
                ? surround[random.Next(SeedTag.AutoExplore, 0, surround.Count)]
                : surround[0];
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

        private void GetStartPointForNPC()
        {
            newPosition = coord.Convert(FindObjects.PC.transform.position);
            checkPosition.Push(newPosition);
        }

        private bool GetStartPointForPC()
        {
            if (GetComponent<AIVision>().CanSeeTarget(MainObjectTag.Actor))
            {
                StopAutoExplore();
                modeline.PrintText("There are enemies nearby.");
                return false;
            }
            else if (countAutoExplore < 1)
            {
                StopAutoExplore();
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
                    isPassable = move.IsPassable(i, j);

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
                if (fov.CheckFOV(FOVStatus.Unknown, newPosition))
                {
                    board[x, y] = 0;
                }
                else
                {
                    board[x, y] = GetMinDistance();
                }
            }
            else
            {
                if (actor.CheckActorTag(SubObjectTag.PC, x, y))
                {
                    board[x, y] = 0;
                }
                else
                {
                    board[x, y] = GetMinDistance();
                }
            }

            foreach (var pos in surround)
            {
                isPassable = move.IsPassable(pos[0], pos[1]);
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
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();

            fov = GetComponent<FieldOfView>();
            move = GetComponent<Move>();

            board = new int[dungeon.Width, dungeon.Height];
            isPC = GetComponent<ObjectMetaInfo>().SubTag == SubObjectTag.PC;
        }

        private void StopAutoExplore()
        {
            countAutoExplore = 0;
            ContinueAutoExplore = false;
        }
    }
}
