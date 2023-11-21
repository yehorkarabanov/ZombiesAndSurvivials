using _Scripts.Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PregameManager : MonoBehaviour {
    [FormerlySerializedAs("panelToShow")] public GameObject _canvas;
    void Update() {
        if (Input.anyKey) {
            _canvas.SetActive(false);
            GameManager.Instance.ChangeState(GameState.GenerateGrid);
        }
    }
}