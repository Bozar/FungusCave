using Fungus.Actor;
using Fungus.Actor.AI;
using Fungus.Actor.Turn;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.WorldBuilding;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Fungus.GameSystem.Render
{
    public enum UITag
    {
        NONE, Seed, Message, Modeline,
        ExamineMessage, ExamineModeline,
        HPData, StressData, PotionData, DamageData,
        Turn, Terrain,
        PowerLabel, PowerData0, PowerData1, PowerData2,
        InfectionLabel,
        InfectionName0, InfectionDuration0,
        InfectionName1, InfectionDuration1
    };

    public class UserInterface : MonoBehaviour
    {
        private DungeonBoard board;
        private GameColor color;
        private UIDict getUI;
        private GameObject pc;
        private StringBuilder sb;

        private delegate GameObject UIDict(UITag tag);

        private void Awake()
        {
            sb = new StringBuilder();
        }

        private void LateUpdate()
        {
            if (pc == null)
            {
                pc = GameObject.FindGameObjectWithTag("PC");
            }

            UpdateHP();
            UpdateStress();
            UpdatePotion();
            UpdateDamage();
            UpdateEnvironment();
            UpdateSeed();
            UpdateInfection();
            UpdatePower();
            UpdateTurn();

            GetComponent<UIMessage>().PrintText();
        }

        private void Start()
        {
            board = GetComponent<DungeonBoard>();
            color = GetComponent<GameColor>();
            getUI = FindObjects.GetUIObject;

            TurnOffUIElements();
        }

        private void TurnOffUIElements()
        {
            getUI(UITag.ExamineMessage).SetActive(false);
        }

        private void UpdateDamage()
        {
            int current = pc.GetComponent<Attack>().GetCurrentDamage();

            getUI(UITag.DamageData).GetComponent<Text>().text
                = current.ToString();
        }

        private void UpdateEnvironment()
        {
            // TODO: check the weather.
            bool hasFog = false;
            sb = sb.Remove(0, sb.Length);
            sb.Append("[ ");

            if (pc.GetComponent<AIVision>().CanSeeTarget(MainObjectTag.Actor))
            {
                sb.Append("@");
            }

            if (board.CheckBlock(SubObjectTag.Pool, pc.transform.position))
            {
                if (sb.Length > 2)
                {
                    sb.Append(" | ");
                }
                sb.Append("Pool");
            }

            if (hasFog)
            {
                if (sb.Length > 2)
                {
                    sb.Append(" | ");
                }
                sb.Append("Fog");
            }
            sb.Append(" ]");

            getUI(UITag.Terrain).GetComponent<Text>().text = sb.ToString();
        }

        private void UpdateHP()
        {
            int current = pc.GetComponent<HP>().CurrentHP;
            int max = pc.GetComponent<HP>().MaxHP;

            getUI(UITag.HPData).GetComponent<Text>().text = current + "/" + max;
        }

        private void UpdateInfection()
        {
            int lineNumber = -1;
            int current;
            string name;
            string duration;
            UITag tagName;
            UITag tagDuration;

            getUI(UITag.InfectionLabel).GetComponent<Text>().text = "";

            for (int i = 0; i < 2; i++)
            {
                name = "InfectionName" + i;
                duration = "InfectionDuration" + i;

                tagName = (UITag)Enum.Parse(typeof(UITag), name);
                tagDuration = (UITag)Enum.Parse(typeof(UITag), duration);

                getUI(tagName).GetComponent<Text>().text = "";
                getUI(tagDuration).GetComponent<Text>().text = "";
            }

            foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
            {
                if (pc.GetComponent<Infection>().HasInfection(tag, out current))
                {
                    lineNumber++;

                    name = "InfectionName" + lineNumber;
                    duration = "InfectionDuration" + lineNumber;

                    tagName = (UITag)Enum.Parse(typeof(UITag), name);
                    tagDuration = (UITag)Enum.Parse(typeof(UITag), duration);

                    getUI(tagName).GetComponent<Text>().text = tag.ToString();
                    getUI(tagDuration).GetComponent<Text>().text
                        = current.ToString();
                }
            }

            if (lineNumber > -1)
            {
                getUI(UITag.InfectionLabel).GetComponent<Text>().text
                    = "[ Infections ]";
            }
        }

        private void UpdatePotion()
        {
            int current = pc.GetComponent<Potion>().CurrentPotion;

            getUI(UITag.PotionData).GetComponent<Text>().text
                = current.ToString();
        }

        private void UpdatePower()
        {
            int max = pc.GetComponent<Stress>().MaxStress;
            string power;
            UITag tagPower;

            for (int i = 0; i < max; i++)
            {
                power = "PowerData" + i;
                tagPower = (UITag)Enum.Parse(typeof(UITag), power);

                getUI(tagPower).GetComponent<Text>().text
                    = pc.GetComponent<Power>().GetPowerName((PowerSlotTag)i);

                getUI(tagPower).GetComponent<Text>().color
                    = pc.GetComponent<Power>().SlotIsActive((PowerSlotTag)i)
                    ? color.PickColor(ColorName.White)
                    : color.PickColor(ColorName.Grey);
            }

            if (pc.GetComponent<Power>().HasPower())
            {
                getUI(UITag.PowerLabel).GetComponent<Text>().text = "[ Powers ]";
            }
        }

        private void UpdateSeed()
        {
            string seed = FindObjects.GameLogic.GetComponent<RandomNumber>()
                .RootSeed.ToString();
            int seedLength = seed.Length;

            for (int i = 1; seedLength > i * 3; i++)
            {
                seed = seed.Insert(i * 4 - 1, "-");
            }

            getUI(UITag.Seed).GetComponent<Text>().text = seed;
        }

        private void UpdateStress()
        {
            int current = pc.GetComponent<Stress>().CurrentStress;
            int max = pc.GetComponent<Stress>().MaxStress;

            getUI(UITag.StressData).GetComponent<Text>().text
                = current + "/" + max;
        }

        private void UpdateTurn()
        {
            int current = pc.GetComponent<TurnIndicator>().CurrentTurn;
            int max = pc.GetComponent<TurnIndicator>().Seperator;
            sb = sb.Remove(0, sb.Length);

            sb.Append("[ ");

            if (current > 0)
            {
                for (int i = 0; i < current; i++)
                {
                    sb.Append("X ");

                    if ((current > max) && (i + 1 == max))
                    {
                        sb.Append("| ");
                    }
                }
                sb.Append("]");
            }
            else
            {
                sb.Append(" ]");
            }

            getUI(UITag.Turn).GetComponent<Text>().text = sb.ToString();
        }
    }
}
