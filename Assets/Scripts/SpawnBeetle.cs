using Fungus.GameSystem.Data;
using Fungus.GameSystem.Render;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class SpawnBeetle : MonoBehaviour
    {
        private readonly string dataNode = "SpawnBeetle";
        private readonly string textNode = "SpawnBeetle";
        private int count;
        private int max;
        private bool notWarned;

        public void BeetleEmerge()
        {
            if ((count == 1) && notWarned)
            {
                StoreMessage("Warning");

                notWarned = false;
            }
            else if (count < 1)
            {
                StoreMessage("Emerge");

                // TODO: Spawn beetles.

                count = max;
                notWarned = true;
            }
        }

        private void SpawnCountDown(object sender, ActorInfoEventArgs e)
        {
            int potion = GetComponent<ActorData>().GetIntData(e.ActorTag,
                DataTag.Potion);
            if (potion == 0)
            {
                return;
            }
            count--;

            // TODO: check surrounding.
        }

        private void Start()
        {
            NourishFungus.CountDeath += SpawnCountDown;

            max = GetComponent<GameData>().GetIntData(dataNode, "MaxCount");
            count = max;
            notWarned = true;
        }

        private void StoreMessage(string node)
        {
            string text = GetComponent<GameText>().GetStringData(textNode,
                node);
            text = GetComponent<GameColor>().GetColorfulText(text,
                ColorName.Orange);
            GetComponent<CombatMessage>().StoreText(text);
        }
    }
}
