using System;
using System.Linq;
using _Scripts.Item.Equip;
using _Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Item.Unit {
    public class Survivial : IUnit {
        public bool _hasWeapon;
        public bool _hasArmor;

        protected override void CurrentMove() {
            var possibleTargets = _rangeFinder.GetTilesInRange(this.OccupiedTile, rangeOfVision).Where(x => {
                    if (x.OccupiedUnit == null) {
                        return false;
                    }

                    if (x.OccupiedUnit.GetType() == typeof(Armor) || x.OccupiedUnit.GetType() == typeof(Weapon)) {
                        return true;
                    }

                    return false;
                }
            ).ToList();
            if (possibleTargets.Any()) {
                targetTile = possibleTargets.OrderBy(x => Random.value).First();
                target = "equip";
                Path = _pathFinder.FindPath(this.OccupiedTile, targetTile);
            } else {
                if (!targetTile || targetTile == OccupiedTile) {
                    targetTile = _rangeFinder.GetTilesInRange(this.OccupiedTile, rangeOfVision).Where(x => x.Walkable)
                        .OrderBy(x => Random.value).First();
                    target = "random";
                }

                if (!Path.Any()) {
                    Path = _pathFinder.FindPath(this.OccupiedTile, targetTile, false);
                }
            }

            if (!Path.Any()) {
                Path.Add(OccupiedTile);
            }

            GridManager.Instance._tiles[new Vector2(Path.First().x, Path.First().y)].SetUnit(this);
            Path.Remove(Path.First());

            var equip = CheckPickUp();
            if (equip != null) {
                PickUp(equip);
            }
        }

        private IEquip CheckPickUp() {
            var neighbours = _rangeFinder.GetTilesInRange(this.OccupiedTile, 1);
            foreach (var item in neighbours) {
                if (item.OccupiedUnit != null && (item.OccupiedUnit.GetType() == typeof(Armor) ||
                                                  item.OccupiedUnit.GetType() == typeof(Weapon))) {
                    return (IEquip)item.OccupiedUnit;
                }
            }

            return null;
        }

        private void PickUp(IEquip equip) {
            if (equip.GetType() == typeof(Weapon)) {
                this._hasWeapon = true;
            } else {
                this._hasArmor = true;
            }

            equip.OccupiedTile.OccupiedUnit = null;
            equip.transform.position = new Vector3(0, 0, 1);
            Destroy(equip);
            ItemManager.Instance.listEquip.Remove(equip);
        }
    }
}