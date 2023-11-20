using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Item.Equip;
using _Scripts.Managers;
using _Scripts.Tile;
using UnityEngine;
using Random = UnityEngine.Random;

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
            var PriorityTargets = _rangeFinder.GetTilesInRange(this.OccupiedTile, 3).Where(x => {
                    if (x.OccupiedUnit == null) {
                        return false;
                    }

                    if ((x.OccupiedUnit.GetType() == typeof(Armor) && !this._hasArmor) ||
                        (x.OccupiedUnit.GetType() == typeof(Weapon) && !this._hasWeapon)) {
                        return true;
                    }

                    return false;
                }
            ).ToList();

            List<ITile> zombies = new List<ITile>();
            List<Survivial> survivials = new List<Survivial>();
            List<ITile> possibleTargets = new List<ITile>();
            foreach (var item in Targets) {
                if ((item.OccupiedUnit.GetType() == typeof(Armor) && !this._hasArmor) ||
                    (item.OccupiedUnit.GetType() == typeof(Weapon) && !this._hasWeapon)) {
                    possibleTargets.Add(item);
                    continue;
                }

                if (item.OccupiedUnit.GetType() == typeof(Zombie)) {
                    zombies.Add(item);
                }

                if (item.OccupiedUnit.GetType() == typeof(Survivial)) {
                    survivials.Add((Survivial)item.OccupiedUnit);
                }
            }

            if (survivials.Count > zombies.Count + 10 && zombies.Count != 0) {
                survivials.ForEach(x => x.Attack = true);
            } else {
                this.Attack = false;
            }

            if (PriorityTargets.Any()) {
                targetTile = PriorityTargets.OrderBy(x => Random.value).First();
                target = "equip";
                Path = _pathFinder.FindPath(this.OccupiedTile, targetTile);
            } else if (this.Attack) {
                if (zombies.Count != 0) {
                    targetTile = zombies.OrderBy(x => Random.value).First();
                    target = "zombie";
                    Path = _pathFinder.FindPath(this.OccupiedTile, targetTile);
                } else {
                    try {
                        targetTile = survivials.Where(s => s.target == "zombie").OrderBy(x => Random.value).First()
                            .OccupiedTile;
                        target = "zombie_s";
                        Path = _pathFinder.FindPath(this.OccupiedTile, targetTile);
                    }
                    catch (InvalidOperationException) {
                        targetTile = possibleTargets.OrderBy(x => Random.value).First();
                        target = "equip";
                        Path = _pathFinder.FindPath(this.OccupiedTile, targetTile);
                    }
                }
            } else if (possibleTargets.Any()) {
                targetTile = possibleTargets.OrderBy(x => Random.value).First();
                target = "equip";
                Path = _pathFinder.FindPath(this.OccupiedTile, targetTile);
            } else if (survivials.Count(s => s.target is "zombie" or "zombie_s")>0) {
                targetTile = survivials.Where(s => s.target is "zombie" or "zombie_s").OrderBy(x => Random.value).First()
                    .OccupiedTile;
                target = "zombie_s";
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

                Destroy(equip.gameObject);
                equip.OccupiedTile.OccupiedUnit = null;
                ItemManager.Instance.listEquip.Remove(equip);
            } else if (!this._hasArmor) {
                this._hasArmor = true;

                Destroy(equip.gameObject);
                equip.OccupiedTile.OccupiedUnit = null;
                ItemManager.Instance.listEquip.Remove(equip);
            }
        }
    }
}