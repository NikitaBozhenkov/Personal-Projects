using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;

public class CarController : PoolObject
{
    public int speed = 25;
    private Tween tw;

    public void Move(float distance) {
        tw = transform.DOMoveX(distance * Mathf.Sign(speed), distance / Mathf.Abs(speed)).OnComplete(ReturnToPool);
    }

    private void OnDisable() {
        tw.Kill();
    }
}
