using UnityEngine;

namespace Fungus.GameSystem
{
    // This script is only attached to GameLogic.
    public class Singleton : MonoBehaviour
    {
        private static bool instance;

        private void Awake()
        {
            if (instance)
            {
                Debug.Log(gameObject.name + " already exists.");
                Destroy(gameObject);

                return;
            }

            instance = true;
        }
    }
}
