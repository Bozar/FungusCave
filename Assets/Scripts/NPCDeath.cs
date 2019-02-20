using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCDeath : MonoBehaviour, IDeath
    {
        private UIMessage message;
        private ObjectPool pool;

        public void Bury()
        {
            message.StoreText("NPC is dead.");
            pool.StoreObject(gameObject);
        }

        public void Revive()
        {
            return;
        }

        private void Start()
        {
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            pool = FindObjects.GameLogic.GetComponent<ObjectPool>();
        }
    }
}
