using _Scripts.Item.Equip;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Item.Unit {
    public class Survivial : IUnit {
        private Weapon _weapon;
        private Armor _armor;

        protected override void CurrentMove() {
            var x = OccupiedTile.x;
            var y = OccupiedTile.y;
            if (GridManager.Instance._tiles[new Vector2(x, y + 1)].Walkable) {
                GridManager.Instance._tiles[new Vector2(x, y + 1)].SetUnit(this);
            }
        }
    }
}