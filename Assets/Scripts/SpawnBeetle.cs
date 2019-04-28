using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class SpawnBeetle : MonoBehaviour
    {
        private void SpawnCountDown(object sender, TagPositionEventArgs e)
        {
            Debug.Log(e.ActorTag);
            Debug.Log(e.Position[0]);
            Debug.Log(e.Position[1]);
        }

        private void Start()
        {
            NourishFungus.CountDeath += SpawnCountDown;
        }
    }
}
