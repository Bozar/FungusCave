using Fungus.Actor.AI;
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

            // TODO: Update after Unity 2018.3.
            if (GetComponent<TurnIndicator>() != null)
            {
                GetComponent<TurnIndicator>().Count();
            }
        }

        public void StartTurn()
        {
            GetComponent<Energy>().Trigger();

            // TODO: Update after Unity 2018.3.
            if (GetComponent<PCAutoExplore>() != null)
            {
                GetComponent<PCAutoExplore>().Count();
            }

            //if (GetComponent<NPCMemory>() != null)
            //{
            //    GetComponent<NPCMemory>().Trigger();
            //    GetComponent<NPCMemory>().Count();
            //}
        }
    }
}
