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
        public event EventHandler<ActorInfoEventArgs> CountDeath;

        public void NourishFungus_CountDeath(ActorInfoEventArgs e)
        {
            OnCountDeath(e);
        }

        protected virtual void OnCountDeath(ActorInfoEventArgs e)
        {
            CountDeath?.Invoke(this, e);
        }
    }
}
