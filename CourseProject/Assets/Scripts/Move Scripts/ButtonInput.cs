using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonInput : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    private PlayerMove _playerMove;

    private void Start() {
        _playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        switch (gameObject.name) {
            case "Left Button":
                _playerMove.HorizontalInput = -1;
                break;
            case "Right Button":
                _playerMove.HorizontalInput = 1;
                break;
            case "Jump Button":
                _playerMove.Jump = true;
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        switch (gameObject.name) {
            case "Left Button":
            case "Right Button":
                _playerMove.HorizontalInput = 0;
                break;
            case "Jump Button":
                _playerMove.Jump = false;
                break;
        }
    }
}