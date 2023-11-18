using System;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Tile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Item.Unit {
    public class Zombie : IUnit {
        protected override void CurrentMove() {
            var possibleTargets = _rangeFinder.GetTilesInRange(this.OccupiedTile, rangeOfVision).Where(x => {
                    if (x.OccupiedUnit == null) {
                        return false;
                    }

                    if (x.OccupiedUnit.GetType() == typeof(Survivial)) {
                        return true;
                    }

                    return false;
                }
            ).ToList();
            if (possibleTargets.Any()) {
                targetTile = possibleTargets.OrderBy(x => Random.value).First();
                target = "survivial";
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
        }
    }
}