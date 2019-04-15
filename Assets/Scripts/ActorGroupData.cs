using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Fungus.GameSystem.ObjectManager
{
    public enum CombatRoleTag
    {
        Minion, Soldier
    }

    public class ActorGroupData : MonoBehaviour
    {
        public Dictionary<CombatRoleTag, SubObjectTag[]> GetActorGroup(
            DungeonLevel dl)
        {
            XElement xele = GetComponent<GameData>().GetData("ActorGroup", dl);
            Stack<SubObjectTag> minion = new Stack<SubObjectTag>();
            Stack<SubObjectTag> soldier = new Stack<SubObjectTag>();

            foreach (XElement e in xele.Elements())
            {
                if (e.Name == CombatRoleTag.Minion.ToString())
                {
                    minion.Push((SubObjectTag)Enum.Parse(
                        typeof(SubObjectTag), e.Value.ToString()));
                }
                else
                {
                    soldier.Push((SubObjectTag)Enum.Parse(
                        typeof(SubObjectTag), e.Value.ToString()));
                }
            }

            var result = new Dictionary<CombatRoleTag, SubObjectTag[]>
            {
                { CombatRoleTag.Minion, minion.ToArray() },
                { CombatRoleTag.Soldier, soldier.ToArray() },
            };
            return result;
        }
    }
}
