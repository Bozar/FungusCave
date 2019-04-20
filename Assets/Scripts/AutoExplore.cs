using Fungus.GameSystem;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public interface IAutoExplore
    {
        SeedTag GetSeedTag();

        bool IsStartPoint(int x, int y);
    }

    // http://www.roguebasin.com/index.php?title=Dijkstra_Maps_Visualized
    public class AutoExplore : MonoBehaviour
    {
        private readonly int gridSize = 10;
        private readonly int impassable = 999999;
        private readonly int notChecked = 111111;
        private readonly int startPoint = 0;
        private DungeonBoard board;
        private ConvertCoordinates coord;
        private int[,] distanceBoard;
        private RandomNumber random;
        private DungeonTerrain terrain;

        public int[] GetDestination()
        {
            distanceBoard = ResetBoard(out Stack<int[]> start);
            if (start.Count < 1)
            {
                return null;
            }

            SetDistance(distanceBoard, start);

            int[] source = coord.Convert(transform.position);
            int[] target = GetNewPosition(source);
            Debug.Log("X: " + source[0] + ", " + target[0]);
            Debug.Log("Y: " + source[1] + ", " + target[1]);
            return target;
        }

        private int GetNewDistance(int[,] dungeon, int[] check,
            out int[][] neighbor)
        {
            List<int[]> surround = coord.SurroundCoord(Surround.Diagonal, check);
            surround = board.FilterPositions(surround);
            neighbor = surround.ToArray();
            int min = dungeon[surround[0][0], surround[0][1]];

            foreach (int[] s in surround)
            {
                if (dungeon[s[0], s[1]] < min)
                {
                    min = dungeon[s[0], s[1]];
                }
            }
            return min + gridSize;
        }

        private int[] GetNewPosition(int[] currentPosition)
        {
            SeedTag tag = GetComponent<IAutoExplore>().GetSeedTag();

            List<int[]> surround = coord.SurroundCoord(
                Surround.Diagonal, currentPosition);
            surround = board.FilterPositions(surround);

            int minDistance = distanceBoard[surround[0][0], surround[0][1]];
            List<int[]> destination = new List<int[]>();

            foreach (int[] pos in surround)
            {
                if (distanceBoard[pos[0], pos[1]] < minDistance)
                {
                    minDistance = distanceBoard[pos[0], pos[1]];
                    destination.Clear();
                    destination.Add(pos);
                }
                else if (distanceBoard[pos[0], pos[1]] == minDistance)
                {
                    destination.Add(pos);
                }
            }

            return (destination.Count > 1)
                ? destination[random.Next(tag, 0, destination.Count)]
                : destination[0];
        }

        private int[,] ResetBoard(out Stack<int[]> start)
        {
            int[,] dungeon = new int[board.Width, board.Height];
            start = new Stack<int[]>();

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    if (GetComponent<IAutoExplore>().IsStartPoint(i, j))
                    {
                        dungeon[i, j] = startPoint;
                        start.Push(new int[] { i, j });
                    }
                    else if (!terrain.IsPassable(i, j))
                    {
                        dungeon[i, j] = impassable;
                    }
                    else
                    {
                        dungeon[i, j] = notChecked;
                    }
                }
            }
            return dungeon;
        }

        private void SetDistance(int[,] dungeon, Stack<int[]> start)
        {
            if (start.Count < 1)
            {
                return;
            }

            int[] check = start.Pop();
            int[][] neighbor;
            if (dungeon[check[0], check[1]] == notChecked)
            {
                dungeon[check[0], check[1]] = GetNewDistance(dungeon, check,
                    out neighbor);
            }
            else
            {
                GetNewDistance(dungeon, check, out neighbor);
            }

            foreach (int[] n in neighbor)
            {
                if (dungeon[n[0], n[1]] == notChecked)
                {
                    start.Push(n);
                }
            }

            SetDistance(dungeon, start);
        }

        private void Start()
        {
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            terrain = FindObjects.GameLogic.GetComponent<DungeonTerrain>();
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            random = FindObjects.GameLogic.GetComponent<RandomNumber>();

            distanceBoard = new int[board.Width, board.Height];
        }
    }
}
