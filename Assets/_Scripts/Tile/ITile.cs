using _Scripts.Item;
using _Scripts.Item.Unit;
using UnityEngine;

namespace _Scripts.Tile {
    public abstract class ITile : MonoBehaviour {
        public int G;
        public int H;

        public int F {
            get { return G + H; }
        }

        [SerializeField] private Color _baseColor;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private bool _isWalkable;

        public IItem OccupiedUnit;
        public float x, y;
        public bool Walkable => _isWalkable && OccupiedUnit == null;

        public void Init(float X, float Y) {
            _renderer.color = _baseColor;
            x = X;
            y = Y;
        }

        public void SetUnit(IItem unit) {
            if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
            unit.transform.position = transform.position;
            OccupiedUnit = unit;
            unit.OccupiedTile = this;
        }
    }
}