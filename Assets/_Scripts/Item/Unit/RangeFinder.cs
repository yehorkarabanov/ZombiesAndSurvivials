using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Tile;
using UnityEngine;

public class RangeFinder {
    public List<ITile> GetTilesInRange(ITile startTile, int range) {
        var inRangeTiles = new List<ITile>();
        int stepCount = 0;

        inRangeTiles.Add(startTile);

        var tilesForPreviousStep = new List<ITile>();
        tilesForPreviousStep.Add(startTile);
        while (stepCount < range) {
            var surroundingTiles = new List<ITile>();
            foreach (var item in tilesForPreviousStep) {
                surroundingTiles.AddRange(GetNeighbourTiles(item));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tilesForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        inRangeTiles.Remove(startTile);
        return inRangeTiles.Distinct().ToList();
    }

    private List<ITile> GetNeighbourTiles(ITile currentITile) {
        var map = GridManager.Instance._tiles;
        List<ITile> neighbours = new List<ITile>();

        //top
        Vector2 locationToCheck = new Vector2(currentITile.x, currentITile.y + 1);
        if (map.ContainsKey(locationToCheck)) {
            neighbours.Add(map[locationToCheck]);
        }

        //bottom
        locationToCheck = new Vector2(currentITile.x, currentITile.y - 1);
        if (map.ContainsKey(locationToCheck)) {
            neighbours.Add(map[locationToCheck]);
        }

        //rigth
        locationToCheck = new Vector2(currentITile.x + 1, currentITile.y);
        if (map.ContainsKey(locationToCheck)) {
            neighbours.Add(map[locationToCheck]);
        }

        //left
        locationToCheck = new Vector2(currentITile.x - 1, currentITile.y);
        if (map.ContainsKey(locationToCheck)) {
            neighbours.Add(map[locationToCheck]);
        }

        return neighbours;
    }
}