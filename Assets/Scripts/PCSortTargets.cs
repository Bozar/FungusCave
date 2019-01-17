using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public interface ISortTargets
    {
        List<GameObject> SortActorsInSight();
    }

    public class PCSortTargets : MonoBehaviour, ISortTargets
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private FieldOfView fov;

        private enum Side { Left, Right };

        public List<GameObject> SortActorsInSight()
        {
            List<GameObject> targets = FindTargets();
            if (targets.Count < 2)
            {
                return targets;
            }

            List<GameObject>[] splitted = SplitTargets(targets);
            List<GameObject> left = splitted[0];
            List<GameObject> right = splitted[1];

            left.Sort(SortLeft);
            right.Sort(SortRight);

            targets = left;
            foreach (GameObject target in right)
            {
                targets.Add(target);
            }
            return targets;
        }

        private List<GameObject> FindTargets()
        {
            int[] position = coord.Convert(FindObjects.PC.transform.position);
            int x = position[0];
            int y = position[1];
            int range = fov.MaxRange;

            List<GameObject> targets = new List<GameObject>();

            for (int i = x - range; i < x + range + 1; i++)
            {
                for (int j = y - range; j < y + range + 1; j++)
                {
                    if (!actor.HasActor(i, j)
                        || !fov.CheckFOV(FOVStatus.Insight, i, j)
                        || (i == x && j == y))
                    {
                        continue;
                    }
                    targets.Add(actor.GetActor(i, j));
                }
            }
            return targets;
        }

        private int SortLeft(GameObject previous, GameObject next)
        {
            return SortLeftOrRight(Side.Left, previous, next);
        }

        private int SortLeftOrRight(Side relativePosition,
            GameObject previous, GameObject next)
        {
            int[] prePos = coord.Convert(previous.transform.position);
            int[] nextPos = coord.Convert(next.transform.position);
            int[] sourcePos = coord.Convert(gameObject.transform.position);

            int preX = prePos[0];
            int preY = prePos[1];
            int nextX = nextPos[0];
            int nextY = nextPos[1];
            int sourceY = sourcePos[1];

            int preAbs;
            int nextAbs;

            // (0, 0) is at the bottom left corner. Actors farther away from the
            // source is greater.
            if (preX != nextX)
            {
                switch (relativePosition)
                {
                    case Side.Left:
                        if (preX < nextX)
                        {
                            return 1;
                        }
                        else if (preX > nextX)
                        {
                            return -1;
                        }
                        break;

                    case Side.Right:
                        if (preX > nextX)
                        {
                            return 1;
                        }
                        else if (preX < nextX)
                        {
                            return -1;
                        }
                        break;
                }
            }
            else if (preX == nextX)
            {
                preAbs = Math.Abs(preY - sourceY);
                nextAbs = Math.Abs(nextY - sourceY);

                if (preAbs > nextAbs)
                {
                    return 1;
                }
                else if (preAbs < nextAbs)
                {
                    return -1;
                }
                else if (preAbs == nextAbs)
                {
                    if (preY < 0)
                    {
                        return 1;
                    }
                    return -1;
                }
            }
            return 0;
        }

        private int SortRight(GameObject previous, GameObject next)
        {
            return SortLeftOrRight(Side.Right, previous, next);
        }

        private List<GameObject>[] SplitTargets(List<GameObject> targets)
        {
            int sourceX = coord.Convert(gameObject.transform.position)[0];

            int targetX;
            List<GameObject> left = new List<GameObject>();
            List<GameObject> right = new List<GameObject>();

            foreach (GameObject target in targets)
            {
                targetX = coord.Convert(target.transform.position)[0];

                if (targetX <= sourceX)
                {
                    left.Add(target);
                }
                else
                {
                    right.Add(target);
                }
            }

            return new List<GameObject>[] { left, right };
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            fov = FindObjects.PC.GetComponent<FieldOfView>();
        }
    }
}
