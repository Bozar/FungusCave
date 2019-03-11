using Fungus.GameSystem;
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
        private SchedulingSystem schedule;

        public void Bury()
        {
            schedule.PauseTurn(true);
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
            schedule = FindObjects.GameLogic.GetComponent<SchedulingSystem>();
        }
    }
}
