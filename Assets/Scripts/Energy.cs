using System.Text;
using UnityEngine;

public class Energy : MonoBehaviour
{
    private int actionThreshold;
    private StringBuilder printText;
    private int[] testPosition;
    public int CurrentEnergy { get; set; }

    public bool HasEnoughEnergy()
    {
        return CurrentEnergy >= actionThreshold;
    }

    public void PrintEnergy()
    {
        testPosition = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
            .Convert(gameObject.transform.position);

        printText.Remove(0, printText.Length);
        printText.Append("[");
        printText.Append(testPosition[0].ToString());
        printText.Append(",");
        printText.Append(testPosition[1].ToString());
        printText.Append("] ");
        printText.Append("Energy: ");
        printText.Append(CurrentEnergy.ToString());

        FindObjects.GameLogic.GetComponent<UIMessage>().StoreText(
            printText.ToString());
    }

    public void RestoreEnergy(int restore, bool checkCurrentEnergy)
    {
        if (checkCurrentEnergy && (CurrentEnergy >= actionThreshold))
        {
            return;
        }

        CurrentEnergy += restore;
    }

    private void Awake()
    {
        actionThreshold = 2000;
        CurrentEnergy = actionThreshold;

        printText = new StringBuilder();
    }
}
