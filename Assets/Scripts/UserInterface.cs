using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private DungeonBoard board;
    private GameColor color;
    private int current;
    private UIDict getUI;
    private int lineNumber;
    private int max;
    private GameObject pc;
    private string printText;
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

        FindObjects.GameLogic.GetComponent<UIMessage>().PrintText();
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        color = FindObjects.GameLogic.GetComponent<GameColor>();
        getUI = FindObjects.GetUIObject;
    }

    private void UpdateDamage()
    {
        current = pc.GetComponent<Attack>().GetCurrentDamage();

        getUI(UITag.DamageData).GetComponent<Text>().text = current.ToString();
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
        current = pc.GetComponent<HP>().CurrentHP;
        max = pc.GetComponent<HP>().MaxHP;

        getUI(UITag.HPData).GetComponent<Text>().text = current + "/" + max;
    }

    private void UpdateInfection()
    {
        lineNumber = 0;

        getUI(UITag.InfectionLabel).GetComponent<Text>().text = "";

        for (int i = 1; i < 3; i++)
        {
            getUI((UITag)Enum.Parse(typeof(UITag), "InfectionName" + i))
                .GetComponent<Text>().text = "";
            getUI((UITag)Enum.Parse(typeof(UITag), "InfectionDuration" + i))
                .GetComponent<Text>().text = "";
        }

        foreach (InfectionTag tag in Enum.GetValues(typeof(InfectionTag)))
        {
            if (pc.GetComponent<Infection>().HasInfection(tag, out current))
            {
                lineNumber++;

                getUI((UITag)Enum.Parse(typeof(UITag),
                    "InfectionName" + lineNumber)).GetComponent<Text>().text
                    = tag.ToString();
                getUI((UITag)Enum.Parse(typeof(UITag),
                    "InfectionDuration" + lineNumber)).GetComponent<Text>().text
                    = current.ToString();
            }
        }

        if (lineNumber > 0)
        {
            getUI(UITag.InfectionLabel).GetComponent<Text>().text
                = "[ Infections ]";
        }
    }

    private void UpdatePotion()
    {
        current = pc.GetComponent<Potion>().CurrentPotion;

        getUI(UITag.PotionData).GetComponent<Text>().text = current.ToString();
    }

    private void UpdatePower()
    {
        List<string> powers = new List<string>
        {
            "Power01",
            "Power02",
            "Power03"
        };

        for (int i = 1; i < 4; i++)
        {
            getUI((UITag)Enum.Parse(typeof(UITag), "PowerData" + i))
                .GetComponent<Text>().text = powers[i - 1];

            getUI((UITag)Enum.Parse(typeof(UITag), "PowerData" + i))
                .GetComponent<Text>().color
                = i > pc.GetComponent<Stress>().CurrentStress
                ? color.PickColor(ColorName.Grey)
                : color.PickColor(ColorName.White);
        }
    }

    private void UpdateSeed()
    {
        int textLength;

        printText = FindObjects.GameLogic.GetComponent<RandomNumber>().RootSeed
            .ToString();
        textLength = printText.Length;

        for (int i = 1; textLength > i * 3; i++)
        {
            printText = printText.Insert(i * 4 - 1, "-");
        }

        getUI(UITag.Seed).GetComponent<Text>().text = printText;
    }

    private void UpdateStress()
    {
        current = pc.GetComponent<Stress>().CurrentStress;
        max = pc.GetComponent<Stress>().MaxStress;

        getUI(UITag.StressData).GetComponent<Text>().text = current + "/" + max;
    }
}
