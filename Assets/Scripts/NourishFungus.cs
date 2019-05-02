using Fungus.GameSystem.Data;
using System;
using UnityEngine;

namespace Fungus.GameSystem.Progress
{
    public class ActorInfoEventArgs : EventArgs
    {
        public readonly SubObjectTag ActorTag;
        public readonly int[] Position;

        public ActorInfoEventArgs(SubObjectTag actorTag, int[] position)
        {
            ActorTag = actorTag;
            Position = position;
        }
    }

    public class NourishFungus : MonoBehaviour
    {
        public event EventHandler<ActorInfoEventArgs> SpawnCountDown;

        public void CountDown(ActorInfoEventArgs e)
        {
            int potion = GetComponent<ActorData>().GetIntData(e.ActorTag,
               DataTag.Potion);
            if (potion == 0)
            {
                return;
            }
            OnSpawnCountDown(e);
        }

        protected virtual void OnSpawnCountDown(ActorInfoEventArgs e)
        {
            SpawnCountDown?.Invoke(this, e);
        }
    }
}
