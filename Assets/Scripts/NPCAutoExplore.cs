using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class NPCAutoExplore : MonoBehaviour, IAutoExplore
    {
        public int GetDistance()
        {
            return 0;
        }

        public SeedTag GetSeedTag()
        {
            return SeedTag.NPCAction;
        }

        public bool GetStartPoint(out int[] startPoint)
        {
            startPoint = new int[2];
            return false;
        }
    }
}
