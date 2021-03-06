﻿using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.FOV
{
    public class FOVSimple : MonoBehaviour, IFOVAlgorithm
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
            checkPosition = new Stack<int[]>();
            //fovTest = true;
            fovTest = false;
        }

        private void Start()
        {
            coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
            fov = GetComponent<FieldOfView>();

            maxRange = fov.MaxRange;
        }

        private void UpdateFOVBoard()
        {
            bool passable;
            bool gridChecked;

            if (checkPosition.Count < 1)
            {
                return;
            }

            position = checkPosition.Pop();
            x = position[0];
            y = position[1];

            surround = coordinate.SurroundCoord(Surround.Diagonal, x, y);

            foreach (var grid in surround)
            {
                if (!board.IsInsideRange(FOVShape.Rhombus,
                    maxRange,
                    coordinate.Convert(transform.position),
                    grid))
                {
                    continue;
                }

                passable = board.CheckBlock(SubObjectTag.Floor, grid)
                    || board.CheckBlock(SubObjectTag.Pool, grid);

                gridChecked = fovTest
                    ? (!fov.CheckFOV(FOVStatus.TEST, grid))
                    : (!fov.CheckFOV(FOVStatus.Insight, grid));

                if (passable && gridChecked)
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
            position = coordinate.Convert(transform.position);
            checkPosition.Push(position);
        }
    }
}
