using UnityEngine;

namespace Fungus.Actor.Turn
{
    public interface ITurnCounter
    {
        void Count();

        void Trigger();
    }

    public class InternalClock : MonoBehaviour
    {
        public void EndTurn()
        {
            GetComponent<Infection>().Count();
            GetComponent<TurnIndicator>()?.Count();
            GetComponent<TurnIndicator>()?.Trigger();
        }

        public void StartTurn()
        {
            GetComponent<Energy>().Trigger();
            GetComponent<IHP>().Trigger();
        }
    }
}
