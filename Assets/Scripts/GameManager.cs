using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] Board gameBoard;
    [SerializeField] PieceManager pieceManager;
    [SerializeField] AI aiPlayer;
    [SerializeField] public List<Sprite> piecePictList;

    public void Start()
    {
        gameBoard.Create();
        pieceManager.SetupPieceManager(gameBoard);
        gameBoard.CreatePlayerSideBoard();
        gameBoard.CreateHuBoard();
        pieceManager.PlaceHus(gameBoard);

        if (PlayerPrefs.GetString("Enemy") != "AI")
        {
            aiPlayer = null;
        }
    }
}
