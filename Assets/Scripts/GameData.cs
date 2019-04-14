using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
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

        public XElement GetData<T, U>(T firstNode, U secondNode)
        {
            return dataFile
                .Element(firstNode.ToString())
                .Element(secondNode.ToString());
        }

        public int GetIntData<T, U>(T firstNode, U secondNode)
        {
            return (int)GetData(firstNode, secondNode);
        }

        public string GetStringData<T, U>(T firstNode, U secondNode)
        {
            return (string)GetData(firstNode, secondNode);
        }

        private void Start()
        {
            fileName = "gameData.xml";
            dataFile = GetComponent<SaveLoad>().LoadXML(fileName);
        }
    }
}
