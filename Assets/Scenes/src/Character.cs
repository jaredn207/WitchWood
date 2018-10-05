using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int currentX { set; get; }
    public int currentY { set; get; }
    public bool isAlly;

    public int healthPoints;
    public int defence;
    public int moveDistance;
    public bool wasMoved;

    public void setPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }

    public virtual bool possibleMove(int x, int y)
    {
        return true;
    }
}
