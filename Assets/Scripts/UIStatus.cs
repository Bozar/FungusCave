using Fungus.Actor;
using Fungus.Actor.AI;
using Fungus.Actor.Turn;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Progress;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public class UIStatus : MonoBehaviour, IUpdateUI
    {
        private UIText getText;
        private string node;

        private delegate Text UIText(UITag tag);

        public void PrintStaticText()
        {
            return;
        }

        public void PrintStaticText(string text)
        {
            return;
        }

        public void PrintText()
        {
            UpdateDungeonLevel();

            UpdateHP();
            UpdateStress();
            UpdatePotion();
            UpdateDamage();

            UpdateEnvironment();
            UpdateTurn();

            UpdatePower();
            UpdateInfection();

            UpdateHelp();
            UpdateVersion();
            UpdateSeed();
        }

        private void Awake()
        {
            node = "Status";
        }

        private string ColorfulLowHP(string text)
        {
            if (FindObjects.PC.GetComponent<HP>().CurrentHP
                <= GetComponent<GameSetting>().LowHP)
            {
                text = GetComponent<GameColor>().GetColorfulText(text,
                    ColorName.Orange);
            }
            else
            {
                text = GetComponent<GameColor>().GetColorfulText(text,
                    ColorName.White);
            }
            return text;
        }

        private string JoinCurrentMax(int current, int max)
        {
            return current + " / " + max;
        }

        private void Start()
        {
            getText = FindObjects.GetUIText;
        }

        private void UpdateDamage()
        {
            int current = FindObjects.PC.GetComponent<IDamage>().CurrentDamage;

            getText(UITag.DamageLabel).text
              = GetComponent<GameText>().GetStringData(node, "Damage");
            getText(UITag.DamageData).text = current.ToString();
        }

        private void UpdateDungeonLevel()
        {
            string dungeon = GetComponent<GameText>().GetStringData(node,
                "DungeonLevel");
            string level = GetComponent<DungeonProgressData>().GetDungeonLevel()
                .ToString();

            // DL1
            level = level.Replace("DL", "");
            // Cave - %num%
            dungeon = dungeon.Replace("%num%", level);

            getText(UITag.DungeonLevel).text = dungeon;
        }

        private void UpdateEnvironment()
        {
            string env = "[ %str1%%str2%%str3% ]";
            bool enemy = false;
            bool pool = false;

            if (FindObjects.PC.GetComponent<AIVision>().CanSeeTarget(
                MainObjectTag.Actor))
            {
                enemy = true;
                env = env.Replace("%str1%",
                    GetComponent<GameText>().GetStringData(node,
                    "EnemyIcon"));
            }
            else
            {
                env = env.Replace("%str1%", "");
            }

            if (GetComponent<DungeonBoard>().CheckBlock(SubObjectTag.Pool,
                FindObjects.PC.transform.position))
            {
                pool = true;
                env = env.Replace("%str3%",
                    GetComponent<GameText>().GetStringData(node,
                    "PoolIcon"));
            }
            else
            {
                env = env.Replace("%str3%", "");
            }

            if (enemy && pool)
            {
                env = env.Replace("%str2%", " | ");
            }
            else
            {
                env = env.Replace("%str2%", "");
            }

            getText(UITag.Terrain).text = env;
        }

        private void UpdateHelp()
        {
            getText(UITag.Help).text
                = GetComponent<GameText>().GetStringData(node, "Help");
        }

        private void UpdateHP()
        {
            int current = FindObjects.PC.GetComponent<HP>().CurrentHP;
            int max = FindObjects.PC.GetComponent<HP>().MaxHP;
            string text = JoinCurrentMax(current, max);
            text = ColorfulLowHP(text);

            getText(UITag.HPLabel).text
                = GetComponent<GameText>().GetStringData(node, "HP");
            getText(UITag.HPData).text = text;
        }

        private void UpdateInfection()
        {
            string label = "[ %str% ]";
            label = label.Replace("%str%",
                GetComponent<GameText>().GetStringData(node, "Infection"));

            if (FindObjects.PC.GetComponent<Infection>().HasInfection(
                out InfectionTag tag, out int duration))
            {
                getText(UITag.InfectionName).text
                    = GetComponent<InfectionData>().GetInfectionName(tag);
                getText(UITag.InfectionDuration).text = duration.ToString();
                getText(UITag.InfectionLabel).text = label;
            }
            else
            {
                getText(UITag.InfectionName).text = "";
                getText(UITag.InfectionDuration).text = "";
                getText(UITag.InfectionLabel).text = "";
            }
        }

        private void UpdatePotion()
        {
            int current = FindObjects.PC.GetComponent<Potion>().CurrentPotion;
            int max = GetComponent<PotionData>().MaxPotion;

            getText(UITag.PotionLabel).text
               = GetComponent<GameText>().GetStringData(node, "Potion");
            getText(UITag.PotionData).text = JoinCurrentMax(current, max);
        }

        private void UpdatePower()
        {
            bool hasPower = false;
            string uiName = "PowerData";
            UITag uiTag;

            foreach (PowerSlotTag slot in Enum.GetValues(typeof(PowerSlotTag)))
            {
                if (FindObjects.PC.GetComponent<Power>().HasPower(slot,
                    out PowerTag power, out bool isActive))
                {
                    hasPower = true;

                    uiTag = (UITag)Enum.Parse(typeof(UITag), uiName + (int)slot);

                    getText(uiTag).text
                        = GetComponent<PowerData>().GetPowerName(power);

                    getText(uiTag).color = isActive
                        ? GetComponent<GameColor>().PickColor(ColorName.White)
                        : GetComponent<GameColor>().PickColor(ColorName.Grey);
                }
            }

            if (hasPower)
            {
                string label = "[ %str% ]";
                label = label.Replace("%str%",
                    GetComponent<GameText>().GetStringData(node, "Power"));
                getText(UITag.PowerLabel).text = label;
            }
        }

        private void UpdateSeed()
        {
            string seed = GetComponent<RandomNumber>().RootSeed.ToString();
            int seedLength = seed.Length;

            for (int i = 1; seedLength > i * 3; i++)
            {
                seed = seed.Insert(i * 4 - 1, "-");
            }

            getText(UITag.Seed).text = seed;
        }

        private void UpdateStress()
        {
            int current = FindObjects.PC.GetComponent<Stress>().CurrentStress;
            int max = FindObjects.PC.GetComponent<Stress>().MaxStress;

            getText(UITag.StressLabel).text
               = GetComponent<GameText>().GetStringData(node, "Stress");
            getText(UITag.StressData).text = JoinCurrentMax(current, max);
        }

        private void UpdateTurn()
        {
            int current = FindObjects.PC.GetComponent<TurnIndicator>()
                .CurrentTurn;
            int max = FindObjects.PC.GetComponent<TurnIndicator>()
                .BarSpearator;
            Queue<string> turn = new Queue<string>();

            for (int i = 0; i < current; i++)
            {
                turn.Enqueue(GetComponent<GameText>().GetStringData(node,
                    "TurnIcon"));
                if ((current > max) && (i + 1 == max))
                {
                    turn.Enqueue("|");
                }
            }
            string indicator = "[ " + string.Join(" ", turn) + " ]";

            getText(UITag.Turn).text = indicator;
        }

        private void UpdateVersion()
        {
            string version = FindObjects.Version;
            if (GetComponent<WizardMode>().IsWizardMode)
            {
                version = GetComponent<GameText>().GetStringData(node,
                    "WizardIcon") + version;
            }

            getText(UITag.Version).text = version;
        }
    }
}
