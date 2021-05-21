using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWall : MonoBehaviour {
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float speed;
    
    private GameplayController _gameplayController;

    private void Start() {
        _gameplayController = GameObject.Find("Gameplay Controller").GetComponent<GameplayController>();
    }

    void Update() {
        if(_gameplayController.IsGameOver) return;

        if (transform.position.z < -10) {
            var pos = transform.position;
            pos.z = 90;
            transform.position = pos;
        }

        transform.Translate(moveDirection * (speed * Time.deltaTime));
    }
}