using Fungus.Actor.Turn;
using Fungus.GameSystem;
using Fungus.GameSystem.ObjectManager;
using System;
using UnityEngine;

namespace Fungus.Actor.AI
{
    public class NPCMemory : MonoBehaviour, ITurnCounter
    {
        private int increaseMemory;
        private int maxMemory;

        public int ForgetCounter { get; private set; }
        public int[] PCPosition { get; private set; }

        public void Count()
        {
            ForgetCounter--;
            ForgetCounter = Math.Max(0, ForgetCounter);
        }

        public void Trigger()
        {
            if (GetComponent<AIVision>().CanSeeTarget(SubObjectTag.PC))
            {
                PCPosition = FindObjects.GameLogic
                    .GetComponent<ConvertCoordinates>().Convert(
                    FindObjects.PC.transform.position);

                ForgetCounter += increaseMemory;
                ForgetCounter = Math.Min(maxMemory, ForgetCounter);
            }
        }

        private void Awake()
        {
            increaseMemory = 3;
            maxMemory = 6;

            PCPosition = new int[2];
            ForgetCounter = 0;
        }
    }
}
