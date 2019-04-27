using Fungus.Actor.Render;
using Fungus.GameSystem;
using Fungus.GameSystem.Progress;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCDeath : MonoBehaviour, IDeath
    {
        private ObjectPool pool;

        public void BurySelf()
        {
            NourishFungus.CountDeath(this,
                new TagPositionEventArgs(GetComponent<MetaInfo>().SubTag,
                new int[] { 42 }));
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
