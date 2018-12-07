using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using System;
using System.Text;
using UnityEngine;

namespace Fungus.Actor
{
    public class Energy : MonoBehaviour
    {
        private int actionThreshold;

        public int CurrentEnergy { get; private set; }

        public void GainEnergy(int energy, bool checkCurrentEnergy)
        {
            if (checkCurrentEnergy && HasEnoughEnergy())
            {
                return;
            }

            CurrentEnergy += energy;
            CurrentEnergy = Math.Min(CurrentEnergy, actionThreshold * 3);
        }

        public bool HasEnoughEnergy()
        {
            return CurrentEnergy >= actionThreshold;
        }

        public void LoseEnergy(int energy)
        {
            CurrentEnergy -= energy;
        }

        public void PrintEnergy()
        {
            StringBuilder printText = new StringBuilder();
            int[] testPosition
                = FindObjects.GameLogic.GetComponent<ConvertCoordinates>()
                .Convert(transform.position);

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

        private void Awake()
        {
            actionThreshold = 2000;
            CurrentEnergy = actionThreshold;
        }
    }
}
