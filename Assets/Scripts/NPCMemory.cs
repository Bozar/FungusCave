using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.Data;
using System;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class NPCMemory : MonoBehaviour, ITurnCounter
    {
        private int forgetCounter;
        private int increaseMemory;
        private int maxMemory;

        public int[] PCPosition { get; private set; }

        public void Count()
        {
            forgetCounter--;
            forgetCounter = Math.Max(0, forgetCounter);

            if (forgetCounter == 0)
            {
                PCPosition = new int[2];
            }
        }

        public bool RememberPC()
        {
            return forgetCounter > 0;
        }

        public void Trigger()
        {
            if (GetComponent<AIVision>().CanSeeTarget(SubObjectTag.PC))
            {
                PCPosition = FindObjects.GameLogic
                    .GetComponent<ConvertCoordinates>().Convert(
                    FindObjects.PC.transform.position);

                forgetCounter += increaseMemory;
                forgetCounter = Math.Min(maxMemory, forgetCounter);
            }
        }

        private void Awake()
        {
            increaseMemory = 3;
            maxMemory = 6;

            PCPosition = new int[2];
            forgetCounter = 0;
        }
    }
}
