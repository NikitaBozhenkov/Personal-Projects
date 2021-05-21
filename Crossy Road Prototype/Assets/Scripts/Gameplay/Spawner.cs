using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    public enum GroundItemType {
        NonPlayableGrass, Grass_light, Grass_dark, Road, Railway, Water
    }

    [SerializeField] int currentSpawnZ = -50;
    [SerializeField] GameObject treePref;

    [SerializeField] GroundLine[] grassPrefs = new GroundLine[2];
    [SerializeField] GroundLine[] otherGroundPrefs = new GroundLine[3];
    [SerializeField] GameObject playerPref;
    [SerializeField] CarController[] carPrefs = new CarController[3];
    [SerializeField] LogController logPref;


    private bool grassWasLast;

    public GameObject SpawnPlayer(Vector3 playerStartPosition) {
        var player = Instantiate(playerPref, playerStartPosition, playerPref.transform.rotation);
        Camera.main.GetComponent<CameraSideFollow>().target = player.transform;
        return player;
    }

    private List<CarController> GetRandomCars(int count) {
        List<CarController> cars = new List<CarController>(carPrefs);
        while(cars.Count != count) {
            cars.RemoveAt(Random.Range(0, cars.Count - 1));
        }

        return cars;
    }

    public List<HashSet<int>> SpawnParts() {
        int count = 1;
        List<HashSet<int>> obstacles = new List<HashSet<int>>();
        HashSet<int> curLine = new HashSet<int>();

        if (grassWasLast) {
            // Добавить шансы чтобы железка не спавнилась часто 
            var spawnObj = otherGroundPrefs[Random.Range(0, 3)];

            switch(spawnObj.name) {
                case "Railway": {
                        SpawnItem(spawnObj.GetComponent<GroundLine>());
                        obstacles.Add(curLine);
                        break;
                    }
                case "Road": {
                        count = Random.Range(1, 4);
                        var cars = GetRandomCars(count);

                        for (int i = 0; i < count; ++i) {
                            var obj = SpawnItem(spawnObj.GetComponent<GroundLine>());
                            var rc = obj.GetComponent<RoadController>();
                            rc.car = cars[i];
                            rc.movementDirection = (Random.Range(0, 2f) >= 1f) ? 1 : -1;
                            rc.repeatDelay = Random.Range(1f, 2f);
                            rc.StartSpawn();
                            obstacles.Add(curLine);
                        }

                        break;
                    }
                case "Water": {
                    SpawnGrass(1, true);
                    obstacles.Add(curLine);
                    var spawn = SpawnItem(spawnObj.GetComponent<GroundLine>()).GetComponent<WaterController>();
                    var set = spawn.RandomLilies();
                    set.Add(-1);
                    obstacles.Add(set);
                    SpawnGrass(1, true);
                    obstacles.Add(curLine);
                    break;
                    }
            }

        } else {
            count = Random.Range(1, 3);

            obstacles = SpawnGrass(count);
        }

        grassWasLast = !grassWasLast;
        return obstacles;
    }

    public List<HashSet<int>> SpawnGrass(int count, bool start = false) {
        List<HashSet<int>> obstacles = new List<HashSet<int>>();
        HashSet<int> emptySet = new HashSet<int>();

        for (int i = 0; i < count; ++i) {
            var item = SpawnItem(grassPrefs[CalculateGrassIndex()].GetComponent<GroundLine>()).GetComponent<GrassController>();
            StartCoroutine(item.SpawnNonPlayableTrees());
            if (!start) obstacles.Add(item.RandomItemsOnPlayablePart());
            else obstacles.Add(emptySet);
        }

        return obstacles;
    }

    private void UpdateSpawnZ() => currentSpawnZ += 10;
    private int CalculateGrassIndex() {
        var temp = currentSpawnZ;
        while(temp % 10 == 0 && temp != 0) {
            temp /= 10;
        }

        return (temp % 2 == 0) ? 0 : 1;
    }


    public IEnumerator SpawnNonPlayablePart() {
        currentSpawnZ = -50;
        while (currentSpawnZ != 0) {
            var line = grassPrefs[CalculateGrassIndex()];
            var inst = SpawnItem(line).GetComponent<GrassController>();
            StartCoroutine(inst.SpawnNonPlayableTrees());
            StartCoroutine(inst.MakeNonPlayablePart());
            yield return new WaitForSeconds(.1f);
        }
    }

    public GameObject SpawnItem(GroundLine line) {
        var inst = PoolManager.GetObject(line.gameObject.name, new Vector3(0, 0, currentSpawnZ), line.transform.rotation);
        Debug.Log("Spawned: " + currentSpawnZ + " " + inst.gameObject.name);
        UpdateSpawnZ();
        return inst;
    }
}
