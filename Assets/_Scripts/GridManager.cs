using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GridManager : MonoBehaviour {
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Camera _cam;

    private void Start() {
        GenerateGrid();
    }

    void GenerateGrid() {
        Tile[,] grid = new Tile[_width, _height];
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
                spawnedTile.name = $"tile {x} {y}";
                grid[x, y] = spawnedTile;
            }
        }

        GenerateProceduralGrid(grid);

        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    void GenerateProceduralGrid(Tile[,] grid) {
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                if (x == 0 || y == 0 || y == _height - 1 || x == _width - 1) {
                    grid[x, y].Init(Color.gray);
                }
                else {
                    float noiseValue = Mathf.PerlinNoise(x * 0.2f, y * 0.2f);

                    if (noiseValue < 0.2f) {
                        grid[x, y].Init(Color.gray);
                    }
                    else if (noiseValue > 0.8f) {
                        grid[x, y].Init(Color.blue);
                    }
                    else {
                        grid[x, y].Init(Color.green);
                    }
                }
            }
        }
    }
}