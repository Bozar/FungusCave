using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCMessage : MonoBehaviour, ICombatMessage
    {
        private ConvertCoordinates coord;
        private UIMessage message;

        public void IsExhausted()
        {
            message.StoreText(GetDefenderName() + " is exhausted.");
        }

        // PC hits NPC.
        public void IsHit(GameObject attacker)
        {
            message.StoreText("You hit " + GetDefenderName() + ".");
        }

        public void IsInfected()
        {
            message.StoreText(GetDefenderName() + " is infected.");
        }

        // PC kills NPC.
        public void IsKilled(GameObject attacker)
        {
            message.StoreText("You kill " + GetDefenderName() + ".");
        }

        public void IsStressed()
        {
            message.StoreText(GetDefenderName() + " looks stressed.");
        }

        private string GetDefenderName()
        {
            return coord.RelativeCoord(gameObject, StringStyle.NameNoBracket);
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
        }
    }
}
