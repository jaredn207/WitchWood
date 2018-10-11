using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public int currentX { set; get; }
    public int currentY { set; get; }
    public bool isAlly;

    public int moveDistance;

    public void setPosition(int x, int y)
    {
        currentX = x;
        currentY = y;
    }

    public virtual bool[,] possibleMove()
    {
        return new bool[BoardManager.boardSize, BoardManager.boardSize];
    }
}
