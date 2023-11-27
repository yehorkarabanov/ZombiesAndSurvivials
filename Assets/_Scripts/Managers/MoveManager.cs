using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Scripts.Item.Unit;
using _Scripts.Managers;
using UnityEngine;

public class MoveManager : MonoBehaviour {
    public static MoveManager Instance;

    private void Awake() {
        Instance = this;
    }
    

    public IEnumerator MoveUnits() {
        for (var i = 0; i < ItemManager.Instance.ListUnits.Count; i++) {
            var unit = ItemManager.Instance.ListUnits[i];
            if (unit.GetType() == typeof(Survivial)) {
                var survivial = (Survivial)unit;
                survivial.Move();
            } else if (unit.GetType() == typeof(Zombie)) {
                var zombie = (Zombie)unit;
                zombie.Move();
            }

            yield return new WaitForSeconds(0f);
            //yield return new WaitForSeconds(0f);
        }

        //GameManager.Instance.ChangeState(GameState.Turns);
    }
}