using Fungus.Actor.FOV;
using Fungus.Actor.ObjectManager;
using Fungus.Actor.WorldBuilding;
using Fungus.GameSystem;
using Fungus.Render;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    // http://www.roguebasin.com/index.php?title=Dijkstra_Maps_Visualized
    public class AutoExplore : MonoBehaviour, ICountDown
    {
        private readonly int NotChecked = 9999;
        private int[,] board;
        private Stack<int[]> checkPosition;
        private ConvertCoordinates coordinate;
        private int countAutoExplore;
        private DungeonBoard dungeon;
        private FieldOfView fov;
        private int maxCount;
        private int minDistance;
        private UIModeline modeline;
        private Move move;
        private int[] pcPosition;
        private int[] position;
        private RandomNumber random;
        private List<int[]> surround;
        private int x;
        private int y;

        public bool ContinueAutoExplore { get; private set; }

        public void AutoAction()
        {
            if (gameObject.GetComponent<AIVision>().CanSeeTarget(
                MainObjectTag.Actor))
            {
                StopAutoExplore();
                modeline.PrintText("There are enemies nearby.");

                return;
            }

            AutoMove();
        }

        public void CountDown()
        {
            if (countAutoExplore > 0)
            {
                countAutoExplore--;
            }
        }

        public void Initialize()
        {
            countAutoExplore = maxCount;
            ContinueAutoExplore = true;
        }

        private void AutoMove()
        {
            FindStartPoint();

            if ((countAutoExplore < 1) || (checkPosition.Count < 1))
            {
                StopAutoExplore();

                if (checkPosition.Count < 1)
                {
                    modeline.PrintText("You have explored everywhere.");
                }
                return;
            }

            pcPosition = coordinate.Convert(gameObject.transform.position);
            ResetBoard();
            MarkDistance();
            position = ChooseNextGrid();

            move.MoveActor(position[0], position[1]);
        }

        private void Awake()
        {
            checkPosition = new Stack<int[]>();
            surround = new List<int[]>();
            maxCount = 20;

            ContinueAutoExplore = false;
        }

        // TODO: Move some methods to AI class?
        private int[] ChooseNextGrid()
        {
            surround = coordinate.SurroundCoord(Surround.Diagonal, pcPosition);
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
                ? surround[random.Next(SeedTag.AutoExplore, 0, surround.Count)]
                : surround[0];
        }

        private void FindStartPoint()
        {
            bool isUnknown;
            bool isPassable;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    isUnknown = fov.CheckFOV(FOVStatus.Unknown, i, j);
                    isPassable = move.IsPassable(i, j);

                    if (isUnknown && isPassable)
                    {
                        position = new int[] { i, j };
                        checkPosition.Push(position);

                        break;
                    }
                }
            }
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

            minDistance = minDistance == NotChecked ? 0 : (minDistance + 1);

            return minDistance;
        }

        private void MarkDistance()
        {
            bool isPassable;
            bool isVaildDistance;

            if (checkPosition.Count < 1)
            {
                return;
            }

            position = checkPosition.Pop();
            x = position[0];
            y = position[1];

            surround = coordinate.SurroundCoord(Surround.Diagonal, position);
            surround = dungeon.FilterPositions(surround);

            if (fov.CheckFOV(FOVStatus.Unknown, position))
            {
                board[x, y] = 0;
            }
            else
            {
                board[x, y] = GetMinDistance();
            }

            foreach (var pos in surround)
            {
                isPassable = move.IsPassable(pos[0], pos[1]);
                isVaildDistance
                    = Math.Abs(board[x, y] - board[pos[0], pos[1]]) <= 1;

                if (isPassable && !isVaildDistance)
                {
                    checkPosition.Push(pos);
                }
            }

            MarkDistance();
        }

        private void ResetBoard()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    board[i, j] = NotChecked;
                }
            }
        }

        private void Start()
        {
            fov = gameObject.GetComponent<FieldOfView>();
            move = gameObject.GetComponent<Move>();

            dungeon = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();

            board = new int[dungeon.Width, dungeon.Height];
        }

        private void StopAutoExplore()
        {
            countAutoExplore = 0;
            ContinueAutoExplore = false;
        }
    }
}
