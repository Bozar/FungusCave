using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.WorldBuilding;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Potion : MonoBehaviour, ISaveLoadActorData
    {
        private readonly string node = "Combat";
        private GameColor color;
        private UIMessage message;
        private PotionData potionData;
        private GameText text;

        public int CurrentPotion { get; private set; }

        public bool LoadedActorData { get; private set; }

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

            string drink = text.GetStringData(node, "Potion");
            drink = color.GetColorfulText(drink, ColorName.Orange);
            message.StoreText(drink);
            return;
        }

        public void GainPotion(int potion)
        {
            CurrentPotion = Math.Min(
                potionData.MaxPotion, CurrentPotion + potion);

            if (CurrentPotion == potionData.MaxPotion)
            {
                string full = text.GetStringData(node, "MaxPotion");
                full = color.GetColorfulText(full, ColorName.Green);
                message.StoreText(full);
            }
        }

        public void Load(DTActor data)
        {
            CurrentPotion = data.Potion;
            LoadedActorData = true;
        }

        public void LosePotion(int potion)
        {
            CurrentPotion = Math.Max(0, CurrentPotion - potion);
        }

        public void Save(DTActor data)
        {
            data.Potion = CurrentPotion;
        }

        private void Start()
        {
            potionData = FindObjects.GameLogic.GetComponent<PotionData>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            text = FindObjects.GameLogic.GetComponent<GameText>();
            color = FindObjects.GameLogic.GetComponent<GameColor>();

            if (!LoadedActorData)
            {
                CurrentPotion = potionData.StartPotion;
            }
        }
    }
}
