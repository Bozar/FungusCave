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
            string defenderName = coord.RelativeCoordWithName(gameObject);
            message.StoreText(defenderName + " is exhausted.");
        }

        // PC hits NPC.
        public void IsHit(GameObject attacker)
        {
            string defenderName = coord.RelativeCoordWithName(gameObject);
            message.StoreText("You hit " + defenderName + ".");
        }

        public void IsInfected()
        {
            string defenderName = coord.RelativeCoordWithName(gameObject);
            message.StoreText(defenderName + " is infected.");
        }

        // PC kills NPC.
        public void IsKilled(GameObject attacker)
        {
            string defenderName = coord.RelativeCoordWithName(gameObject);
            message.StoreText("You kill " + defenderName + ".");
        }

        public void IsStressed()
        {
            string defenderName = coord.RelativeCoordWithName(gameObject);
            message.StoreText(defenderName + " looks stressed.");
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
        }
    }
}
