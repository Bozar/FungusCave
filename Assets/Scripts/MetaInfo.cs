using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using Fungus.GameSystem.SaveLoadData;
using Fungus.GameSystem.WorldBuilding;
using UnityEngine;

namespace Fungus.Actor
{
    public class MetaInfo : MonoBehaviour, ISaveLoadActorData
    {
        private ConvertCoordinates coord;

        public bool IsPC
        {
            get
            {
                return SubTag == SubObjectTag.PC;
            }
        }

        public bool LoadedActorData { get; private set; }

        public MainObjectTag MainTag { get; private set; }

        public SubObjectTag SubTag { get; private set; }

        public void Load(DTActor data)
        {
            return;
        }

        public void Save(DTActor data)
        {
            data.ActorTag = SubTag;
            data.Position = coord.Convert(transform.position);
        }

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
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
        }
    }
}
