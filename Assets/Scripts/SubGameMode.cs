using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class SubGameMode : MonoBehaviour
    {
        public void SwitchExamineMode(bool examine)
        {
            FindObjects.GetUIObject(UITag.ExamineMessage).SetActive(examine);
            FindObjects.GetUIObject(UITag.Message).SetActive(!examine);

            FindObjects.GameLogic.GetComponent<SchedulingSystem>()
                .PauseTurn(examine);
            FindObjects.Examiner.SetActive(examine);
        }
    }
}
