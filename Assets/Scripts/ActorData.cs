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

    public class ActorData : MonoBehaviour, IGetData
    {
        private string defaultActor;
        private XElement xFile;

        public XElement GetData<T, U>(T subObjTag, U dataTag)
        {
            XElement actor = xFile.Element(subObjTag.ToString());
            XElement defActor = xFile.Element(defaultActor);

            if (actor.Element(dataTag.ToString()) == null)
            {
                return defActor.Element(dataTag.ToString());
            }
            return actor.Element(dataTag.ToString());
        }

        public int GetIntData<T, U>(T subObjTag, U dataTag)
        {
            return (int)GetData(subObjTag, dataTag);
        }

        public string GetStringData<T, U>(T subObjTag, U dataTag)
        {
            XElement strData = GetData(subObjTag, dataTag);
            string lang = GetComponent<GameSetting>().GetValidLanguage(strData);

            return (string)strData.Element(lang);
        }

        private void Start()
        {
            defaultActor = "DEFAULT";
            xFile = GetComponent<SaveLoad>().LoadXML("actorData.xml");
        }
    }
}
