using System.Collections.Generic;
using System.Linq;
using _Scripts.Tile;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Managers {
    public class GridManager : MonoBehaviour {
        public static GridManager Instance;
        [SerializeField] private int _width, _height;
        [SerializeField] private ITile _grassTile, _waterTile, _mountainTile;
        [SerializeField] private Camera _cam;
        private Dictionary<Vector2, ITile> _tiles;

        private void Awake() {
            Instance = this;
        }

        public void GenerateGrid() {
            _tiles = new Dictionary<Vector2, ITile>();
            ITile spawnedTile;
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    if (x == 0 || y == 0 || y == _height - 1 || x == _width - 1) {
                        spawnedTile = Instantiate(_mountainTile, new Vector3(x, y), Quaternion.identity);
                    } else {
                        float noiseValue = Mathf.PerlinNoise(x * 0.2f, y * 0.2f);

                        if (noiseValue < 0.2f) {
                            spawnedTile = Instantiate(_mountainTile, new Vector3(x, y), Quaternion.identity);
                        } else if (noiseValue > 0.8f) {
                            spawnedTile = Instantiate(_waterTile, new Vector3(x, y), Quaternion.identity);
                        } else {
                            spawnedTile = Instantiate(_grassTile, new Vector3(x, y), Quaternion.identity);
                        }
                    }

                    spawnedTile.name = $"tile {x} {y}";
                    spawnedTile.Init();
                    _tiles[new Vector2(x, y)] = spawnedTile;
                }
            }

            _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

            GameManager.Instance.ChangeState(GameState.SpawnSurvivials);
        }

        public ITile GetSurvivialSpawnTile() {
            return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        }
        public ITile GetZombieSpawnTile() {
            return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
        }
    }
}