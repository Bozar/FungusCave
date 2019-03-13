using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Potion : MonoBehaviour
    {
        private UIMessage message;
        private PotionData potionData;

        public int CurrentPotion { get; private set; }

        public void DrinkPotion()
        {
            if (CurrentPotion < 1)
            {
                return;
            }

            if (GetComponent<Infection>().HasInfection(InfectionTag.Mutated))
            {
                GetComponent<HP>().GainHP(potionData.ReducedHP);
            }
            else
            {
                GetComponent<HP>().GainHP(GetComponent<HP>().MaxHP);
            }

            GetComponent<Infection>().ResetInfection();
            GetComponent<Stress>().LoseStress(potionData.RelieveStress);
            GetComponent<Energy>().GainEnergy(potionData.RestoreEnergy);
            LosePotion(1);

            message.StoreText("You inject yourself with a blood potion.");
            return;
        }

        public void GainPotion(int potion)
        {
            CurrentPotion = Math.Min(
                potionData.MaxPotion, CurrentPotion + potion);
        }

        public void LosePotion(int potion)
        {
            CurrentPotion = Math.Max(0, CurrentPotion - potion);
        }

        private void Start()
        {
            potionData = FindObjects.GameLogic.GetComponent<PotionData>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();

            CurrentPotion = potionData.StartPotion;
        }
    }
}
