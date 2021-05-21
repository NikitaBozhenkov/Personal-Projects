using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRemover : MonoBehaviour
{
    private List<string> linesNames = new List<string>{
        "Grass_dark", "Grass_light", "Railway", "Road", "Water"
    };

    private void OnCollisionEnter(Collision collision) {
        var obj = collision.gameObject.GetComponentInParent<PoolObject>();
        if (obj != null) {
            obj.ReturnToPool();

            if(linesNames.Contains(obj.gameObject.name)) {
                Debug.Log("LineDeleted");
                EventBroker.CallLineDeleted();
            }
        }
    }
}
