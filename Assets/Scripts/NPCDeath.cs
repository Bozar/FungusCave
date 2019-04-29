using Fungus.Actor.Render;
using Fungus.GameSystem;
using Fungus.GameSystem.Progress;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.Actor
{
    public class NPCDeath : MonoBehaviour, IDeath
    {
        private ConvertCoordinates coord;
        private NourishFungus fungus;
        private ObjectPool pool;

        public void BurySelf()
        {
            fungus.CountDeath(this, new ActorInfoEventArgs(
                GetComponent<MetaInfo>().SubTag,
                coord.Convert(transform.position)));
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
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            fungus = FindObjects.GameLogic.GetComponent<NourishFungus>();
        }
    }
}
