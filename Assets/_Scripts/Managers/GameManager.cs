using System;
using UnityEngine;

namespace _Scripts.Managers {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance;
        public GameState GameState;

        void Awake() {
            Instance = this;
        }

        void Start() {
            ChangeState(GameState.GenerateGrid);
        }

        public void ChangeState(GameState newState) {
            GameState = newState;
            switch (newState) {
                case GameState.GenerateGrid:
                    GridManager.Instance.GenerateGrid();
                    break;
                case GameState.SpawnSurvivials:
                    UnitManager.Instance.SpawnSurvivials();
                    break;
                case GameState.SpawnZombie:
                    UnitManager.Instance.SpawnZombie();
                    break;
                case GameState.SurvivialsTurn:
                    break;
                case GameState.ZombieTurn:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
            }
        }
    
    }

    public enum GameState {
        GenerateGrid = 0,
        SpawnSurvivials = 1,
        SpawnZombie = 2,
        SurvivialsTurn = 3,
        ZombieTurn = 4,
    }
}