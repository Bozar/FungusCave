using System.Collections.Generic;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public interface ISortTargets { List<GameObject> GetTargetsInSight(); }

    public class PCSortTargets : MonoBehaviour, ISortTargets
    {
        public List<GameObject> GetTargetsInSight()
        {
            //> Store actors in sight.
            //> Classify actors into 4 lists.
            //> Sort actors.
            //> Combine lists and return the result.

            return new List<GameObject>();
        }
    }
}
