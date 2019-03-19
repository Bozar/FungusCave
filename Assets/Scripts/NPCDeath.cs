using Fungus.Actor.Render;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCDeath : MonoBehaviour, IDeath
    {
        private ObjectPool pool;

        public void BurySelf()
        {
            pool.StoreObject(gameObject);
        }

        public void DefeatTarget(GameObject target)
        {
            GetComponent<RenderSprite>().ChangeDefaultColor(ColorName.Orange);
        }

        public void ReviveSelf()
        {
            return;
        }

        private void Start()
        {
            pool = FindObjects.GameLogic.GetComponent<ObjectPool>();
        }
    }
}
