using Fungus.GameSystem.Data;
using UnityEngine;

namespace Fungus.Actor
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
