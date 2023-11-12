using _Scripts.Tile;
using UnityEngine;

namespace _Scripts.Item {
    public abstract class IItem : MonoBehaviour {
        public ITile OccupiedTile;
    }
}