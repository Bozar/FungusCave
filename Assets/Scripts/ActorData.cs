using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public enum DataTag
    {
        ActorName,

        Stress, Damage, Potion,
        InfectionAttack, InfectionDefend, InfectionRecovery,
        EnergyRestore, EnergyDrain,
        HP, HPRestore,
        SightRange
    }

    public class ActorData : MonoBehaviour
    {
        private string defaultActor;
        private XElement xFile;

        public int GetIntData(SubObjectTag oTag, DataTag dTag)
        {
            return (int)GetData(oTag, dTag);
        }

        public string GetStringData(SubObjectTag oTag, DataTag dTag)
        {
            return (string)GetData(oTag, dTag);
        }

        private XElement GetData(SubObjectTag oTag, DataTag dTag)
        {
            XElement actor = xFile.Element(oTag.ToString());
            XElement defActor = xFile.Element(defaultActor);

            if (actor.Element(dTag.ToString()) == null)
            {
                return defActor.Element(dTag.ToString());
            }
            return actor.Element(dTag.ToString());
        }

        private void Start()
        {
            defaultActor = "DEFAULT";
            xFile = GetComponent<SaveLoad>().LoadXML("actorData.xml");
        }
    }
}
