using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PieceManager : MonoBehaviour
{
    public int playerTurn = 1;
    [SerializeField] public Text player1TurnText;
    [SerializeField] public Text player2TurnText;
    [SerializeField] public Text playerWinnerText;

    [SerializeField] public List<Sprite> piecePictList;

    public Dictionary<string, List<int>> startingPieceDict = new Dictionary<string, List<int>>()
    {
        { "Sang", new List<int>(){0, 0}},
        { "King", new List<int>(){1, 0}},
        { "Jang", new List<int>(){2, 0}},
        { "Ja", new List<int>(){1, 1}}
    };

    public Dictionary<string, Sprite> pieceDict = new Dictionary<string, Sprite>();
    public void CreatePieceDict()
    {
        for (int spriteInt = 0; spriteInt < piecePictList.Count; spriteInt++)
        {
            pieceDict.Add(piecePictList[spriteInt].name, piecePictList[spriteInt]);
        }
    }

    public Dictionary<string, List<Vector2Int>> mPieceLibrary = new Dictionary<string, List<Vector2Int>>()
    {
        {"Hu", new List<Vector2Int>
            {
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, 0),
            }
        },
        {"Ja", new List<Vector2Int>
            {
                new Vector2Int(0, 1),
            }
        },
        {"Jang", new List<Vector2Int>
            {
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
            }
        },
        {"Sang", new List<Vector2Int>
            {
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
            }
        },
        {"King", new List<Vector2Int>
            {
                new Vector2Int(-1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 0),
            }
        }
    };

    private Color32 player1PieceColor = new Color32(29, 37, 255, 165);
    private Color32 player2PieceColor = new Color32(255, 94, 63, 165);
    [SerializeField] GameObject mPiecePrefab;

    private bool gameOver = false;

    public Dictionary<int, List<Piece>> playerPiecesDict = new Dictionary<int, List<Piece>>();

    [SerializeField] public AI aiPlayer;

    public void SetupPieceManager(Board board)
    {
        CreatePieceDict();
        playerPiecesDict[1] = CreatePieces(player1PieceColor, board, true);
        playerPiecesDict[2] = CreatePieces(player2PieceColor, board, false);
        PlacePieces(true, playerPiecesDict[1], board);
        PlacePieces(false, playerPiecesDict[2], board);
        player1TurnText.GetComponent<Outline>().effectColor = new Color32(79, 123, 159, 255);
        player1TurnText.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
        if (PlayerPrefs.GetString("Enemy") != "AI")
        {
            aiPlayer = null;
        }
    }

    private List<Piece> CreatePieces(Color32 pieceColor, Board board, bool isPlayerOne)
    {
        List<Piece> newPieces = new List<Piece>();
        foreach (string pieceName in startingPieceDict.Keys)
        {
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);
            Piece newPiece = newPieceObject.GetComponent<Piece>();
            int playerOwnerInt;
            if (isPlayerOne)
            {
                playerOwnerInt = 1;
            }
            else
            {
                playerOwnerInt = 2;
            }
            newPiece.SetupPiece(pieceColor, this, pieceName, false, playerOwnerInt, pieceDict[pieceName], mPieceLibrary[pieceName]);
            newPieces.Add(newPiece);
        }
        return newPieces;
    }

    private void PlacePieces(bool isPlayerOne, List<Piece> pieces, Board board)
    {
        foreach (Piece piece in pieces)
        {
            int x = startingPieceDict[piece.pieceName][0];
            int y = startingPieceDict[piece.pieceName][1];
            if (!isPlayerOne)
            {
                x = 2 - x;
                y = 3 - y;
            }
            piece.Place(board.mAllCells[x, y]);
        }
    }

    public void PlaceHus(Board board)
    {
        string pieceName = "Hu";
        int y = 0;
        for (int x = 0; x < 2; x++)
        {
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);
            Piece newPiece = newPieceObject.GetComponent<Piece>();
            newPiece.SetupPiece(new Color32(0, 0, 0, 0), this, pieceName, false, 0, pieceDict[pieceName], mPieceLibrary[pieceName]);
            newPiece.Place(board.mHuCells[x, y]);
        }
    }

    public void ResetPiecesPlayer(Piece piece)
    {
        foreach (int key in playerPiecesDict.Keys)
        {
            if (playerPiecesDict[key].Contains(piece) && key != piece.playerOwnerInt)
            {
                playerPiecesDict[key].Remove(piece);
            }
            else if (!playerPiecesDict[key].Contains(piece) && key == piece.playerOwnerInt)
            {
                playerPiecesDict[key].Add(piece);
            }
        }
    }

    public bool CheckPiecesPlayersTurn(Piece piece)
    {
        return (piece.playerOwnerInt == playerTurn);
    }

    public void SwitchPlayerTurn()
    {
        if (playerTurn != 0)
        {
            playerTurn = (playerTurn % 2) + 1;
            SetTextColors();
        }
    }

    public void SetTextColors()
    {
        if (playerTurn == 1)
        {
            player1TurnText.GetComponent<Text>().color = new Color32(50, 50, 50, 255);
            player1TurnText.GetComponent<Outline>().effectColor = new Color32(79, 123, 159, 255);
            player1TurnText.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            player2TurnText.GetComponent<Text>().color = new Color32(50, 50, 50, 64);
            player2TurnText.GetComponent<Outline>().effectColor = new Color32(50, 50, 50, 255);
            player2TurnText.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
        }
        else
        {
            player2TurnText.GetComponent<Text>().color = new Color32(50, 50, 50, 255);
            player2TurnText.GetComponent<Outline>().effectColor = new Color32(210, 94, 63, 255);
            player2TurnText.GetComponent<Outline>().effectDistance = new Vector2(5, 5);
            player1TurnText.GetComponent<Text>().color = new Color32(50, 50, 50, 64);
            player1TurnText.GetComponent<Outline>().effectColor = new Color32(50, 50, 50, 255);
            player1TurnText.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
        }
    }

    public void Win(int playerWinner)
    {
        gameOver = true;
        playerTurn = 0;
        playerWinnerText.GetComponent<Text>().text = "Player " + playerWinner.ToString() + " Wins!";
        playerWinnerText.rectTransform.anchoredPosition = new Vector2(0, 0);
        if (playerWinner == 1)
        {
            playerWinnerText.color = player1PieceColor;
        }
        else
        {
            playerWinnerText.color = player2PieceColor;
        }

    }

    public void SendDataToAI(Vector2Int originalPosition, bool isDead, Vector2Int currentPosition, int playerInt)
    {
        if (aiPlayer != null)
        {
            if (!gameOver)
            {
                //aiPlayer.ResetBoardWithNewMoves(originalPosition, isDead, currentPosition, playerInt);
                //aiPlayer.ResetNewBoard();
                aiPlayer.MakeBestMove();
            }
        }
    }
}
