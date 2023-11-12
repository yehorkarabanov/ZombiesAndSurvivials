using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Item.Unit {
    [CreateAssetMenu(fileName = "New unit", menuName = "Scriptable unit")]
    public class ScriptableUnit : ScriptableObject {
        public IUnit UnitPrefab;
        [FormerlySerializedAs("Faction")] public UnitFaction unitFaction;
    }

    public enum UnitFaction {
        Survivials = 0,
        Zombie = 1
    }
}