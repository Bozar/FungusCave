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
            throw new System.NotImplementedException();
        }

        // PC hits NPC.
        public void IsHit(GameObject attacker)
        {
            string defenderName = coord.RelativeCoordWithName(gameObject);
            message.StoreText("You hit " + defenderName + ".");
        }

        public void IsInfected()
        {
            throw new System.NotImplementedException();
        }

        // PC kills NPC.
        public void IsKilled(GameObject attacker)
        {
            string defenderName = coord.RelativeCoordWithName(gameObject);
            message.StoreText("You kill " + defenderName + ".");
        }

        public void IsStressed()
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
        }
    }
}
