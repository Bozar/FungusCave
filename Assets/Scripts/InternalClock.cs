using UnityEngine;

namespace Fungus.Actor.Turn
{
    public interface IInternalClock
    {
        void EndTurn();

        void StartTurn();
    }

    public interface ITurnCounter
    {
        void Count();

        void Trigger();
    }

    public class InternalClock : MonoBehaviour
    {
        public void EndTurn()
        {
            GetComponent<IInternalClock>().EndTurn();

            GetComponent<Infection>().Count();
        }

        public void StartTurn()
        {
            GetComponent<IInternalClock>().StartTurn();

            GetComponent<Energy>().Trigger();
            GetComponent<IHP>().Trigger();
        }
    }
}
