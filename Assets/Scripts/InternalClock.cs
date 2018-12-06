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
            gameObject.GetComponent<Infection>().Count();

            // TODO: Update after Unity 2018.3.
            if (gameObject.GetComponent<TurnIndicator>() != null)
            {
                gameObject.GetComponent<TurnIndicator>().Count();
            }
        }

        public void StartTurn()
        {
            gameObject.GetComponent<Energy>().GainEnergy(energyTurn, true);

            if (gameObject.GetComponent<Power>().PowerIsActive(
                PowerTag.Energy1))
            {
                gameObject.GetComponent<Energy>().GainEnergy(
                    powerEnergy1, false);
            }

            // TODO: Update after Unity 2018.3.
            if (gameObject.GetComponent<AutoExplore>() != null)
            {
                gameObject.GetComponent<AutoExplore>().Count();
            }
        }

        private void Awake()
        {
            energyTurn = 2000;
            powerEnergy1 = 200;
        }
    }
}
