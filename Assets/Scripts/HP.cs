using Fungus.Actor.ObjectManager;
using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using Fungus.GameSystem.Render;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IHP : ITurnCounter
    {
        void RestoreAfterKill();
    }

    public class HP : MonoBehaviour
    {
        private ActorData actorData;
        private ConvertCoordinates coord;
        private UIMessage message;

        public int CurrentHP { get; private set; }

        public int MaxHP
        {
            get
            {
                return actorData.GetIntData(
                    GetComponent<MetaInfo>().SubTag, DataTag.HP);
            }
        }

        public void GainHP(int hp)
        {
            CurrentHP = Math.Min(MaxHP, CurrentHP + hp);
        }

        public bool LoseHP(int hp)
        {
            CurrentHP = Math.Max(0, CurrentHP - hp);

            // TODO: Either remove these lines or change them later.
            int[] position = coord.Convert(transform.position);
            message.StoreText(coord.RelativeCoordWithName(gameObject) + " is hit.");
            //message.StoreText(position[0] + "," + position[1] + " is hit.");

            if (CurrentHP < 1)
            {
                GetComponent<IDeath>().Revive();
            }

            if (CurrentHP < 1)
            {
                GetComponent<IDeath>().Bury();
                return true;
            }
            return false;
        }

        private void Start()
        {
            coord = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();

            CurrentHP = MaxHP;
        }
    }
}
