using Fungus.GameSystem;
using Fungus.GameSystem.Data;
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
        private GameColor color;
        private ConvertCoordinates coord;
        private InfectionData data;
        private UIMessage message;
        private UIModeline modeline;
        private string node;
        private GameText text;

        public void IsExhausted()
        {
            message.StoreText(text.GetStringData(node, "PCExhaust"));
        }

        // NPC hits PC.
        public void IsHit(GameObject attacker)
        {
            string hit = text.GetStringData(node, "PCHit");
            hit = hit.Replace("%str%", GetAttackerName(attacker));
            message.StoreText(hit);
        }

        public void IsInfected()
        {
            // You are infected: [ %str% ].
            string infection = text.GetStringData(node, "PCInfect");
            GetComponent<Infection>().HasInfection(out InfectionTag tag, out _);
            string name = data.GetInfectionName(tag);

            infection = infection.Replace("%str%", name);
            infection = color.GetColorfulText(infection, ColorName.Orange);

            message.StoreText(infection);
        }

        // NPC kills PC.
        public void IsKilled(GameObject attacker)
        {
            string kill = text.GetStringData(node, "PCKill");
            kill = color.GetColorfulText(kill, ColorName.Orange);

            message.StoreText(kill);
            modeline.PrintStaticText(text.GetStringData("EnterExit", "ReloadDie"));
        }

        public void IsStressed()
        {
            message.StoreText(text.GetStringData(node, "PCStress"));
        }

        private void Awake()
        {
            node = "Combat";
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
            text = FindObjects.GameLogic.GetComponent<GameText>();
            color = FindObjects.GameLogic.GetComponent<GameColor>();
            data = FindObjects.GameLogic.GetComponent<InfectionData>();
        }
    }
}
