using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSideFollow : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed;
    public float speed;
    public float xOffset;
    public float zOffsetBound;

    private void Start() {
        xOffset = transform.position.x;
    }

    void FixedUpdate() {
        if (target == null) return;

        Follow(true, target.position.z - transform.position.z > zOffsetBound);
        if(GameManager.Instance.IsGameStarted()) Move();

        if(target.position.z - transform.position.z < 5) {
            target.gameObject.GetComponent<PlayerKiller>().SetKillMethod(PlayerKiller.KillMethod.Bird);
            EventBroker.CallKillPlayer();
        }
    }

    private void Follow(bool x, bool z) {
        var desiredPosition = transform.position;
        if(x)
        desiredPosition.x = target.position.x + xOffset;
        if(z)
        desiredPosition.z = target.position.z - zOffsetBound;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void Move() {
        var desiredPosition = transform.position + Vector3.forward  ;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, speed);
        transform.position = smoothedPosition;
    }
}
