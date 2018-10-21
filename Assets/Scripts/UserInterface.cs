using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    private string printText;

    private void Update()
    {
        UpdateSeed();
    }

    private void UpdateSeed()
    {
        printText = FindObjects.GameLogic.GetComponent<RandomNumber>()
            .Seed.ToString();
        int textLength = printText.Length;

        for (int i = 1; textLength > i * 3; i++)
        {
            printText = printText.Insert(i * 4 - 1, "-");
        }

        FindObjects.MainUIDict[(int)FindObjects.UITags.Seed].
            GetComponent<Text>().text = printText;
    }
}
