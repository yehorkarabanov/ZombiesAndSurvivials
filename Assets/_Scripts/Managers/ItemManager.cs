using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _Scripts.Item.Equip;
using _Scripts.Item.Unit;
using _Scripts.Tile;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using ScriptableUnit = _Scripts.Item.Unit.ScriptableUnit;
using Vector3 = UnityEngine.Vector3;

namespace _Scripts.Managers {
    public class ItemManager : MonoBehaviour {
        [SerializeField] private int _survivialsCount, _zombieCount, _armorCount, _weaponCount;

        public static ItemManager Instance;

        private List<ScriptableUnit> _units;
        private List<ScriptableEquip> _equips;

        public List<IUnit> ListUnits = new List<IUnit>();
        public List<IEquip> listEquip = new List<IEquip>();

        public void Awake() {
            Instance = this;

            _units = Resources.LoadAll<ScriptableUnit>("Items/Units").ToList();
            _equips = Resources.LoadAll<ScriptableEquip>("Items/Equip").ToList();
        }

        public void ClearItems() {
            foreach (var unit in ListUnits) {
                unit.OccupiedTile.OccupiedUnit = null;
                Destroy(unit.gameObject);
            }

            ListUnits = new List<IUnit>();

            foreach (var equip in listEquip) {
                equip.OccupiedTile.OccupiedUnit = null;
                Destroy(equip.gameObject);
            }

            listEquip = new List<IEquip>();
        }

        public void SpawnSurvivials() {
            ClearItems();

            for (int i = 0; i < _survivialsCount; i++) {
                var item = GetRandomUnit<Survivial>(UnitFaction.Survivials);
                var sitem = Instantiate(item);
                sitem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                var randomSpawnTile = GridManager.Instance.GetSurvivialSpawnTile();
                randomSpawnTile.SetUnit(sitem);
                ListUnits.Add(sitem);
            }

            GameManager.Instance.ChangeState(GameState.SpawnZombie);
        }

        public void SpawnZombie() {
            for (int i = 0; i < _zombieCount; i++) {
                var item = GetRandomUnit<Zombie>(UnitFaction.Zombie);
                var sitem = Instantiate(item);
                sitem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                var randomSpawnTile = GridManager.Instance.GetZombieSpawnTile();
                randomSpawnTile.SetUnit(sitem);
                ListUnits.Add(sitem);
            }

            GameManager.Instance.ChangeState(GameState.SpawnWeapon);
        }

        public void SpawnOneZombie(ITile tile) {
            var item = GetRandomUnit<Zombie>(UnitFaction.Zombie);
            var sitem = Instantiate(item);
            sitem.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            tile.SetUnit(sitem);
            ListUnits.Add(sitem);
        }

        public void SpawnWeapon() {
            for (int i = 0; i < _weaponCount; i++) {
                var item = GetRandomEquip<Weapon>(EquipFaction.Weapon);
                var sitem = Instantiate(item);
                sitem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                var randomSpawnTile = GridManager.Instance.GetAnyTile();
                randomSpawnTile.SetUnit(sitem);
                var pos = sitem.transform.position;
                sitem.transform.position = new Vector3(pos.x, pos.y, -1);
                listEquip.Add(sitem);
            }

            GameManager.Instance.ChangeState(GameState.SpawnArmor);
        }

        public void SpawnOneWeapon(ITile tile) {
            var item = GetRandomEquip<Weapon>(EquipFaction.Weapon);
            var sitem = Instantiate(item);
            sitem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            tile.SetUnit(sitem);
            var pos = sitem.transform.position;
            sitem.transform.position = new Vector3(pos.x, pos.y, -1);
            listEquip.Add(sitem);
        }

        public void SpawnArmor() {
            for (int i = 0; i < _armorCount; i++) {
                var item = GetRandomEquip<Armor>(EquipFaction.Armor);
                var sitem = Instantiate(item);
                sitem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                var randomSpawnTile = GridManager.Instance.GetAnyTile();
                randomSpawnTile.SetUnit(sitem);
                listEquip.Add(sitem);
            }

            GameManager.Instance.ChangeState(GameState.Turns);
        }

        public void SpawnOneArmor(ITile tile) {
            var item = GetRandomEquip<Armor>(EquipFaction.Armor);
            var sitem = Instantiate(item);
            sitem.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            tile.SetUnit(sitem);
            listEquip.Add(sitem);
        }


        private T GetRandomUnit<T>(UnitFaction unitFaction) where T : IUnit {
            return (T)_units.First(u => u.unitFaction == unitFaction).UnitPrefab;
        }

        private T GetRandomEquip<T>(EquipFaction equipFaction) where T : IEquip {
            return (T)_equips.First(u => u.equipFaction == equipFaction).equipPrefab;
        }
    }
}