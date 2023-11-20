using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Tile;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Item.Unit {
    public abstract class IUnit : IItem {
        [FormerlySerializedAs("Faction")] public UnitFaction unitFaction;
        [SerializeField] protected int speed;
        [SerializeField] protected int rangeOfVision;
        protected PathFinder _pathFinder = new PathFinder();
        protected RangeFinder _rangeFinder = new RangeFinder();
        protected List<ITile> Path = new List<ITile>();
        protected string target;
        protected ITile targetTile;

        public void Move() {
            findMove();
            for (int i = 0; i < speed; i++) {
                CurrentMove();
            }

            var opunit = checkBattle(this);
            if (opunit != null) {
                battle(this, opunit);
            }
        }

        protected abstract void findMove();

        protected abstract void CurrentMove();

        protected void battle(IUnit survivial_, IUnit zombie_) {
            Survivial survivial;
            Zombie zombie;
            if (survivial_.GetType() != typeof(Survivial)) {
                var temp = survivial_;
                survivial = (Survivial)zombie_;
                zombie = (Zombie)temp;
            } else {
                survivial = (Survivial)survivial_;
                zombie = (Zombie)zombie_;
            }

            if (Random.Range(0f, 100f) < 40f + (survivial._hasWeapon ? 30f : 0f)) {
                //nothing
                return;
            }

            if (Random.Range(0f, 100f) < 65f + (survivial._hasWeapon ? 30f : 0f)) {
                //battle start but sur win
                Destroy(zombie.gameObject);
                zombie.OccupiedTile.OccupiedUnit = null;
                ItemManager.Instance.ListUnits.Remove(zombie);
                return;
            }

            //battle start zombie win
            var survTile = survivial.OccupiedTile;
            Destroy(survivial.gameObject);
            survivial.OccupiedTile.OccupiedUnit = null;
            ItemManager.Instance.ListUnits.Remove(survivial);
            if (Random.Range(0f, 100f) > 80f + (survivial._hasArmor ? 15f : 0f)) {
                //new zombie added
                ItemManager.Instance.SpawnOneZombie(survivial.OccupiedTile);
            }

            if (!survivial._hasArmor && !survivial._hasWeapon) {
                return;
            }

            int range = 1;
            if (Random.Range(0f, 100f) > 5f && survivial._hasArmor) {
                //drop armor
                ITile freeTile;
                while (true) {
                    var tiles = _rangeFinder.GetTilesInRange(survTile, range).Where(x => x.Walkable).ToList();
                    if (tiles.Any()) {
                        freeTile = tiles.First();
                        break;
                    }

                    range++;
                }

                ItemManager.Instance.SpawnOneArmor(freeTile);
            }

            if (Random.Range(0f, 100f) > 5f && survivial._hasArmor) {
                //drop weapon
                ITile freeTile;
                while (true) {
                    var tiles = _rangeFinder.GetTilesInRange(survTile, range).Where(x => x.Walkable).ToList();
                    if (tiles.Any()) {
                        freeTile = tiles.First();
                        break;
                    }

                    range++;
                }

                ItemManager.Instance.SpawnOneWeapon(freeTile);
            }
        }

        protected IUnit checkBattle(IUnit unit) {
            var neighbours = _rangeFinder.GetTilesInRange(this.OccupiedTile, 1);
            foreach (var item in neighbours) {
                if (item.OccupiedUnit != null && (item.OccupiedUnit.GetType() == typeof(Survivial) ||
                                                  item.OccupiedUnit.GetType() == typeof(Zombie))) {
                    var uitem = (IUnit)item.OccupiedUnit;
                    if (uitem.unitFaction != this.unitFaction) {
                        return uitem;
                    }
                }
            }

            return null;
        }
    }
}