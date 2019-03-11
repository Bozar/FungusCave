using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor
{
    public interface ICombatMessage
    {
        void IsExhausted();

        void IsHit(GameObject attacker);

        void IsInfected();

        void IsKilled(GameObject attacker);

        void IsStressed();
    }

    public class PCMessage : MonoBehaviour, ICombatMessage
    {
        private ConvertCoordinates coord;
        private UIMessage message;
        private UIModeline modeline;

        public void IsExhausted()
        {
            throw new System.NotImplementedException();
        }

        // NPC hits PC.
        public void IsHit(GameObject attacker)
        {
            string attackerName = coord.RelativeCoordWithName(attacker);
            message.StoreText(attackerName + " hits you.");
        }

        public void IsInfected()
        {
            throw new System.NotImplementedException();
        }

        // NPC kills PC.
        public void IsKilled(GameObject attacker)
        {
            string attackerName = coord.RelativeCoordWithName(attacker);
            message.StoreText(attackerName + " kills you.");
            modeline.PrintStaticText("Press Space to reload.");
        }

        public void IsStressed()
        {
            throw new System.NotImplementedException();
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
        }
    }
}
