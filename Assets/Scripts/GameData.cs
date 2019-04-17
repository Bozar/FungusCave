using Fungus.GameSystem.SaveLoadData;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem.Data
{
    public interface IGetData
    {
        XElement GetData<T, U>(T t, U u);

        int GetIntData<T, U>(T t, U u);

        string GetStringData<T, U>(T t, U u);
    }

    public class GameData : MonoBehaviour, IGetData
    {
        private XElement dataFile;
        private string fileName;

        public XElement GetData<T, U>(T parentNode, U childNode)
        {
            return dataFile
                .Element(parentNode.ToString())
                .Element(childNode.ToString());
        }

        public int GetIntData<T, U>(T parentNode, U childNode)
        {
            return (int)GetData(parentNode, childNode);
        }

        public string GetStringData<T, U>(T parentNode, U childNode)
        {
            return (string)GetData(parentNode, childNode);
        }

        private void Start()
        {
            fileName = "gameData.xml";
            dataFile = GetComponent<SaveLoadFile>().LoadXML(fileName);
        }
    }
}
