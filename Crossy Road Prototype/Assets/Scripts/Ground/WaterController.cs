using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : GroundLine
{
    [SerializeField] GameObject lilyPref;
    public LogController log;
    public float repeatDelay = 2f;
    public int movementDirection;

    public void StartSpawn() {
        InvokeRepeating(nameof(SpawnOneObject), 0, repeatDelay);
    }

    private void SpawnOneObject() {
        var inst = PoolManager.GetObject(log.gameObject.name,
            transform.position + movementDirection * new Vector3(leftNonPlayablePart.transform.localScale.x, 0, 0) + Vector3.up * 1,
            Quaternion.Euler(0, 90 + movementDirection * 90, 0)).GetComponent<CarController>();
        inst.speed = -movementDirection * Mathf.Abs(inst.speed);
        inst.Move(leftNonPlayablePart.transform.localScale.x + playablePart.transform.localScale.x);
    }

    public void SpawnObject(Vector3 pos, GameObject obj) {
        PoolManager.GetObject(obj.name, transform.position + pos, Quaternion.identity);
    }

    public HashSet<int> RandomLilies() {
        int count = Random.Range(1, 3);
        HashSet<int> obstaclesPositions = new HashSet<int>();

        while (obstaclesPositions.Count != count) {
            obstaclesPositions.Add(Random.Range(2, squaresCount - 2));
        }

        foreach (var index in obstaclesPositions) {
            SpawnObject(new Vector3(index * 10 - 40, 0, 0), lilyPref);
        }


        return obstaclesPositions;
    }
}
