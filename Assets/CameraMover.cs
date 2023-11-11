using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour {
    private Vector3 CammeraPossition;
    [SerializeField] private float CamSpeed;

    private void Start() {
        
    }

    void Update() {
        CammeraPossition = transform.position;
        
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= zoomInput;
        
        if (Input.GetKey(KeyCode.W)) {
            CammeraPossition.y += CamSpeed / 50;
        }

        if (Input.GetKey(KeyCode.S)) {
            CammeraPossition.y -= CamSpeed / 50;
        }

        if (Input.GetKey(KeyCode.A)) {
            CammeraPossition.x -= CamSpeed / 50;
        }

        if (Input.GetKey(KeyCode.D)) {
            CammeraPossition.x += CamSpeed / 50;
        }

        transform.position = CammeraPossition;
    }
}