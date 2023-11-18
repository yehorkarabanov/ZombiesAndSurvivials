using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Tile;
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
            for (int i = 0; i < speed; i++) {
                CurrentMove();
                var opunit = checkBattle(this);
                if (opunit != null) {
                    battle(this, opunit);
                }
            }
        }

        protected abstract void CurrentMove();

        protected void battle(IUnit survivial, IUnit zombie) {
            if (survivial.GetType() != typeof(Survivial)) {
                var temp = survivial;
                survivial = (Survivial)zombie;
                zombie = (Zombie)temp;
            } else {
                survivial = (Survivial)survivial;
                zombie = (Zombie)zombie;
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