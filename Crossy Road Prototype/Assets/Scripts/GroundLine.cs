using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLine : PoolObject
{
    [SerializeField] protected GameObject playablePart;
    [SerializeField] protected GameObject leftNonPlayablePart;
    [SerializeField] protected GameObject rightNonPlayablePart;
    protected const int leftBound = -190;
    protected const int rightBound = 190;
    protected const int leftPlayBound = -40;
    protected const int rightPlayBound = 40;
    protected const int squaresCount = 9;
}
