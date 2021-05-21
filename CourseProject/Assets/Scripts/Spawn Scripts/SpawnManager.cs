using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour {
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject powerupPrefab;

    [SerializeField] private float zSpawn;
    [SerializeField] private float ySpawn;

    [SerializeField] private float xSpawnRange;

    [SerializeField] private float enemySpawnDelay = 1f;
    [SerializeField] private float obstacleSpawnDelay = 1.5f;
    [SerializeField] private float powerupSpawnDelay = 5f;
    private const float SpawnDelayBound = 0.5f;

    private GameplayController _gameplayController;
    
    void Start() {
        _gameplayController = GameObject.Find("Gameplay Controller").GetComponent<GameplayController>();
        for (var i = 0; i < 3; ++i) {
            StartCoroutine(SpawnObject(i));
        }
    }

    private IEnumerator SpawnObject(int key) {
        while (!_gameplayController.IsGameOver) {
            var delay = 1f;
            var go = new GameObject();
            switch (key) {
                case 0:
                    delay = enemySpawnDelay;
                    go = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
                    break;
                case 1:
                    delay = obstacleSpawnDelay;
                    go = obstaclePrefabs[Random.Range(0, enemyPrefabs.Length)];
                    break;
                case 2:
                    delay = powerupSpawnDelay;
                    go = powerupPrefab;
                    break;
            }

            yield return new WaitForSeconds(delay);
            if (_gameplayController.IsGameOver) continue;
            var spawnPos = new Vector3(Random.Range(-xSpawnRange, xSpawnRange), ySpawn, zSpawn);

            Instantiate(go, spawnPos, go.transform.rotation);
        }
    }

    private void Update() {
        if (enemySpawnDelay > SpawnDelayBound) enemySpawnDelay -= Time.deltaTime / 100;
        if (obstacleSpawnDelay > SpawnDelayBound) obstacleSpawnDelay -= Time.deltaTime / 100;
    }
}