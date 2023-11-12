using _Scripts.Item;
using _Scripts.Item.Unit;
using UnityEngine;

namespace _Scripts.Tile {
    public abstract class ITile : MonoBehaviour {
        [SerializeField] private Color _baseColor;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private bool _isWalkable;

        public IItem OccupiedUnit;
        public bool Walkable => _isWalkable && OccupiedUnit == null;

        public void Init() {
            _renderer.color = _baseColor;
        }

        public void SetUnit(IItem unit) {
            if (unit.OccupiedTile != null) unit.OccupiedTile.OccupiedUnit = null;
            unit.transform.position = transform.position;
            OccupiedUnit = unit;
            unit.OccupiedTile = this;
        }
    }
}