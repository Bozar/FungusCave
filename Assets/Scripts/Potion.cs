using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Potion : MonoBehaviour
    {
        private UIMessage message;
        private PotionData potionData;
        private GameText text;

        public int CurrentPotion { get; private set; }

        public void DrinkPotion()
        {
            if (CurrentPotion < 1)
            {
                return;
            }

            int hp = GetComponent<Infection>().HasInfection(InfectionTag.Mutated)
                ? potionData.MutatedHP
                : potionData.RestoreHP;
            GetComponent<HP>().GainHP(hp);

            GetComponent<Infection>().ResetInfection();
            GetComponent<Stress>().LoseStress(potionData.RelieveStress);
            GetComponent<Energy>().GainEnergy(potionData.RestoreEnergy);
            LosePotion(1);

            message.StoreText(text.GetStringData("Combat", "Potion"));
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
            text = FindObjects.GameLogic.GetComponent<GameText>();

            CurrentPotion = potionData.StartPotion;
        }
    }
}
