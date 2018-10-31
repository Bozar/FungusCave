﻿using System.Collections.Generic;
using UnityEngine;

public class FOVSimple : MonoBehaviour
{
    private DungeonBoard board;
    private Stack<int[]> checkPosition;
    private ConvertCoordinates coordinate;
    private FieldOfView fov;
    private bool fovTest;
    private int maxRange;
    private int[] position;
    private List<int[]> surround;
    private int x;
    private int y;

    public void UpdateFOV()
    {
        UpdatePosition();
        UpdateFOVBoard();
    }

    private void Awake()
    {
        maxRange = 5;
        checkPosition = new Stack<int[]>();
        fovTest = false;
    }

    private void Start()
    {
        coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        fov = gameObject.GetComponent<FieldOfView>();
    }

    private void UpdateFOVBoard()
    {
        bool walkable;
        bool gridChecked;

        if (checkPosition.Count < 1)
        {
            return;
        }

        position = checkPosition.Pop();
        x = position[0];
        y = position[1];

        surround = coordinate.SurroundCoord(
            Surround.Diagonal, x, y);

        foreach (var grid in surround)
        {
            if (!board.IsInsideRange(FOVShape.Rhombus,
                maxRange,
                coordinate.Convert(gameObject.transform.position),
                grid))
            {
                continue;
            }

            walkable = board.CheckBlock(DungeonBlock.Floor, grid)
                || board.CheckBlock(DungeonBlock.Pool, grid);

            gridChecked = fovTest
                ? (fov.CheckFOV(grid) != FOVStatus.TEST)
                : (fov.CheckFOV(grid) != FOVStatus.Insight);

            if (walkable && gridChecked)
            {
                checkPosition.Push(grid);
            }

            if (fovTest)
            {
                fov.ChangeFOVBoard(FOVStatus.TEST, grid);
            }
            else
            {
                fov.ChangeFOVBoard(FOVStatus.Insight, grid);
            }
        }

        UpdateFOVBoard();
    }

    private void UpdatePosition()
    {
        position = coordinate.Convert(gameObject.transform.position);
        checkPosition.Push(position);
    }
}
