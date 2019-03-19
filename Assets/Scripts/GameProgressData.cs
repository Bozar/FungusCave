using UnityEngine;

namespace Fungus.GameSystem
{
    public class GameProgressData : MonoBehaviour
    {
        public int MaxActor { get; private set; }
        public int MaxSoldier { get; private set; }

        private void Awake()
        {
            MaxActor = 40;
            MaxSoldier = 20;
        }
    }
}
