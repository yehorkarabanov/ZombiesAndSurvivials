using _Scripts.Managers;
using _Scripts.Tile;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Item.Unit {
    public abstract class IUnit : IItem {
        [FormerlySerializedAs("Faction")] public UnitFaction unitFaction;
        [SerializeField] private float speed;
        [SerializeField] private float rangeOfVision;

        public void Move() {
            for (int i = 0; i < speed; i++) {
                CurrentMove();
            }
        }

        protected abstract void CurrentMove();
    }
}