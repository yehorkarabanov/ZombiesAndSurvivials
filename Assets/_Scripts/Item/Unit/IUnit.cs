using _Scripts.Tile;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Item.Unit {
    public abstract class IUnit : IItem {
        [FormerlySerializedAs("Faction")] public UnitFaction unitFaction;
    }
}