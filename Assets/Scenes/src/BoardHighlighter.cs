using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlighter : MonoBehaviour
{
    public static BoardHighlighter Instance { set; get; }

    public GameObject highlightPrefab;
    public GameObject enemyHighlightPrefab;

    private List<GameObject> highlights;
    internal object selectedCharacter;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject getHighlightObject()
    {
        GameObject go = null;

        if(go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    private GameObject getEnemyHighlightObject()
    {
        GameObject go = null;

        if (go == null)
        {
            go = Instantiate(enemyHighlightPrefab);
            highlights.Add(go);
        }

        return go;
    }

    public void highlightAllowedMoves(bool[,] moves)
    {
        for (int i = 0; i<BoardManager.boardSize; i++)
        {
            for(int j = 0; j<BoardManager.boardSize; j++)
            {
                if(moves[i,j] == true)
                {
                    GameObject go;
                    
                    //if the tile is an enemy, highlight it red
                    if (BoardManager.Characters[i, j] != null && BoardManager.Characters[i, j].isPlayer1 != BoardManager.selectedCharacter.isPlayer1)
                    {
                        go = getEnemyHighlightObject();
                        go.SetActive(true);
                        go.transform.position = new Vector3(i + 0.5f, .1f, j + 0.5f);
                    }
                    //if the tile is an ally, don't highlight it
                    else if (BoardManager.Characters[i, j] != null && BoardManager.Characters[i, j].isPlayer1 == BoardManager.selectedCharacter.isPlayer1) { }
                    //if the tile is empty, highlight it blue
                    else
                    {
                        go = getHighlightObject();
                        go.SetActive(true);
                        go.transform.position = new Vector3(i + 0.5f, .1f, j + 0.5f);
                    }
                }
            }
        }
    }

    public void hideHighlights()
    {
        foreach (GameObject go in highlights)
            Object.Destroy(go);
    }
}
