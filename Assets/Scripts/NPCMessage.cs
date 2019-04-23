using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCMessage : MonoBehaviour, ICombatMessage
    {
        private ConvertCoordinates coord;
        private UIMessage message;
        private string node;
        private GameText text;

        public void IsExhausted()
        {
            string exhaust = text.GetStringData(node, "NPCExhaust");
            exhaust = exhaust.Replace("%str%", GetDefenderName());
            message.StoreText(exhaust);
        }

        // PC hits NPC.
        public void IsHit(GameObject attacker)
        {
            string hit = text.GetStringData(node, "NPCHit");
            hit = hit.Replace("%str%", GetDefenderName());
            message.StoreText(hit);
        }

        public void IsInfected()
        {
            string infect = text.GetStringData(node, "NPCInfect");
            infect = infect.Replace("%str%", GetDefenderName());
            message.StoreText(infect);
        }

        // PC kills NPC.
        public void IsKilled(GameObject attacker)
        {
            string kill = text.GetStringData(node, "NPCKill");
            kill = kill.Replace("%str%", GetDefenderName());
            message.StoreText(kill);
        }

        public void IsStressed()
        {
            string stress = text.GetStringData(node, "NPCStress");
            stress = stress.Replace("%str%", GetDefenderName());
            message.StoreText(stress);
        }

        private void Awake()
        {
            node = "Combat";
        }

        private string GetDefenderName()
        {
            return coord.RelativeCoord(gameObject, StringStyle.NameNoBracket);
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            text = FindObjects.GameLogic.GetComponent<GameText>();
        }
    }
}
