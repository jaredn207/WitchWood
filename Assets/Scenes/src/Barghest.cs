using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barghest : Character
{

    public override bool[,] possibleMove()
    {
        bool[,] r = new bool[BoardManager.boardSize, BoardManager.boardSize];

        int k = 0;
        int x = currentX;
        int y = currentY + moveDistance;

        //top-half
        while (k < moveDistance * 2 + 1)
        {
            for (int i = 0; i <= k; i++)
            {
                if ((x + i >= 0 && y < BoardManager.boardSize) && (x + i < BoardManager.boardSize && y >= 0))
                    r[x + i, y] = true;
            }
            x--;
            y--;
            k = k + 2;
        }
        x = x + 2;
        k = k - 4;
        //bottom-half
        while (k >= 0)
        {
            for (int i = 0; i <= k; i++)
            {
                if ((x + i >= 0 && y < BoardManager.boardSize) && (x + i < BoardManager.boardSize && y >= 0))
                    r[x + i, y] = true;
            }
            k = k - 2;
            x++;
            y--;
        }

        //remove the highlight on the character
        r[currentX, currentY] = false;

        return r;
    }
}
