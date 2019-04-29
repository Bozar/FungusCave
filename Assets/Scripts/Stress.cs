using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Stress : MonoBehaviour, IResetData
    {
        private ActorData actorData;

        public int CurrentStress { get; private set; }

        public int MaxStress
        {
            get
            {
                int stress = actorData.GetIntData(
                    GetComponent<MetaInfo>().SubTag, DataTag.Stress);

                if ((GetComponent<Power>() != null)
                    && GetComponent<Power>().IsActive(PowerTag.DefInfection1))
                {
                    stress += actorData.GetIntData(
                        GetComponent<MetaInfo>().SubTag, DataTag.BonusStress);
                }

                return stress;
            }
        }

        public void GainStress(int stress)
        {
            GetComponent<ICombatMessage>().IsStressed();
            CurrentStress = Math.Min(MaxStress, CurrentStress + stress);
        }

        public bool HasMaxStress()
        {
            return CurrentStress == MaxStress;
        }

        public void LoseStress(int stress)
        {
            CurrentStress = Math.Max(0, CurrentStress - stress);
        }

        public void Reset()
        {
            CurrentStress = 0;
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();

            Reset();
        }
    }
}
