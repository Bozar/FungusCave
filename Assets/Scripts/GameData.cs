using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem
{
    public class GameData : MonoBehaviour
    {
        private XElement dataFile;
        private string fileName;

        public int GetIntData(string firstNode, string secondNode)
        {
            return (int)GetData(firstNode, secondNode);
        }

        public string GetStringData(string firstNode, string secondNode)
        {
            return (string)GetData(firstNode, secondNode);
        }

        private XElement GetData(string firstNode, string secondNode)
        {
            return dataFile.Element(firstNode).Element(secondNode);
        }

        private void Start()
        {
            fileName = "gameData.xml";
            dataFile = GetComponent<SaveLoad>().LoadXML(fileName);
        }
    }
}
