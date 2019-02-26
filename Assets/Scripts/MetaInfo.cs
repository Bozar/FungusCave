using Fungus.GameSystem.ObjectManager;
using UnityEngine;

namespace Fungus.Actor.ObjectManager
{
    public class MetaInfo : MonoBehaviour
    {
        public bool IsPC
        {
            get
            {
                return SubTag == SubObjectTag.PC;
            }
        }

        public MainObjectTag MainTag { get; private set; }

        public string Name
        {
            get
            {
                return SubTag.ToString();
            }
        }

        public SubObjectTag SubTag { get; private set; }

        public void SetMainTag(MainObjectTag tag)
        {
            MainTag = tag;
        }

        public void SetSubTag(SubObjectTag tag)
        {
            SubTag = tag;
        }
    }
}
