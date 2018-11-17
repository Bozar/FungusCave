﻿using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private DungeonBoard board;
    private UIDict getUI;
    private bool hasFog;
    private GameObject pc;
    private string printText;
    private StringBuilder sb;
    private int textLength;

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

        UpdateSeed();
        UpdateEnvironment();
        FindObjects.GameLogic.GetComponent<UIMessage>().PrintText();
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        getUI = FindObjects.GetUIObject;
    }

    private void UpdateEnvironment()
    {
        // TODO: check the weather.
        hasFog = false;
        sb = sb.Remove(0, sb.Length);
        sb.Append("[ ");

        if (board.CheckBlock(SubObjectTag.Floor, pc.transform.position))
        {
            sb.Append("Floor");
        }
        else if (board.CheckBlock(SubObjectTag.Pool, pc.transform.position))
        {
            sb.Append("Pool");
        }

        if (hasFog)
        {
            sb.Append(" | ");
            sb.Append("Fog");
        }
        sb.Append(" ]");

        getUI(UITag.Terrain).GetComponent<Text>().text = sb.ToString();
    }

    private void UpdateSeed()
    {
        printText = FindObjects.GameLogic.GetComponent<RandomNumber>()
            .RootSeed.ToString();
        textLength = printText.Length;

        for (int i = 1; textLength > i * 3; i++)
        {
            printText = printText.Insert(i * 4 - 1, "-");
        }

        getUI(UITag.Seed).GetComponent<Text>().text = printText;
    }
}
