using UnityEngine;

namespace _Scripts.Units {
    [CreateAssetMenu(fileName = "New unit", menuName = "Scriptable unit")]
    public class ScriptableUnit : ScriptableObject {
        public IUnit UnitPrefab;
        public Faction Faction;
    }

    public enum Faction {
        Survivials = 0,
        Zombie = 1
    }
}