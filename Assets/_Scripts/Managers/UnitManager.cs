using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Scripts.Units;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace _Scripts.Managers {
    public class UnitManager: MonoBehaviour {
        [SerializeField] private int _survivialsCount, _zombieCount;
        
        public static UnitManager Instance;

        private List<ScriptableUnit> _units;
    
        public void Awake() {
            Instance = this;

            _units = Resources.LoadAll<ScriptableUnit>("Units").ToList();
        }

        public void SpawnSurvivials() {
            for (int i = 0; i < _survivialsCount; i++) {
                var sunit = GetRandomUnit<Survivial>(Faction.Survivials);
                var ssunit = Instantiate(sunit);
                ssunit.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                var randomSpawnTile = GridManager.Instance.GetSurvivialSpawnTile();
                randomSpawnTile.SetUnit(ssunit);
            }
            GameManager.Instance.ChangeState(GameState.SpawnZombie);
        }
        
        public void SpawnZombie() {
            for (int i = 0; i < _zombieCount; i++) {
                var sunit = GetRandomUnit<Zombie>(Faction.Zombie);
                var ssunit = Instantiate(sunit);
                ssunit.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                var randomSpawnTile = GridManager.Instance.GetZombieSpawnTile();
                randomSpawnTile.SetUnit(ssunit);
            }
            GameManager.Instance.ChangeState(GameState.SurvivialsTurn);
        }

        private T GetRandomUnit<T>(Faction faction) where T : IUnit {
            return (T)_units.First(u => u.Faction == faction).UnitPrefab;
        }
    }
}