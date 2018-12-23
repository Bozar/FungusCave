using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor.ObjectManager
{
    public class ObjectMetaInfo : MonoBehaviour
    {
        public int DropPotion { get; private set; }
        public MainObjectTag MainTag { get; private set; }
        public SubObjectTag SubTag { get; private set; }

        public void SetMainTag(MainObjectTag tag)
        {
            MainTag = tag;
        }

        public void SetSubTag(SubObjectTag tag)
        {
            SubTag = tag;
        }

        private void Start()
        {
            DropPotion = FindObjects.GameLogic.GetComponent<ObjectData>()
                .GetIntData(SubTag, DataTag.DropPotion);
        }
    }
}
