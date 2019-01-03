using Fungus.GameSystem;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public interface IAutoExplore
    {
        SeedTag GetSeedTag();

        bool GetStartPoint(out Stack<int[]> startPoint);

        bool IsStartPoint(int[] position);
    }

    // http://www.roguebasin.com/index.php?title=Dijkstra_Maps_Visualized
    public class AutoExplore : MonoBehaviour
    {
        private readonly int gridSize = 10;
        private readonly int notChecked = 9999;
        private int[,] board;
        private ConvertCoordinates coord;
        private DungeonBoard dungeon;
        private RandomNumber random;
        private DungeonTerrain terrain;

        public int[] GetDestination()
        {
            Stack<int[]> startPoint = new Stack<int[]>();

            if (!GetComponent<IAutoExplore>().GetStartPoint(out startPoint))
            {
                return null;
            }

            ResetBoard();
            SetDistance(startPoint);

            int[] currentPosition = coord.Convert(transform.position);
            int[] newPosition = GetNewPosition(currentPosition);

            return newPosition;
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

            foreach (int[] pos in surround)
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
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();

            board = new int[dungeon.Width, dungeon.Height];
        }
    }
}
