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

        bool IsValidDestination(int[] check);
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
        private int[,] distance;
        private RandomNumber random;
        private DungeonTerrain terrain;

        public int[] GetDestination()
        {
            distance = ResetBoard(out Stack<int[]> start);
            if (start.Count < 1)
            {
                return null;
            }

            SetDistance(distance, start);

            int[] source = coord.Convert(transform.position);
            int[] target = GetTarget(source);
            return target;
        }

        private int GetDistance(int[,] dungeon, int[] check)
        {
            int[][] neighbor = GetNeighbor(check);
            int min = GetMinDistance(dungeon, neighbor);

            return min + gridSize;
        }

        private int GetMinDistance(int[,] dungeon, int[][] check)
        {
            int min = dungeon[check[0][0], check[0][1]];

            foreach (int[] c in check)
            {
                if (dungeon[c[0], c[1]] < min)
                {
                    min = dungeon[c[0], c[1]];
                }
            }
            return min;
        }

        private int[][] GetNeighbor(int[] center)
        {
            List<int[]> neighbor = coord.SurroundCoord(Surround.Diagonal, center);
            neighbor = board.FilterPositions(neighbor);

            return neighbor.ToArray();
        }

        private int[] GetTarget(int[] source)
        {
            int[][] neighbor = GetNeighbor(source);
            int min = GetMinDistance(distance, neighbor);
            List<int[]> targets = new List<int[]>();

            foreach (int[] n in neighbor)
            {
                if (distance[n[0], n[1]] == min)
                {
                    targets.Add(n);
                }
            }

            SeedTag tag = GetComponent<IAutoExplore>().GetSeedTag();
            int[] target = (targets.Count > 1)
                ? targets[random.Next(tag, 0, targets.Count)]
                : targets[0];
            // The actor might dance around two grids of the same distance
            // repeatedly. We need to at least prevent PC from doing this.
            if (GetComponent<IAutoExplore>().IsValidDestination(target))
            {
                return target;
            }
            return null;
        }

        private int[,] ResetBoard(out Stack<int[]> start)
        {
            int[,] dungeon = new int[board.Width, board.Height];
            start = new Stack<int[]>();

            for (int i = 0; i < board.Width; i++)
            {
                for (int j = 0; j < board.Height; j++)
                {
                    // A start point might be impassable.
                    // 1) NPC's start point is PC's current position.
                    // 2) PC's start points are unexplored positions in the map.
                    if (GetComponent<IAutoExplore>().IsStartPoint(i, j))
                    {
                        dungeon[i, j] = startPoint;
                        start.Push(new int[] { i, j });
                    }
                    else if (terrain.IsPassable(i, j))
                    {
                        dungeon[i, j] = notChecked;
                    }
                    else
                    {
                        dungeon[i, j] = impassable;
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
            if (dungeon[check[0], check[1]] == notChecked)
            {
                dungeon[check[0], check[1]] = GetDistance(dungeon, check);
            }

            int[][] neighbor = GetNeighbor(check);
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

            distance = new int[board.Width, board.Height];
        }
    }
}
