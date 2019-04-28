using Fungus.GameSystem;
using Fungus.GameSystem.Progress;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public class PCInternalClock : MonoBehaviour, IInternalClock
    {
        public void EndTurn()
        {
            GetComponent<TurnIndicator>().Count();
            GetComponent<TurnIndicator>().Trigger();
        }

        public void StartTurn()
        {
            FindObjects.GameLogic.GetComponent<SpawnBeetle>().BeetleEmerge();
        }
    }
}
