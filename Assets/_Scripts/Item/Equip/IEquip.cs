using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Item.Equip {
    public abstract class IEquip : IItem {
        [FormerlySerializedAs("Faction")] public EquipFaction equipFaction;
    }
}