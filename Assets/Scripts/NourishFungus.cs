using Fungus.GameSystem.Data;
using System;

namespace Fungus.GameSystem.Progress
{
    public class NourishFungus
    {
        public static EventHandler<TagPositionEventArgs> CountDeath;

        protected virtual void OnCountDeath(TagPositionEventArgs e)
        {
            CountDeath?.Invoke(this, e);
        }
    }

    public class TagPositionEventArgs : EventArgs
    {
        public readonly SubObjectTag ActorTag;
        public readonly int[] Position;

        public TagPositionEventArgs(SubObjectTag actorTag, int[] position)
        {
            ActorTag = actorTag;
            Position = position;
        }
    }
}
