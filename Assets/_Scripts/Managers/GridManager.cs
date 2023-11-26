using System.Collections.Generic;
using System.Linq;
using _Scripts.Tile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Managers {
    public class GridManager : MonoBehaviour {
        public static GridManager Instance;
        // [SerializeField] private int _width, _height;
        [SerializeField] public int _width, _height;
        [SerializeField] public ITile _grassTile, _waterTile, _mountainTile;
        [SerializeField] public Camera _cam;
        public Dictionary<Vector2, ITile> _tiles = new Dictionary<Vector2, ITile>();
        private float randomNoise = 0;

        private void Awake() {
            Instance = this;
        }

        public void clearAllTiles() {
            if (this._tiles.Any()) {
                for (int x = 0; x < _width; x++) {
                    for (int y = 0; y < _height; y++) {
                        Destroy(_tiles[new Vector2(x, y)]);
                    }
                }
            }
        
            _tiles = new Dictionary<Vector2, ITile>();
            randomNoise = Random.Range(0, 10000);
        }
        
        public void GenerateGrid() {
            clearAllTiles();
            ITile spawnedTile;
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    if (x == 0 || y == 0 || y == _height - 1 || x == _width - 1) {
                        spawnedTile = Instantiate(_mountainTile, new Vector3(x, y), Quaternion.identity);
                    } else {
                        float noiseValue = Mathf.PerlinNoise((x + randomNoise) * 0.2f, (y + randomNoise) * 0.2f);
        
                        if (noiseValue < 0.2f) {
                            spawnedTile = Instantiate(_mountainTile, new Vector3(x, y), Quaternion.identity);
                        } else if (noiseValue > 0.8f) {
                            spawnedTile = Instantiate(_waterTile, new Vector3(x, y), Quaternion.identity);
                        } else {
                            spawnedTile = Instantiate(_grassTile, new Vector3(x, y), Quaternion.identity);
                        }
                    }
        
                    spawnedTile.name = $"tile {x} {y}";
                    spawnedTile.Init(x, y);
                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }
        
            _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
        
            GameManager.Instance.ChangeState(GameState.SpawnSurvivials);
        }

        public void RenderGrid() {
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    _tiles[new Vector2(x, y)].Render();
                }
            }
        }

        public ITile GetSurvivialSpawnTile() {
            return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        }

        public ITile GetZombieSpawnTile() {
            return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        }

        public ITile GetAnyTile() {
            return _tiles.Where(t => t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        }
    }
}