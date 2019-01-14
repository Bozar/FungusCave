using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public interface ISortTargets
    {
        List<GameObject> GetTargetsInSight();
    }

    public class PCSortTargets : MonoBehaviour, ISortTargets
    {
        private ActorBoard actor;
        private ConvertCoordinates coord;
        private FieldOfView fov;

        public List<GameObject> GetTargetsInSight()
        {
            //> Sort actors.
            //> Combine lists and return the result.

            List<GameObject> targets = FindTargets();

            if (targets.Count < 2)
            {
                return targets;
            }

            List<GameObject>[] splitted = SplitTargets(targets);
            List<GameObject> left = splitted[0];
            List<GameObject> right = splitted[1];

            return targets;
        }

        private List<GameObject> FindTargets()
        {
            int[] position = coord.Convert(transform.position);
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

            fov = GetComponent<FieldOfView>();
        }
    }
}
