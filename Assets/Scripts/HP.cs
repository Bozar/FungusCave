using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using Fungus.Render;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class HP : MonoBehaviour
    {
        private ConvertCoordinates coordinate;
        private UIMessage message;

        public int CurrentHP { get; private set; }
        public int MaxHP { get; private set; }

        public void GainHP(int hp)
        {
            CurrentHP = Math.Min(MaxHP, CurrentHP + hp);
        }

        public bool LoseHP(int hp)
        {
            CurrentHP = Math.Max(0, CurrentHP - hp);

            // TODO: Check if actor is dead.

            int[] position;

            position = coordinate.Convert(gameObject.transform.position);
            message.StoreText(position[0] + "," + position[1] + " is hit.");

            if (IsDead())
            {
                gameObject.GetComponent<Die>().Bury();
                return true;
            }
            return false;
        }

        private bool IsDead()
        {
            return CurrentHP < 1;
        }

        private void Start()
        {
            MaxHP = FindObjects.GameLogic.GetComponent<ObjectData>().GetIntData(
                gameObject.GetComponent<ObjectMetaInfo>().SubTag, DataTag.HP);
            CurrentHP = MaxHP;

            coordinate = FindObjects.GameLogic.GetComponent<ConvertCoordinates>();
            message = FindObjects.GameLogic.GetComponent<UIMessage>();
        }
    }
}
