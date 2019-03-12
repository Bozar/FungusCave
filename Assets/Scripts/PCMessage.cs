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
            message.StoreText("You are exhausted.");
        }

        // NPC hits PC.
        public void IsHit(GameObject attacker)
        {
            message.StoreText(GetAttackerName(attacker) + " hits you.");
        }

        public void IsInfected()
        {
            message.StoreText("You are infected.");
        }

        // NPC kills PC.
        public void IsKilled(GameObject attacker)
        {
            message.StoreText(GetAttackerName(attacker) + " kills you.");
            modeline.PrintStaticText("Press Space to reload.");
        }

        public void IsStressed()
        {
            message.StoreText("You feel stressed.");
        }

        private string GetAttackerName(GameObject attacker)
        {
            return coord.RelativeCoord(attacker, StringStyle.NameNoBracket);
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
        }
    }
}
