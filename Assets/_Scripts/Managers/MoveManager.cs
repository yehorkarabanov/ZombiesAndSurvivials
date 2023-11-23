using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Item.Unit;
using _Scripts.Managers;
using UnityEngine;

public class MoveManager : MonoBehaviour {
    public static MoveManager Instance;

    private void Awake() {
        Instance = this;
    }

    public void UnitMove() {
        StartCoroutine(MoveUnitsWithDelay());

        if (ItemManager.Instance._zombieFactCount == 0) {
            PostGameManager.Instance.setWinText("Survivials");
        } else if (ItemManager.Instance._survivialsCount == 0) {
            PostGameManager.Instance.setWinText("Zombie");
        }
    }

    IEnumerator MoveUnitsWithDelay() {
        for (var i = 0; i < ItemManager.Instance.ListUnits.Count; i++) {
            var unit = ItemManager.Instance.ListUnits[i];
            if (unit.GetType() == typeof(Survivial)) {
                var survivial = (Survivial)unit;
                survivial.Move();
            } else if (unit.GetType() == typeof(Zombie)) {
                var zombie = (Zombie)unit;
                zombie.Move();
            }

            // yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(0f);
        }

        GameManager.Instance.ChangeState(GameState.Turns);
    }
}