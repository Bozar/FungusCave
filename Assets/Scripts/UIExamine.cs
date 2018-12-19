using UnityEngine;

namespace Fungus.GameSystem.Render
{
    public class UIExamine : MonoBehaviour, IUpdateUI
    {
        public void PrintText()
        {
        }

        public void SwitchExamineMode(bool isExamine)
        {
            FindObjects.GetUIObject(UITag.ExamineMessage).SetActive(isExamine);
            FindObjects.GetUIObject(UITag.Message).SetActive(!isExamine);
        }
    }
}
