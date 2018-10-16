﻿using UnityEngine;

// Change the 2D array's content, which is defined in DungeonBoard.
public class DungeonBlueprint : MonoBehaviour
{
    protected DungeonBoard board;
    protected RandomNumber random;

    public interface IIsEmptyArea
    {
        bool IsEmptyArea(int x, int y, int width, int height);
    }

    public void DrawManually()
    {
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 4, 4);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 5, 5);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, 6, 6);

        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall,
            board.Width - 1, board.Height - 1);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Wall,
            board.Width, board.Height - 1);

        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 8, 8);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 8, 9);
        board.ChangeBlock(DungeonBoard.DungeonBlock.Pool, 9, 9);
    }

    public void DrawRandomly()
    {
        int wall = 5;
        int count = 0;
        int[] index;
        int x;
        int y;

        while (count < wall)
        {
            index = RandomIndex();
            x = index[0];
            y = index[1];

            if (board.CheckTerrain(DungeonBoard.DungeonBlock.Floor, x, y))
            {
                board.ChangeBlock(DungeonBoard.DungeonBlock.Wall, x, y);
                count++;
            }
        }
    }

    protected int[] RandomIndex()
    {
        int[] index;
        int x;
        int y;

        x = random.RNG.Next(0, board.Width);
        y = random.RNG.Next(0, board.Height);
        index = new int[] { x, y };

        return index;
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        random = FindObjects.GameLogic.GetComponent<RandomNumber>();
    }
}
