using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] Board gameBoard;
    [SerializeField] PieceManager pieceManager;
    public VirtualBoard virtualBoard;
    public int aiPlayerInt;
    public int enemyInt;
    public int foresightDepth = 10;

    public void SetVirtualBoard()
    {
        //virtualBoard = new VirtualBoard();
        //virtualBoard.SetVirtualBoard(gameBoard);
        //virtualBoard.CreateRecursiveBoardDict();

        //foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp in virtualBoard.recursiveBoardDict)
        //{
        //    kvp.Value.CreateRecursiveBoardDict();
        //    foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp2 in kvp.Value.recursiveBoardDict)
        //    {
        //        kvp2.Value.CreateRecursiveBoardDict();
        //    }
        //}
    }

    public Tuple<VirtualPiece, Vector2Int> GetBestMove()
    {
        virtualBoard = new VirtualBoard();
        virtualBoard.SetVirtualBoard(gameBoard);
        return virtualBoard.DoEverythingBecauseNothingWorks();
    }

    public Tuple<VirtualPiece, Vector2Int> GetBestMove2()
    {
        virtualBoard = new VirtualBoard();
        virtualBoard.SetVirtualBoard(gameBoard);
        virtualBoard.CreateRecursiveBoardDict();
        foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp in virtualBoard.recursiveBoardDict)
        {
            kvp.Value.CreateRecursiveBoardDict();
            foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp2 in kvp.Value.recursiveBoardDict)
            {
                kvp2.Value.CreateRecursiveBoardDict();
                foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp3 in kvp2.Value.recursiveBoardDict)
                {
                    kvp3.Value.CreateRecursiveBoardDict();
                    foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp4 in kvp3.Value.recursiveBoardDict)
                    {
                        kvp4.Value.CreateRecursiveBoardDict();
                    }
                    kvp3.Value.GetBestMove();
                }
                kvp2.Value.GetBestMove();
            }
            kvp.Value.GetBestMove();
        }
        virtualBoard.GetBestMove();
        return virtualBoard.bestMove;
    }

    public Vector2Int GetDeadPosition(int playerInt, string pieceName)
    {
        for (int x = 0; x < gameBoard.sideBoardDict[playerInt].GetLength(0); x++)
        {
            for (int y = 0; y < gameBoard.sideBoardDict[playerInt].GetLength(1); y++)
            {
                if (gameBoard.sideBoardDict[playerInt][x, y].mCurrentPiece != null)
                {
                    if (gameBoard.sideBoardDict[playerInt][x, y].mCurrentPiece.pieceName == pieceName)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
        }
        return new Vector2Int(0, 0);
    }

    public void MakeBestMove()
    {
        Tuple<VirtualPiece, Vector2Int> bestMove = GetBestMove();
        Vector2Int position = bestMove.Item1.cCurrentPosition;
        Vector2Int targetCellPosition = bestMove.Item2;

        bool isDead = bestMove.Item1.cIsDead;
        int playerInt = bestMove.Item1.cPlayerInt;
        Cell[,] arr = null;
        if (isDead)
        {
            arr = gameBoard.sideBoardDict[playerInt];
            position = GetDeadPosition(playerInt, bestMove.Item1.cPieceName);
        }
        else
        {
            arr = gameBoard.mAllCells;
        }
        Cell targetCell = gameBoard.mAllCells[targetCellPosition.x, targetCellPosition.y];
        Piece pieceToMove = arr[position.x, position.y].mCurrentPiece;
        pieceToMove.mTargetCell = targetCell;
        pieceToMove.MovePiece(true);

        //virtualBoard = virtualBoard.recursiveBoardDict[bestMove];
        //foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp in virtualBoard.recursiveBoardDict)
        //{
        //    foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp2 in kvp.Value.recursiveBoardDict)
        //    {
        //        kvp2.Value.CreateRecursiveBoardDict();
        //    }
        //}
    }

    public void MakeBestMove2()
    {
        Tuple<VirtualPiece, Vector2Int> bestMove = GetBestMove2();
        Vector2Int position = bestMove.Item1.cCurrentPosition;
        Vector2Int targetCellPosition = bestMove.Item2;

        bool isDead = bestMove.Item1.cIsDead;
        int playerInt = bestMove.Item1.cPlayerInt;
        Cell[,] arr = null;
        if (isDead)
        {
            arr = gameBoard.sideBoardDict[playerInt];
            position = GetDeadPosition(playerInt, bestMove.Item1.cPieceName);
        }
        else
        {
            arr = gameBoard.mAllCells;
        }
        Cell targetCell = gameBoard.mAllCells[targetCellPosition.x, targetCellPosition.y];
        Piece pieceToMove = arr[position.x, position.y].mCurrentPiece;
        pieceToMove.mTargetCell = targetCell;
        pieceToMove.MovePiece(true);

        //virtualBoard = virtualBoard.recursiveBoardDict[bestMove];
        //foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp in virtualBoard.recursiveBoardDict)
        //{
        //    foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp2 in kvp.Value.recursiveBoardDict)
        //    {
        //        kvp2.Value.CreateRecursiveBoardDict();
        //    }
        //}
    }

    public void ResetNewBoard()
    {
        virtualBoard = new VirtualBoard();
        virtualBoard.SetVirtualBoard(gameBoard);
        virtualBoard.CreateRecursiveBoardDict();
        foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp in virtualBoard.recursiveBoardDict)
        {
            kvp.Value.CreateRecursiveBoardDict();
            foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp2 in kvp.Value.recursiveBoardDict)
            {
                kvp2.Value.CreateRecursiveBoardDict();
            }
        }
        foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp in virtualBoard.recursiveBoardDict)
        {
            foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp2 in kvp.Value.recursiveBoardDict)
            {
                kvp2.Value.GetUtilsFromFutureMove();
            }
            kvp.Value.GetUtilsFromFutureMove();
        }
    }
}

public class VirtualPiece
{
    public string cPieceName;
    public int cPlayerInt;
    public Vector2Int cCurrentPosition;
    public List<Vector2Int> cPossibleMovement;
    public VirtualBoard cVirtualBoard;
    public bool cIsDead;
    public void SetVirtualPiece(string pieceName, int playerInt, List<Vector2Int> possibleMovement, bool isDead, Vector2Int currentPosition, VirtualBoard virtualBoard)
    {
        cPieceName = pieceName;
        cPlayerInt = playerInt;
        cPossibleMovement = possibleMovement;
        cIsDead = isDead;
        cCurrentPosition = currentPosition;
        cVirtualBoard = virtualBoard;
    }

    public List<Vector2Int> CheckPathing()
    {
        List<Vector2Int> possibleCells = new List<Vector2Int>();
        if (cIsDead)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 2; x++)
                {
                    if (cVirtualBoard.mAllCells[x, y] == null)
                    {
                        if (y != cVirtualBoard.mPlayerIntToEnemyEndZone[cPlayerInt])
                        {
                            possibleCells.Add(new Vector2Int(x, y));
                        }
                    }
                }
            }
        }
        else
        {
            foreach (Vector2Int move in cPossibleMovement)
            {
                int xDirection = move.x;
                int yDirection = move.y;
                if (cPlayerInt == 2)
                {
                    yDirection = yDirection * -1;
                }
                int currentX = cCurrentPosition.x;
                int currentY = cCurrentPosition.y;

                currentX += xDirection;
                currentY += yDirection;

                if (currentX < cVirtualBoard.mAllCells.GetLength(0) && currentX >= 0 && currentY < cVirtualBoard.mAllCells.GetLength(1) && currentY >= 0)
                {
                    if (cVirtualBoard.mAllCells[currentX, currentY] == null || cVirtualBoard.mAllCells[currentX, currentY].cPlayerInt != cPlayerInt)
                    {
                        possibleCells.Add(new Vector2Int(currentX, currentY));
                    }
                }
            }
        }
        return possibleCells;
    }

    private void Capture(int x, int y)
    {
        VirtualPiece capturedPiece = cVirtualBoard.mAllCells[x, y];
        string previousName = capturedPiece.cPieceName;
        capturedPiece.cPlayerInt = cPlayerInt;
        capturedPiece.cIsDead = true;
        cVirtualBoard.AddPieceToPlayerSideBoard(capturedPiece, cPlayerInt);
        if (previousName == "Hu")
        {
            capturedPiece.SwitchHuAndJa();
        }
        cVirtualBoard.ResetPiecesPlayer(capturedPiece);
    }

    public void MovePiece(int x, int y)
    {
        if (cVirtualBoard.mAllCells[x, y] != null)
        {
            Capture(x, y);
        }

        if (cPieceName == "Ja" && InEnemyEndZone(y))
        {
            SwitchHuAndJa();
        }
        if (!cIsDead)
        {
            cVirtualBoard.mAllCells[cCurrentPosition.x, cCurrentPosition.y] = null;
        }
        else
        {
            cVirtualBoard.sideBoardDict[cPlayerInt].Remove(this);
        }
        cIsDead = false;
        cCurrentPosition = new Vector2Int(x, y);
        cVirtualBoard.mAllCells[cCurrentPosition.x, cCurrentPosition.y] = this;
    }


    private void SwitchHuAndJa()
    {
        if (cPieceName == "Hu")
        {
            cPieceName = "Ja";
            cPossibleMovement = new List<Vector2Int> { new Vector2Int(0, 1) };
        }
        else if (cPieceName == "Ja")
        {
            cPieceName = "Hu";
            cPossibleMovement = new List<Vector2Int>
            {
                new Vector2Int(0, -1),
                new Vector2Int(-1, 0),
                new Vector2Int(-1, 1),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1),
                new Vector2Int(1, 0),
            };
        }
    }

    public bool InEnemyEndZone(int y)
    {
        return (y == cVirtualBoard.mPlayerIntToEnemyEndZone[cPlayerInt]);
    }

    private int JaEval(int x, int y, bool isDead)
    {
        if (isDead)
        {
            return 1;
        }
        return (y ^ 2) + ((y % 2) * 5);
    }

    private int JangEval(int x, int y, bool isDead)
    {
        if (isDead)
        {
            return 5;
        }
        return 12 - (Mathf.Abs(y - 2) * 5);
    }

    private int SangEval(int x, int y, bool isDead)
    {
        if (isDead)
        {
            return 5;
        }
        return 12 - (Mathf.Abs(y - 2) * 3);
    }

    private int KingEval(int x, int y, bool isDead)
    {
        if (y == 3)
        {
            return 1000;
        }
        return 100 + ((y + x) * 5);
    }

    private int HuEval(int x, int y, bool isDead)
    {
        return 15 - (Mathf.Abs(y - 2) * 5);
    }

    public int EvalPiece()
    {
        int x = cCurrentPosition.x;
        int y = cCurrentPosition.y;
        if (cPlayerInt == 2)
        {
            x = 2 - x;
            y = 3 - y;
        }
        if (cPieceName == "Ja")
        {
            return JaEval(x, y, cIsDead);
        }
        if (cPieceName == "Jang")
        {
            return JangEval(x, y, cIsDead);
        }
        if (cPieceName == "Sang")
        {
            return SangEval(x, y, cIsDead);
        }
        if (cPieceName == "King")
        {
            return KingEval(x, y, cIsDead);
        }
        if (cPieceName == "Hu")
        {
            return HuEval(x, y, cIsDead);
        }
        return 0;
    }

    public VirtualPiece ClonePiece(VirtualBoard newVirtualBoard)
    {
        VirtualPiece newPiece = new VirtualPiece();
        newPiece.SetVirtualPiece(cPieceName, cPlayerInt, cPossibleMovement, cIsDead, cCurrentPosition, newVirtualBoard);
        return newPiece;
    }
}

public class VirtualBoard
{
    public int player1Utils;
    public VirtualPiece[,] mAllCells = new VirtualPiece[3,4];

    public Dictionary<int, List<VirtualPiece>> sideBoardDict = new Dictionary<int, List<VirtualPiece>>
    {
        {1, new List<VirtualPiece>()},
        {2, new List<VirtualPiece>()}
    };

    public Dictionary<int, int> mPlayerIntToEnemyEndZone = new Dictionary<int, int>
    {
        {2,  0},
        {1,  3},
    };

    public Dictionary<int, List<VirtualPiece>> playerPiecesDict = new Dictionary<int, List<VirtualPiece>>
    {
        {1, new List<VirtualPiece>() },
        {2, new List<VirtualPiece>() }
    };

    public Dictionary<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> recursiveBoardDict = new Dictionary<Tuple<VirtualPiece, Vector2Int>, VirtualBoard>();

    public int playerTurn = 2;

    public bool aiPlayerWinning = false;
    public bool aiPlayerLosing = false;

    public Tuple<VirtualPiece, Vector2Int> bestMove;

    public void SetVirtualBoard(Board board)
    {
        for (int x = 0; x < board.mAllCells.GetLength(0); x++)
        {
            for (int y = 0; y < board.mAllCells.GetLength(1); y++)
            {
                if (board.mAllCells[x, y].mCurrentPiece != null)
                {
                    Piece piece = board.mAllCells[x, y].mCurrentPiece;
                    VirtualPiece newPiece = new VirtualPiece();
                    newPiece.SetVirtualPiece(piece.pieceName, piece.playerOwnerInt, piece.mPossibleMovement, piece.isDead, new Vector2Int(x, y), this);
                    mAllCells[x, y] = newPiece;
                    playerPiecesDict[piece.playerOwnerInt].Add(newPiece);
                }
            }
        }
        foreach (KeyValuePair<int, Cell[,]> kvp in board.sideBoardDict)
        {
            for (int x = 0; x < kvp.Value.GetLength(0); x++)
            {
                for (int y = 0; y < kvp.Value.GetLength(1); y++)
                {
                    if (kvp.Value[x, y].mCurrentPiece != null)
                    {
                        Piece piece = kvp.Value[x, y].mCurrentPiece;
                        VirtualPiece newPiece = new VirtualPiece();
                        newPiece.SetVirtualPiece(piece.pieceName, piece.playerOwnerInt, piece.mPossibleMovement, piece.isDead, new Vector2Int(x, y), this);
                        sideBoardDict[kvp.Key].Add(newPiece);
                        playerPiecesDict[piece.playerOwnerInt].Add(newPiece);
                    }
                }
            }
        }
        player1Utils = EvaluateVirtualBoard();
    }

    public void AddPieceToPlayerSideBoard(VirtualPiece piece, int newplayerOwnerInt)
    {
        sideBoardDict[newplayerOwnerInt].Add(piece);
    }

    public VirtualBoard CloneVirtualBoard()
    {
        VirtualBoard newVirtualBoard = new VirtualBoard();

        for (int x = 0; x < mAllCells.GetLength(0); x++)
        {
            for (int y = 0; y < mAllCells.GetLength(1); y++)
            {
                if (mAllCells[x, y] != null)
                {
                    VirtualPiece newPiece = mAllCells[x, y].ClonePiece(newVirtualBoard);
                    newVirtualBoard.mAllCells[x, y] = newPiece;
                    newVirtualBoard.playerPiecesDict[newPiece.cPlayerInt].Add(newPiece);
                }
            }
        }

        foreach (KeyValuePair<int, List<VirtualPiece>> kvp in sideBoardDict)
        {
            for (int x = 0; x < kvp.Value.Count; x++)
            {
                VirtualPiece newPiece = kvp.Value[x].ClonePiece(newVirtualBoard);
                newVirtualBoard.sideBoardDict[kvp.Key].Add(newPiece);
            }
        }

        newVirtualBoard.playerTurn = SwitchPlayerTurn();
        return newVirtualBoard;
    }

    public void ResetPiecesPlayer(VirtualPiece piece)
    {
        foreach (KeyValuePair<int, List<VirtualPiece>> kvp in playerPiecesDict)
        {
            if (kvp.Value.Contains(piece) && kvp.Key != piece.cPlayerInt)
            {
                kvp.Value.Remove(piece);
            }
            else if (!kvp.Value.Contains(piece) && kvp.Key == piece.cPlayerInt)
            {
                kvp.Value.Add(piece);
            }
        }
    }

    public int SwitchPlayerTurn()
    {
        return (playerTurn % 2) + 1;
    }

    private int EvaluateVirtualBoard()
    {
        int utils = 0;
        foreach (KeyValuePair<int, List<VirtualPiece>> kvp in playerPiecesDict)
        {
            foreach (VirtualPiece piece in kvp.Value)
            {
                int pieceValue = piece.EvalPiece();
                if (kvp.Key == 1)
                {
                    utils = utils + pieceValue;
                }
                else
                {
                    utils = utils - pieceValue;
                }
            }
        }
        if (utils < -100)
        {
            aiPlayerWinning = true;
        }
        if (utils > 100)
        {
            aiPlayerLosing = true;
        }

        return utils;
    }

    public VirtualPiece FindPiece(Vector2Int position, string pieceName, bool isDead, int playerInt)
    {
        if (isDead)
        {
            foreach (VirtualPiece piece in sideBoardDict[playerInt])
            {
                if (piece.cPieceName == pieceName)
                {
                    return piece;
                }
            }
            return null;
        }
        else
        {
            return mAllCells[position.x, position.y];
        }
    }

    public void CreateRecursiveBoardDict()
    {
        foreach (VirtualPiece piece in playerPiecesDict[playerTurn])
        {
            List<Vector2Int> possileCells = piece.CheckPathing();
            Vector2Int currentPosition = piece.cCurrentPosition;
            bool isDead = piece.cIsDead;
            foreach (Vector2Int movement in possileCells)
            {
                VirtualBoard newVirtualBoard = CloneVirtualBoard();
                VirtualPiece newPiece = newVirtualBoard.FindPiece(piece.cCurrentPosition, piece.cPieceName, isDead, piece.cPlayerInt);
                newPiece.MovePiece(movement.x, movement.y);
                newVirtualBoard.player1Utils = newVirtualBoard.EvaluateVirtualBoard();
                recursiveBoardDict.Add(new Tuple<VirtualPiece, Vector2Int>(piece, movement), newVirtualBoard);
            }
        }

        //    foreach (VirtualPiece piece in playerPiecesDict[playerTurn])
        //{
        //    if (piece.cPlayerInt == playerTurn)
        //    {
        //        List<Vector2Int> possileCells = piece.CheckPathing();
        //        Vector2Int currentPosition = piece.cCurrentPosition;
        //        bool isDead = piece.cIsDead;
        //        foreach (Vector2Int movement in possileCells)
        //        {
        //            VirtualBoard newVirtualBoard = CloneVirtualBoard();

        //            VirtualPiece newPiece = newVirtualBoard.FindPiece(piece.cCurrentPosition, piece.cPieceName, isDead, piece.cPlayerInt);

        //            newPiece.MovePiece(movement.x, movement.y);
        //            newVirtualBoard.player1Utils = newVirtualBoard.EvaluateVirtualBoard();
        //            recursiveBoardDict.Add(new Tuple<VirtualPiece, Vector2Int>(piece, movement), newVirtualBoard);
        //        }
        //    }
        //}
    }

    public Tuple<VirtualPiece, Vector2Int> DoEverythingBecauseNothingWorks()
    {
        int maxUtils = 1500;
        VirtualPiece bestMovePiece = null;
        Vector2Int bestMovePosition = new Vector2Int(0, 0);
        foreach (VirtualPiece piece in playerPiecesDict[playerTurn])
        {
            List<Vector2Int> possileCells = piece.CheckPathing();
            Vector2Int currentPosition = piece.cCurrentPosition;
            bool isDead = piece.cIsDead;
            foreach (Vector2Int movement in possileCells)
            {
                VirtualBoard newVirtualBoard = CloneVirtualBoard();
                VirtualPiece newPiece = newVirtualBoard.FindPiece(piece.cCurrentPosition, piece.cPieceName, isDead, piece.cPlayerInt);

                newPiece.MovePiece(movement.x, movement.y);
                newVirtualBoard.player1Utils = newVirtualBoard.EvaluateVirtualBoard();
                if (newVirtualBoard.aiPlayerWinning)
                {
                    maxUtils = newVirtualBoard.player1Utils;
                    bestMovePiece = piece;
                    bestMovePosition = movement;
                    return new Tuple<VirtualPiece, Vector2Int>(bestMovePiece, bestMovePosition);
                }
                int maxUtils2 = -1500;
                VirtualPiece bestMovePiece2 = null;
                Vector2Int bestMovePosition2 = new Vector2Int(0, 0);
                foreach (VirtualPiece piece2 in newVirtualBoard.playerPiecesDict[newVirtualBoard.playerTurn])
                {
                    List<Vector2Int> possileCells2 = piece2.CheckPathing();
                    Vector2Int currentPosition2 = piece2.cCurrentPosition;
                    bool isDead2 = piece2.cIsDead;
                    foreach (Vector2Int movement2 in possileCells2)
                    {
                        VirtualBoard newVirtualBoard2 = newVirtualBoard.CloneVirtualBoard();
                        VirtualPiece newPiece2 = newVirtualBoard2.FindPiece(piece2.cCurrentPosition, piece2.cPieceName, isDead2, piece2.cPlayerInt);
                        newPiece2.MovePiece(movement2.x, movement2.y);
                        newVirtualBoard2.player1Utils = newVirtualBoard2.EvaluateVirtualBoard();

                        if (newVirtualBoard2.player1Utils > maxUtils2)
                        {
                            maxUtils2 = newVirtualBoard2.player1Utils;
                            newVirtualBoard.player1Utils = newVirtualBoard2.player1Utils;
                            bestMovePiece2 = newPiece2;
                            bestMovePosition2 = movement2;
                        }
                    }
                }

                if (newVirtualBoard.player1Utils < maxUtils)
                {
                    maxUtils = newVirtualBoard.player1Utils;
                    bestMovePiece = piece;
                    bestMovePosition = movement;
                }
            }
        }
        return new Tuple<VirtualPiece, Vector2Int>(bestMovePiece, bestMovePosition);
    }

    public void GetBestMove()
    {
        int maxUtils = 1500;
        if (playerTurn == 1)
        {
            maxUtils = -1500;
        }

        VirtualPiece bestMovePiece = null;
        Vector2Int bestMovePosition = new Vector2Int(0, 0);

        foreach (KeyValuePair<Tuple<VirtualPiece, Vector2Int>, VirtualBoard> kvp in recursiveBoardDict)
        {
            if (playerTurn == 2)
            {
                if (kvp.Value.aiPlayerWinning)
                {
                    maxUtils = kvp.Value.player1Utils;
                    bestMovePiece = kvp.Key.Item1;
                    bestMovePosition = kvp.Key.Item2;
                    aiPlayerWinning = true;
                    bestMove = kvp.Key;
                    return;
                }
            }
            if ((playerTurn == 1 && kvp.Value.player1Utils > maxUtils) || (playerTurn == 2 && kvp.Value.player1Utils < maxUtils))
            {
                maxUtils = kvp.Value.player1Utils;
                player1Utils = kvp.Value.player1Utils;
                bestMovePiece = kvp.Key.Item1;
                bestMovePosition = kvp.Key.Item2;
            }
        }
        bestMove = new Tuple<VirtualPiece, Vector2Int>(bestMovePiece, bestMovePosition);
    }

    public void GetUtilsFromFutureMove()
    {
        foreach (KeyValuePair<Tuple<VirtualPiece,Vector2Int>, VirtualBoard> kvp in recursiveBoardDict)
        {
            if (kvp.Value.aiPlayerWinning)
            {
                bestMove = kvp.Key;
                player1Utils = kvp.Value.player1Utils;
                return;
            }
            if (kvp.Value.aiPlayerLosing)
            {
                continue;
            }
            if (bestMove == null || (playerTurn == 1 && kvp.Value.player1Utils > player1Utils) || (playerTurn == 2 && kvp.Value.player1Utils < player1Utils))
            {
                bestMove = kvp.Key;
                player1Utils = kvp.Value.player1Utils;
            }
        }
    }
}
