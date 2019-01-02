using Fungus.Actor.FOV;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class PCAutoExplore : MonoBehaviour, IAutoExplore
    {
        public SeedTag GetSeedTag()
        {
            return SeedTag.AutoExplore;
        }

        public bool GetStartPoint(out int[] startPoint)
        {
            startPoint = new int[2];
            return false;
        }

        public bool IsStartPoint(int[] position)
        {
            return GetComponent<FieldOfView>().CheckFOV(
                FOVStatus.Unknown, position);
        }
    }
}
