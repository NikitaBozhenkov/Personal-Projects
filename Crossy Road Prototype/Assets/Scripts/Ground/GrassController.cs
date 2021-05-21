using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : GroundLine
{
    [SerializeField] GameObject[] objectPrefabs = new GameObject[2];
    [SerializeField] GameObject coinPref;

    public IEnumerator SpawnNonPlayableTrees() {
        for (int i = leftBound; i < leftPlayBound; i += 10) {
            SpawnObject(new Vector3(i,0,0), objectPrefabs[0]);
            yield return new WaitForSeconds(.05f);
        }
        for (int i = rightPlayBound + 10; i <= rightBound; i += 10) {
            SpawnObject(new Vector3(i, 0, 0), objectPrefabs[0]);
            yield return new WaitForSeconds(.05f);
        }
    }

    public IEnumerator MakeNonPlayablePart() {
        for (int i = leftBound; i <= rightBound; i += 10) {
            SpawnObject(new Vector3(i, 0, 0), objectPrefabs[0]);
            yield return new WaitForSeconds(.05f);
        }
    }

    public void SpawnObject(Vector3 pos, GameObject obj) {
        PoolManager.GetObject(obj.name, transform.position + pos, Quaternion.identity);
    }

    public HashSet<int> RandomItemsOnPlayablePart() {
        int count = Random.Range(1, 4);
        HashSet<int> obstaclesPositions = new HashSet<int>();

        while(obstaclesPositions.Count != count) {
            obstaclesPositions.Add(Random.Range(0, squaresCount));
        }

        foreach(var index in obstaclesPositions) {
            SpawnObject(new Vector3(index * 10 - 40, 0, 0), objectPrefabs[Random.Range(0, 2)]);
        }

        for(int i = 0; i < 9; ++i) {
            if(!obstaclesPositions.Contains(i)) {
                if(Random.Range(0, 100) <= 3)
                SpawnObject(new Vector3(i * 10 - 40, 3.2f, 0), coinPref);
            }
        }

        return obstaclesPositions;
    }
}
