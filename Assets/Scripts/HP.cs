using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public interface IHP : ITurnCounter
    {
        void RestoreAfterKill();
    }

    public class HP : MonoBehaviour, IResetData
    {
        private ActorData actorData;

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

        public void LoseHP(int hp)
        {
            CurrentHP = Math.Max(0, CurrentHP - hp);

            if (CurrentHP < 1)
            {
                GetComponent<IDeath>().ReviveSelf();
            }
            if (CurrentHP < 1)
            {
                GetComponent<IDeath>().BurySelf();
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            actorData = FindObjects.GameLogic.GetComponent<ActorData>();

            CurrentHP = MaxHP;
        }
    }
}
