using Fungus.Actor.ObjectManager;
using Fungus.GameSystem;
using System;
using UnityEngine;

namespace Fungus.Actor
{
    public class Stress : MonoBehaviour
    {
        public int CurrentStress { get; private set; }
        public int MaxStress { get; private set; }

        public void GainStress(int stress)
        {
            CurrentStress = Math.Min(MaxStress, CurrentStress + stress);
        }

        public bool HasMaxStress()
        {
            return CurrentStress == MaxStress;
        }

        public void LoseStress(int stress)
        {
            CurrentStress = Math.Max(0, CurrentStress - stress);
        }

        private void Start()
        {
            MaxStress = FindObjects.GameLogic.GetComponent<ObjectData>()
                .GetIntData(gameObject.GetComponent<ObjectMetaInfo>().SubTag,
                DataTag.Stress);
            CurrentStress = 0;
        }
    }
}
