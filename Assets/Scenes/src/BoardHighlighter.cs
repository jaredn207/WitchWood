using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardHighlighter : MonoBehaviour
{
    public static BoardHighlighter Instance { set; get; }

    public GameObject highlightPrefab;
    private List<GameObject> highlights;

    private void Start()
    {
        Instance = this;
        highlights = new List<GameObject>();
    }

    private GameObject getHighlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if(go == null)
        {
            go = Instantiate(highlightPrefab);
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
                    GameObject go = getHighlightObject();
                    go.SetActive(true);
                    go.transform.position = new Vector3(i+0.5f, 0, j+0.5f);
                }
            }
        }
    }

    public void hideHighlights()
    {
        foreach (GameObject go in highlights)
            go.SetActive(false);
    }
}
