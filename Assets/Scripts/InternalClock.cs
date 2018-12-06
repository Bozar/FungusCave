using Fungus.Actor.AI;
using UnityEngine;

namespace Fungus.Actor.Turn
{
    public interface ITurnCounter { void Count(); }

    public class InternalClock : MonoBehaviour
    {
        private int energyTurn;
        private int powerEnergy1;

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
            GetComponent<Energy>().GainEnergy(energyTurn, true);

            if (GetComponent<Power>().PowerIsActive(PowerTag.Energy1))
            {
                GetComponent<Energy>().GainEnergy(powerEnergy1, false);
            }

            // TODO: Update after Unity 2018.3.
            if (GetComponent<AutoExplore>() != null)
            {
                GetComponent<AutoExplore>().Count();
            }
        }

        private void Awake()
        {
            energyTurn = 2000;
            powerEnergy1 = 200;
        }
    }
}
