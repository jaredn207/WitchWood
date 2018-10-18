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
                    if(BoardManager.Characters[i,j] != null && BoardManager.Characters[i,j].isAlly == false)
                    {
                        go = getEnemyHighlightObject();
                    }
                    else
                    {
                        go = getHighlightObject();
                    }
                    go.SetActive(true);
                    go.transform.position = new Vector3(i + 0.5f, 0, j + 0.5f);
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
