using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Item.Equip {
    [CreateAssetMenu(fileName = "New equip", menuName = "Scriptable equip")]
    public class ScriptableEquip : ScriptableObject {
        public IEquip equipPrefab;
        [FormerlySerializedAs("Faction")] public EquipFaction equipFaction;
    }


    public enum EquipFaction {
        Weapon = 0,
        Armor = 1
    }
}