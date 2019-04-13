using UnityEngine;

namespace Fungus.GameSystem
{
    public class ProgressData : MonoBehaviour
    {
        public int MaxActor
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Dungeon",
                    "MaxActor");
            }
        }

        public int MaxSoldier
        {
            get
            {
                return GetComponent<GameData>().GetIntData("Dungeon",
                    "MaxSoldier");
            }
        }
    }
}
