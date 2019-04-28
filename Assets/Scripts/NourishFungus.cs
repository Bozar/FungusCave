using Fungus.GameSystem.Data;
using System;

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

    public class NourishFungus
    {
        public static EventHandler<ActorInfoEventArgs> CountDeath;

        protected virtual void OnCountDeath(ActorInfoEventArgs e)
        {
            CountDeath?.Invoke(this, e);
        }
    }
}
