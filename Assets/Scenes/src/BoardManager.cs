using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour
{
    public Camera player1Camera, player2Camera;

    public static BoardManager Instance { set; get; }
    private bool[,] allowedMoves { set; get; }

    //array of characters that  are moveable
    public static Character[,] Characters { set; get; }
    public static Character selectedCharacter;

    //Tile size, middle of the first tile is (0.5,0.5)
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    //position of the mouse. Started at -1 and set to -1 if there is no real tile selection
    private int selectionX = -1;
    private int selectionY = -1;

    //Lists of playable characters
    public List<GameObject> entities;
    private List<GameObject> activeCharacter;

    //position of the character,(x,y,z). X flips the character up and down, y rotates the character,
    //z flips the character up and down in the other direction
    private Quaternion orientation = Quaternion.Euler(0, 0, 0);

    //Is it your turn?
    public bool isPlayer1Turn = true;

    //Board size, x * x. Only change this to modify the board size.
    public const int boardSize = 8;

    //Instantiate the activeCharacters list, set the Characters array to the entire board so that something can be selected at any position.
    //spawn the knight at (0,0) on the board
    private void Start()
    {
        activeCharacter = new List<GameObject>();
        Characters = new Character[boardSize,boardSize];

        //knight player 1
        spawnCharacter(0, 0, 0);
        Characters[0, 0].isPlayer1 = true;

        //knight player 2
        spawnCharacter(0, 7, 7);
        Characters[7, 7].isPlayer1 = false;
        Characters[7, 7].transform.localRotation *= Quaternion.Euler(0, 180, 0);

        player1Camera.enabled = true;
        player2Camera.enabled = false;
    }

    //Draws the board with the debugger, allows the player to move and select characters;
    private void Update()
    {
        UpdateSelection();
        drawBoard();

        //adjust the plane that detects tile clicking to the right size
        GameObject.Find("BoardPlane").transform.localScale = new Vector3((float)boardSize/10f, (float)boardSize/10f, (float)boardSize/10f);
        GameObject.Find("BoardPlane").transform.position = new Vector3((float)boardSize / 2f, 0f, (float)boardSize / 2f);

        if(Input.GetMouseButtonDown(0))
        {
            if(selectionX >= 0 && selectionY >= 0)
            {
                if(selectedCharacter == null)
                {
                    selectCharacter(selectionX, selectionY);
                }
                else
                {
                    moveCharacter(selectionX, selectionY);
                }
            }
        }

    }

    //select a character, x is the x-coordinate, y is the y-coordinate you clicked
    private void selectCharacter(int x, int y)
    {
        //if there is no character on a tile, return
        if (Characters[x, y] == null)
            return;
        
        //if there is no character on the tile and it's not your turn, return
        if (Characters[x, y].isPlayer1 != isPlayer1Turn)
            return;

        //if the character has already moved
        if (Characters[x, y].hasMoved == true)
            return;

        allowedMoves = Characters[x, y].possibleMove();

        //set the selected character to the character you clicked
        selectedCharacter = Characters[x, y];

        BoardHighlighter.Instance.highlightAllowedMoves(allowedMoves);
    }

    //move a character, x is the x-coordinate, y is the y-coordinate you clicked
    private void moveCharacter(int x, int y)
    {
        //if the selected tile is an enemy, damage the enemy, if they are at 0 hp, remove the enemy
        if(allowedMoves[x,y] == true && Characters[x,y] != null && Characters[selectedCharacter.currentX, selectedCharacter.currentY].isPlayer1 != Characters[x,y].isPlayer1)
        {
            Characters[selectedCharacter.currentX, selectedCharacter.currentY] = null;

            //Finds the nearest spot next to an enemy if you attack one.
            int tempX = 0, tempY = 0, newX = 0, newY = 0;
            double minDist = 0;

            //bottom
            tempX = x;
            tempY = y - 1;
            minDist = findDistance(tempX,tempY,selectedCharacter.currentX, selectedCharacter.currentY);
            newX = tempX;
            newY = tempY;

            //left
            tempX = x-1;
            tempY = y;
            if (minDist > findDistance(tempX, tempY, selectedCharacter.currentX, selectedCharacter.currentY))
            {
                minDist = findDistance(tempX, tempY, selectedCharacter.currentX, selectedCharacter.currentY);
                newX = tempX;
                newY = tempY;
            }

            //right
            tempX = x + 1;
            tempY = y;
            if (minDist > findDistance(tempX, tempY, selectedCharacter.currentX, selectedCharacter.currentY))
            {
                minDist = findDistance(tempX, tempY, selectedCharacter.currentX, selectedCharacter.currentY);
                newX = tempX;
                newY = tempY;
            }

            //top
            tempX = x;
            tempY = y+1;
            if (minDist > findDistance(tempX, tempY, selectedCharacter.currentX, selectedCharacter.currentY))
            {
                newX = tempX;
                newY = tempY;
            }

            selectedCharacter.transform.position = getTileCenter(newX, newY);
            selectedCharacter.setPosition(newX, newY);
            Characters[newX, newY] = selectedCharacter;

            Characters[x, y].hp--;
            if(Characters[x,y].hp <= 0)
            {
                Destroy(Characters[x, y].gameObject);
                Characters[x, y] = null;
            }
            Characters[x, y].hasMoved = true;
        }
        //if the position you selected is a valid move, move the character
        else if (allowedMoves[x, y] == true && Characters[x, y] == null)
        {
            Characters[selectedCharacter.currentX, selectedCharacter.currentY] = null;
            selectedCharacter.transform.position = getTileCenter(x, y);
            selectedCharacter.setPosition(x, y);
            Characters[x, y] = selectedCharacter;
            Characters[x, y].hasMoved = true;
        }

        BoardHighlighter.Instance.hideHighlights();

        //deselect the character
        selectedCharacter = null;
    }

    //Does not interact with character selection. this method shows which tile the mouse is over by drawing an X
    private void UpdateSelection()
    {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("BoardPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    //Spawns a character from the board's character array
    //index is the index in a character array, x is the x-coordinate, y is the y-coordinate on the board
    private void spawnCharacter(int index, int x, int y)
    {
        GameObject go = Instantiate(entities[index], getTileCenter(x,y), orientation) as GameObject;
        go.transform.SetParent(transform);
        Characters[x, y] = go.GetComponent<Character>();
        Characters[x, y].setPosition(x, y);
        activeCharacter.Add(go);
    }

    //quickly gets the center of a tile. x and z are the coordinates on the board where you want to find the center
    private Vector3 getTileCenter(int x, int z)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * z) + TILE_OFFSET;
        return origin;
    }

    //draws a board with the debugger
    private void drawBoard()
    {
        Vector3 widthLine = Vector3.right * boardSize;
        Vector3 heightLine = Vector3.forward * boardSize;

        for(int i = 0; i<= boardSize; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for(int j = 0; j<= boardSize; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        if(selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX, 
                           Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));

            Debug.DrawLine(Vector3.forward * (selectionY+1) + Vector3.right * selectionX,
                           Vector3.forward * selectionY + Vector3.right * (selectionX + 1));
        }
    }

    private double findDistance(int x1, int y1, int x2, int y2)
    {
        return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
    }

    public void endTurn()
    {
        isPlayer1Turn = !isPlayer1Turn;
        for(int i = 0; i< boardSize; i++)
        {
            for(int j = 0; j< boardSize; j++)
            {
                if (isPlayer1Turn && Characters[i, j] != null && Characters[i, j].isPlayer1)
                    Characters[i, j].hasMoved = false;
                else if (!isPlayer1Turn && Characters[i, j] != null && !Characters[i, j].isPlayer1)
                    Characters[i, j].hasMoved = false;
            }
        }
        player1Camera.enabled = !player1Camera.enabled;
        player2Camera.enabled = !player2Camera.enabled;
    }

}