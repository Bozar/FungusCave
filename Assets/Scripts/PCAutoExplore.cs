using Fungus.Actor.FOV;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class PCAutoExplore : MonoBehaviour, IAutoExplore, ITurnCounter
    {
        private int countAutoExplore;
        private int maxCount;

        public bool ContinueAutoExplore { get { return countAutoExplore > 0; } }

        public void Count()
        {
            if (countAutoExplore > 0)
            {
                countAutoExplore--;
            }
        }

        public SeedTag GetSeedTag()
        {
            return SeedTag.AutoExplore;
        }

        public bool IsStartPoint(int x, int y)
        {
            return GetComponent<FieldOfView>().CheckFOV(
                FOVStatus.Unknown, new int[] { x, y });
        }

        public void Trigger()
        {
            countAutoExplore = maxCount;
        }

        private void Awake()
        {
            maxCount = 20;
        }

        private void StopAutoExplore()
        {
            countAutoExplore = 0;
        }
    }
}
