using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject mCellPrefab;
    public GameObject mPiecePrefab;

    public Color32 player1BoardColor = new Color32(79, 123, 159, 255);
    public Color32 player2BoardColor = new Color32(210, 94, 63, 255);
    public Color32 blankBoardColor = new Color32(186, 177, 150, 255);

    public Cell[,] mAllCells = new Cell[3, 4];
    public Dictionary<int, Cell[,]> sideBoardDict = new Dictionary<int, Cell[,]>
    {
        {1, new Cell[2,3]},
        {2, new Cell[2,3]}
    };
    public Cell[,] mHuCells = new Cell[3, 1];

    public void Create()
    {
        Vector2Int start = new Vector2Int(-200, -200);

        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                GameObject newCell = Instantiate(mCellPrefab, transform);
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 200), (y * 200)) + start;

                mAllCells[x, y] = newCell.GetComponent<Cell>();
                int endZone;
                Color32 cellColor;
                if (y == 0)
                {
                    cellColor = player1BoardColor;
                    endZone = 1;
                }
                else if (y == 3)
                {
                    cellColor = player2BoardColor;
                    endZone = 2;
                }
                else
                {
                    cellColor = blankBoardColor;
                    endZone = 0;
                }
                mAllCells[x, y].SetupCell(new Vector2Int(x, y), this, endZone, cellColor);
            }
        }
    }
    public void CreatePlayerSideBoard()
    {
        foreach (int playerInt in sideBoardDict.Keys)
        {
            Vector2Int start = new Vector2Int(-800, -100);
            int shift = 200;
            if (playerInt == 2)
            {
                start = new Vector2Int(800, 300);
                shift = shift * -1;
            }
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    GameObject newCell = Instantiate(mCellPrefab, transform);
                    RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                    rectTransform.anchoredPosition = new Vector2((x * shift), (y * shift)) + start;
                    sideBoardDict[playerInt][x, y] = newCell.GetComponent<Cell>();
                    Color32 cellColor;
                    if (playerInt == 1)
                    {
                        cellColor = player1BoardColor;
                    }
                    else
                    {
                        cellColor = player2BoardColor;
                    }
                    sideBoardDict[playerInt][x, y].SetupCell(new Vector2Int(x, y), this, 0, cellColor);
                }
            }
        }
    }

    public void CreateHuBoard()
    {
        Vector2Int start = new Vector2Int(-2000, 0);
        int shift = 200;
        int y = 0;
        for (int x = 0; x < 3; x++)
        {
            GameObject newCell = Instantiate(mCellPrefab, transform);
            RectTransform rectTransform = newCell.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2((x * shift), (y * shift)) + start;
            mHuCells[x, y] = newCell.GetComponent<Cell>();
            Color32 cellColor = blankBoardColor;
            mHuCells[x, y].SetupCell(new Vector2Int(x, y), this, 0, cellColor);
        }
    }

    public void AddPieceToPlayerSideBoard(Piece piece, Color32 teamColor, bool dead, int newplayerOwnerInt)
    {
        for (int x = 0; x < 2; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (sideBoardDict[newplayerOwnerInt][x, y].mCurrentPiece == null)
                {
                    sideBoardDict[newplayerOwnerInt][x, y].mCurrentPiece = piece;
                    piece.SetTeam(teamColor, dead, newplayerOwnerInt);
                    piece.Place(sideBoardDict[newplayerOwnerInt][x, y]);
                    return;
                }
            }
        }
    }
}
