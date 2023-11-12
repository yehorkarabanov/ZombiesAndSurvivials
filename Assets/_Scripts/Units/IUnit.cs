using _Scripts.Tile;
using UnityEngine;

namespace _Scripts.Units {
    public class IUnit : MonoBehaviour {
        public ITile OccupiedTile;
        public Faction Faction;
    }
}