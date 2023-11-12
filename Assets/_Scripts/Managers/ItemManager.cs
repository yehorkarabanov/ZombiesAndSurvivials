using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Scripts.Item.Equip;
using _Scripts.Item.Unit;
using UnityEngine;
using Random = UnityEngine.Random;
using ScriptableUnit = _Scripts.Item.Unit.ScriptableUnit;
using Vector3 = UnityEngine.Vector3;

namespace _Scripts.Managers {
    public class ItemManager: MonoBehaviour {
        [SerializeField] private int _survivialsCount, _zombieCount, _armorCount, _weaponCount;
        
        public static ItemManager Instance;

        private List<ScriptableUnit> _units;
        private List<ScriptableEquip> _equips;
    
        public void Awake() {
            Instance = this;

            _units = Resources.LoadAll<ScriptableUnit>("Items/Units").ToList();
            _equips = Resources.LoadAll<ScriptableEquip>("Items/Equip").ToList();
        }

        public void SpawnSurvivials() {
            for (int i = 0; i < _survivialsCount; i++) {
                var sunit = GetRandomUnit<Survivial>(UnitFaction.Survivials);
                var ssunit = Instantiate(sunit);
                ssunit.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                var randomSpawnTile = GridManager.Instance.GetSurvivialSpawnTile();
                randomSpawnTile.SetUnit(ssunit);
            }
            GameManager.Instance.ChangeState(GameState.SpawnZombie);
        }
        
        public void SpawnZombie() {
            for (int i = 0; i < _zombieCount; i++) {
                var sunit = GetRandomUnit<Zombie>(UnitFaction.Zombie);
                var ssunit = Instantiate(sunit);
                ssunit.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
                var randomSpawnTile = GridManager.Instance.GetZombieSpawnTile();
                randomSpawnTile.SetUnit(ssunit);
            }
            GameManager.Instance.ChangeState(GameState.SpawnWeapon);
        }
        
        public void SpawnWeapon() {
            for (int i = 0; i < _armorCount; i++) {
                var sunit = GetRandomEquip<Weapon>(EquipFaction.Weapon);
                var ssunit = Instantiate(sunit);
                ssunit.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                var randomSpawnTile = GridManager.Instance.GetAnyTile();
                randomSpawnTile.SetUnit(ssunit);
            }
            GameManager.Instance.ChangeState(GameState.SpawnArmor);
        }
        
        public void SpawnArmor() {
            for (int i = 0; i < _weaponCount; i++) {
                var sunit = GetRandomEquip<Armor>(EquipFaction.Armor);
                var ssunit = Instantiate(sunit);
                ssunit.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                var randomSpawnTile = GridManager.Instance.GetAnyTile();
                randomSpawnTile.SetUnit(ssunit);
            }
            GameManager.Instance.ChangeState(GameState.SurvivialsTurn);
        }



        private T GetRandomUnit<T>(UnitFaction unitFaction) where T : IUnit {
            return (T)_units.First(u => u.unitFaction == unitFaction).UnitPrefab;
        }
        private T GetRandomEquip<T>(EquipFaction equipFaction) where T : IEquip {
            return (T)_equips.First(u => u.equipFaction == equipFaction).equipPrefab;
        }
    }
}