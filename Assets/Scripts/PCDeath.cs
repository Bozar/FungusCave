using Fungus.GameSystem;
using Fungus.GameSystem.Render;
using Fungus.GameSystem.Turn;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IDeath
    {
        void Bury();

        void Revive();
    }

    public class PCDeath : MonoBehaviour, IDeath
    {
        private UIMessage message;
        private UIModeline modeline;
        private SchedulingSystem schedule;

        public void Bury()
        {
            schedule.PauseTurn(true);

            message.StoreText("PC is dead.");
            modeline.PrintStaticText("Press Space to reload.");

            FindObjects.Guide.transform.position = transform.position;

            gameObject.SetActive(false);
            FindObjects.Guide.SetActive(true);
        }

        public void Revive()
        {
            GetComponent<Potion>().DrinkPotion();
        }

        private void Start()
        {
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            modeline = FindObjects.GameLogic.GetComponent<UIModeline>();
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
        }
    }
}
