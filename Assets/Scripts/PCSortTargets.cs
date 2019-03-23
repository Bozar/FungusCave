using Fungus.Actor.FOV;
using Fungus.GameSystem;
using Fungus.GameSystem.WorldBuilding;
using System.Collections.Generic;
using System.Linq;
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

            IEnumerable<GameObject> sorted
                = from n in targets
                  orderby (coord.Convert(n.transform.position)[0]
                  > coord.Convert(FindObjects.PC.transform.position)[0])
                  select n;

            targets = new List<GameObject> { };
            foreach (GameObject go in sorted)
            {
                targets.Add(go);
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

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            actor = FindObjects.GameLogic.GetComponent<ActorBoard>();
            fov = FindObjects.PC.GetComponent<FieldOfView>();
        }
    }
}
