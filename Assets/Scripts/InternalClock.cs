using Fungus.Actor.AI;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
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
            GetComponent<Energy>().GainEnergy(energyTurn);

            if (GetComponent<Power>().PowerIsActive(PowerTag.DefEnergy1))
            {
                GetComponent<Energy>().GainEnergy(powerEnergy1);
            }

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

        private void Awake()
        {
            powerEnergy1 = 200;
        }

        private void Start()
        {
            energyTurn
                = FindObjects.GameLogic.GetComponent<EnergyData>().Restore;
        }
    }
}
