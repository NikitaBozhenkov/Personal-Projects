using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : GroundLine
{
    public CarController car;
    public float repeatDelay = 2f;
    public int movementDirection;

    public void StartSpawn() {
        InvokeRepeating(nameof(SpawnOneObject), 0, repeatDelay);
    }   

    private void SpawnOneObject() {
        var inst = PoolManager.GetObject(car.gameObject.name,
            transform.position + movementDirection * new Vector3(leftNonPlayablePart.transform.localScale.x, 0,0) + Vector3.up * 13, 
            Quaternion.Euler(0, 90 + movementDirection * 90, 0)).GetComponent<CarController>();
        inst.speed = -movementDirection * Mathf.Abs(inst.speed);
        inst.Move(leftNonPlayablePart.transform.localScale.x + playablePart.transform.localScale.x);
    }


}
