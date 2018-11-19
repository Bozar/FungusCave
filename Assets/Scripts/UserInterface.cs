using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private DungeonBoard board;
    private UIDict getUI;
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

        UpdateHP();
        UpdateStress();
        UpdatePotion();
        UpdateDamage();
        UpdateEnvironment();
        UpdateSeed();

        FindObjects.GameLogic.GetComponent<UIMessage>().PrintText();
    }

    private void Start()
    {
        board = FindObjects.GameLogic.GetComponent<DungeonBoard>();
        getUI = FindObjects.GetUIObject;
    }

    private void UpdateDamage()
    {
        int current = pc.GetComponent<Attack>().GetCurrentDamage();

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
            sb.Append(" | ");
        }

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

    private void UpdateHP()
    {
        int current = pc.GetComponent<HP>().CurrentHP;
        int max = pc.GetComponent<HP>().MaxHP;

        getUI(UITag.HPData).GetComponent<Text>().text = current + "/" + max;
    }

    private void UpdatePotion()
    {
        int current = pc.GetComponent<Potion>().CurrentPotion;

        getUI(UITag.PotionData).GetComponent<Text>().text = current.ToString();
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

    private void UpdateStress()
    {
        int current = pc.GetComponent<Stress>().CurrentStress;
        int max = pc.GetComponent<Stress>().MaxStress;

        getUI(UITag.StressData).GetComponent<Text>().text = current + "/" + max;
    }
}
