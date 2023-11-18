using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Managers;
using _Scripts.Tile;
using UnityEngine;

public class PathFinder {
    public List<ITile> FindPath(ITile start, ITile end, bool exludeLast = true) {
        List<ITile> openList = new List<ITile>();
        List<ITile> closedList = new List<ITile>();
        openList.Add(start);

        while (openList.Count > 0) {
            ITile currentITile = openList.OrderBy(x => x.F).First();
            openList.Remove(currentITile);
            closedList.Add(currentITile);

            if (currentITile == end) {
                return GetFinishedList(start, end, exludeLast);
            }

            var neighbourTiles = GetNeighbourTiles(currentITile);
            foreach (var neighbour in neighbourTiles) {
                if ((!neighbour.Walkable || closedList.Contains(neighbour)) && neighbour != end) {
                    continue;
                }

                neighbour.G = getManhattemDistance(start, neighbour);
                neighbour.H = getManhattemDistance(end, neighbour);
                neighbour.previous = currentITile;

                if (!openList.Contains(neighbour)) {
                    openList.Add(neighbour);
                }
            }
        }

        return new List<ITile>();
    }

    private List<ITile> GetFinishedList(ITile start, ITile end, bool exludeLast) {
        List<ITile> finishedList = new List<ITile>();
        ITile currentTile = end;
        while (currentTile != start) {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }

        if (exludeLast) {
            finishedList.Remove(finishedList.First());
        }

        finishedList.Reverse();
        return finishedList;
    }

    private int getManhattemDistance(ITile start, ITile neighbour) {
        return (int)(Mathf.Abs(start.x - neighbour.x) + Mathf.Abs(start.y - neighbour.y));
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