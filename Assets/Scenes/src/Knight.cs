using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Character
{
    public override bool[,] possibleMove()
    {
        bool[,] r = new bool[BoardManager.boardSize, BoardManager.boardSize];

        //max move distance
        moveDistance = 2;

        //front
        for (int i = 0; i < moveDistance; i++)
        {
            if (currentY + (i + 1) < BoardManager.boardSize)
                r[currentX, currentY + (i + 1)] = true;
        }

        //right
        for (int i = 0; i < moveDistance; i++)
        {
            if (currentX + (i + 1) < BoardManager.boardSize)
                 r[currentX + (i + 1), currentY] = true;
        }

        //left
        for (int i = 0; i < moveDistance; i++)
        {
            if(currentX - (i + 1) >= 0)
                r[currentX - (i + 1), currentY] = true;
        }

        //down
        for (int i = 0; i < moveDistance; i++)
        {
            if(currentY - (i + 1) >= 0)
                r[currentX, currentY - (i + 1)] = true;
        }
        
        //diagonals
        int originX = currentX - 1;
        int originY = currentY - 1;
        for(int i = 0; i< 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if((originX+i >= 0 && originY+j < BoardManager.boardSize) && (originX+i < BoardManager.boardSize && originY+j >=0))
                    r[originX+i, originY+j] = true;
            }
        }

        //remove the highlight on the character
        r[currentX, currentY] = false;
        
        return r;
    }
}
