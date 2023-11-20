using System.Collections.Generic;
using System.Linq;
using _Scripts.Item.Equip;
using _Scripts.Managers;
using _Scripts.Tile;
using UnityEngine;

namespace _Scripts.Item.Unit {
    public class Survivial : IUnit {
        public bool _hasWeapon = false;
        public bool _hasArmor = false;
        public bool Attack = false;

        protected override void CurrentMove() {
            var Targets = _rangeFinder.GetTilesInRange(this.OccupiedTile, rangeOfVision).Where(x => {
                    if (x.OccupiedUnit != null) {
                        return true;
                    }

                    return false;
                }
            ).ToList();

            List<Zombie> zombies = new List<Zombie>();
            List<Survivial> survivials = new List<Survivial>();
            List<ITile> possibleTargets = new List<ITile>();
            foreach (var item in Targets) {
                if ((item.OccupiedUnit.GetType() == typeof(Armor) && !this._hasArmor) ||
                    (item.OccupiedUnit.GetType() == typeof(Weapon) && !this._hasWeapon)) {
                    possibleTargets.Add(item);
                    continue;
                }

                if (item.OccupiedUnit.GetType() == typeof(Zombie)) {
                    zombies.Add((Zombie)item.OccupiedUnit);
                }

                if (item.OccupiedUnit.GetType() == typeof(Survivial)) {
                    survivials.Add((Survivial)item.OccupiedUnit);
                }
            }

            if (survivials.Count > zombies.Count + 5) {
            }


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
            var neighbours = _rangeFinder.GetTilesInRange(this.OccupiedTile, 1).Where(item => {
                if (item.OccupiedUnit == null) {
                    return false;
                }

                if ((item.OccupiedUnit.GetType() == typeof(Armor) && !this._hasArmor) ||
                    (item.OccupiedUnit.GetType() == typeof(Weapon) && !this._hasWeapon)) {
                    return true;
                }

                return false;
            }).ToList();
            foreach (var item in neighbours) {
                return (IEquip)item.OccupiedUnit;
            }

            return null;
        }

        private void PickUp(IEquip equip) {
            if (equip.GetType() == typeof(Weapon) && !this._hasWeapon) {
                this._hasWeapon = true;

                equip.OccupiedTile.OccupiedUnit = null;
                equip.transform.position = new Vector3(0, 0, 1);
                Destroy(equip);
                ItemManager.Instance.listEquip.Remove(equip);
            } else if (!this._hasArmor) {
                this._hasArmor = true;

                equip.OccupiedTile.OccupiedUnit = null;
                equip.transform.position = new Vector3(0, 0, 1);
                Destroy(equip);
                ItemManager.Instance.listEquip.Remove(equip);
            }
        }
    }
}