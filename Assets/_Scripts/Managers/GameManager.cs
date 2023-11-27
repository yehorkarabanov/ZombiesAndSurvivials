using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Managers {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance;
        public GameState gameState;
        [SerializeField] public Camera _cam;
        private Coroutine stateCoroutine = null;

        void Awake() {
            Instance = this;
        }

        public void ChangeState(GameState newState) {
            if (stateCoroutine != null) {
                StopCoroutine(stateCoroutine);
            }

            stateCoroutine = StartCoroutine(ChangeStateCoroutine(newState));
        }

        private IEnumerator ChangeStateCoroutine(GameState newState) {
            gameState = newState;
            while (gameState != GameState.Turns) {
                switch (gameState) {
                    case GameState.ClearItems:
                        ItemManager.Instance.ClearItems();
                        PostGameManager.Instance._canvas.SetActive(false);
                        gameState = GameState.SpawnItems;
                        break;
                    case GameState.ClearItemsWithGrid:
                        ItemManager.Instance.ClearItems();
                        GridManager.Instance.clearAllTiles();
                        PostGameManager.Instance._canvas.SetActive(false);
                        gameState = GameState.GenerateGrid;
                        break;
                    case GameState.GenerateGrid:
                        GridManager.Instance.GenerateGrid();
                        _cam.transform.position = new Vector3((float)GridManager.Instance._width / 2 - 0.5f,
                            (float)GridManager.Instance._height / 2 - 0.5f, -10);
                        gameState = GameState.RenderGrid;
                        break;
                    case GameState.RenderGrid:
                        GridManager.Instance.RenderGrid();
                        gameState = GameState.SpawnItems;
                        break;
                    case GameState.SpawnItems:
                        ItemManager.Instance.SpawnSurvivials();
                        ItemManager.Instance.SpawnZombie();
                        ItemManager.Instance.SpawnWeapon();
                        ItemManager.Instance.SpawnArmor();
                        gameState = GameState.Turns;
                        break;
                }
            }
            while (true) {
                if (ItemManager.Instance._zombieFactCount == 0) {
                    PostGameManager.Instance.setWinText("Survivials");
                    yield break;
                }
            
                if (ItemManager.Instance._survivialsCount == 0) {
                    PostGameManager.Instance.setWinText("Zombie");
                    yield break;
                }
            
                // MoveManager.Instance.UnitMove();
                var a = StartCoroutine(MoveManager.Instance.MoveUnits());
                yield return a;
            }
        }
    }

    public enum GameState {
        GenerateGrid,
        RenderGrid,
        SpawnItems,
        ClearItems,
        ClearItemsWithGrid,
        Turns,
    }
}