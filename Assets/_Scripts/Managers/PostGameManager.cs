using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PostGameManager : MonoBehaviour {
    [FormerlySerializedAs("panelToShow")] public GameObject _canvas;
    [FormerlySerializedAs("text")] public TMP_Text _text;
    public static PostGameManager Instance;

    void Start() {
        Instance = this;
        _canvas.SetActive(false);
    }

    public void setWinText(string text) {
        _canvas.SetActive(true);
        _text.text = "Win - "+ text;
    }
}