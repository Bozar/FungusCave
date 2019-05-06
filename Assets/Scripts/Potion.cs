using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.WorldBuilding;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Potion : MonoBehaviour, ILoadActorData
    {
        private GameColor color;
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

            string drink = text.GetStringData("Combat", "Potion");
            drink = color.GetColorfulText(drink, ColorName.Green);
            message.StoreText(drink);
            return;
        }

        public void GainPotion(int potion)
        {
            CurrentPotion = Math.Min(
                potionData.MaxPotion, CurrentPotion + potion);
        }

        public void Load(DTActor data)
        {
            CurrentPotion = data.Potion;
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
            color = FindObjects.GameLogic.GetComponent<GameColor>();

            CurrentPotion = potionData.StartPotion;
        }
    }
}
