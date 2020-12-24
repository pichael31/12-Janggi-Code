using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Piece : EventTrigger
{
    public string pieceName;
    public int playerOwnerInt;

    public RectTransform mRectTransform;
    public Cell mCurrentCell;
    public Cell mTargetCell;
    public bool isDead;
    public List<Vector2Int> mPossibleMovement;
    protected PieceManager mPieceManager;
    public List<Cell> mHighlightedCells = new List<Cell>();

    public void SetupPiece(Color32 teamColor, PieceManager pieceManager, string newpieceName, bool dead, int newplayerOwnerInt, Sprite sprite, List<Vector2Int> possibleMovement)
    {
        mPossibleMovement = possibleMovement;
        playerOwnerInt = newplayerOwnerInt;
        GetComponent<Image>().sprite = sprite;
        isDead = dead;
        mPieceManager = pieceManager;
        GetComponentsInChildren<Image>()[1].color = teamColor;
        pieceName = newpieceName;
        mRectTransform = GetComponent<RectTransform>();
        mRectTransform.sizeDelta = new Vector2(100, 100);
    }

    public void SetTeam(Color32 teamColor, bool dead, int newplayerOwnerInt)
    {
        playerOwnerInt = newplayerOwnerInt;
        isDead = dead;
        GetComponentsInChildren<Image>()[1].color = teamColor;
    }

    public void Place(Cell newCell)
    {
        mCurrentCell = newCell;
        mCurrentCell.mCurrentPiece = this;
        transform.position = newCell.transform.position;
        if (playerOwnerInt == 1)
        {
            transform.rotation = new Quaternion(0, 0, 0, 1);
        }
        else
        {
            transform.rotation = new Quaternion(0, 0, 1, 0);
        }
    }

    private void CreateCellPath(int xDirection, int yDirection)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        currentX += xDirection;
        currentY += yDirection;

        if (currentX < mCurrentCell.mBoard.mAllCells.GetLength(0) && currentX >= 0 && currentY < mCurrentCell.mBoard.mAllCells.GetLength(1) && currentY >= 0)
        {
            if (mCurrentCell.mBoard.mAllCells[currentX, currentY].mCurrentPiece == null || mCurrentCell.mBoard.mAllCells[currentX, currentY].mCurrentPiece.playerOwnerInt != playerOwnerInt)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            }
        }
    }

    private void CheckAllCells()
    {
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                if (mCurrentCell.mBoard.mAllCells[x, y].mCurrentPiece == null)
                {
                    if (new List<int>{ 0, playerOwnerInt}.Contains(mCurrentCell.mBoard.mAllCells[x, y].playerEndZone)){
                        mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[x, y]);
                    }
                }
            }
        }
    }

    public void CheckPathing()
    {
        if (isDead)
        {
            CheckAllCells();
        }
        else
        {
            foreach (Vector2Int move in mPossibleMovement)
            {
                int x = move.x;
                int y = move.y;
                if (playerOwnerInt == 2)
                {
                    y = y * -1;
                }
                CreateCellPath(x, y);
            }
        }
    }

    protected void ShowCells()
    {
        foreach (Cell cell in mHighlightedCells)
        {
            cell.Highlight();
        }
    }

    protected void ClearCells()
    {
        foreach (Cell cell in mHighlightedCells)
        {
            cell.Unhighlight();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        if (!mPieceManager.CheckPiecesPlayersTurn(this))
        {
            return;
        }
        transform.SetAsLastSibling();

        CheckPathing();
        ShowCells();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        if (!mPieceManager.CheckPiecesPlayersTurn(this))
        {
            return;
        }
        transform.position += (Vector3)eventData.delta;
        foreach (Cell cell in mHighlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                mTargetCell = cell;
                break;
            }
            else
            {
                mTargetCell = null;
            }
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        Cell originalCell = mCurrentCell;
        bool originalIsDead = isDead;
        base.OnEndDrag(eventData);
        if (!mPieceManager.CheckPiecesPlayersTurn(this))
        {
            return;
        }
        if (TargetCellisMoveableTo())
        {
            Vector2Int actualCurrentCellVector2 = mTargetCell.mBoardPosition;
            MovePiece(true);
            ClearCells();
            mHighlightedCells.Clear();
            isDead = false;
            mPieceManager.SendDataToAI(new Vector2Int(originalCell.mBoardPosition.x, originalCell.mBoardPosition.y), originalIsDead, actualCurrentCellVector2, playerOwnerInt);
        }
        else
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            ClearCells();
            mHighlightedCells.Clear();
        }
    }

    private bool TargetCellisMoveableTo()
    {
        return (mHighlightedCells.Contains(mTargetCell));
    }

    public bool InEnemyEndZone()
    {
        return !(new List<int> { 0, playerOwnerInt }.Contains(mCurrentCell.playerEndZone));
    }

    public void Capture()
    {
        Piece capturedPiece = mTargetCell.mCurrentPiece;
        string previousName = capturedPiece.pieceName;
        if (capturedPiece.pieceName == "King")
        {
            mPieceManager.Win(playerOwnerInt);
        }
        mCurrentCell.mBoard.AddPieceToPlayerSideBoard(capturedPiece, GetComponentsInChildren<Image>()[1].color, true, playerOwnerInt);
        if (previousName == "Hu")
        {
            capturedPiece.SwitchHuAndJa(mCurrentCell.mBoard.mHuCells, true);
        }
        capturedPiece.mPieceManager.ResetPiecesPlayer(capturedPiece);
    }

    public void MovePiece(bool personMadeMove)
    {
        if (mTargetCell.mCurrentPiece != null)
        {
            Capture();
        }
        mCurrentCell.mCurrentPiece = null;
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;

        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;
        if (pieceName == "Ja" && InEnemyEndZone())
        {
            SwitchHuAndJa(mCurrentCell.mBoard.mHuCells, false);
        }
        if (pieceName == "King" && InEnemyEndZone())
        {
            mPieceManager.Win(playerOwnerInt);
        }
        if (personMadeMove)
        {
            isDead = false;
            mPieceManager.SwitchPlayerTurn();
        }
    }

    private void SwitchHuAndJa(Cell[,] arr, bool dead)
    {
        Cell originalCell = mCurrentCell;
        for (int x = 0; x < arr.GetLength(0); x++)
        {
            for (int y = 0; y < arr.GetLength(1); y++)
            {
                if (arr[x, y].mCurrentPiece == null)
                {
                    mTargetCell = arr[x, y];
                    MovePiece(false);
                    mTargetCell = null;
                    for (int x2 = 0; x2 < arr.GetLength(0); x2++)
                    {
                        for (int y2 = 0; y2 < arr.GetLength(1); y2++)
                        {
                            if (arr[x2, y2].mCurrentPiece != null && arr[x2, y2].mCurrentPiece.pieceName != pieceName)
                            {
                                Piece huPiece = arr[x2, y2].mCurrentPiece;
                                huPiece.SetTeam(GetComponentsInChildren<Image>()[1].color, dead, playerOwnerInt);
                                huPiece.Place(originalCell);
                                arr[x2, y2].mCurrentPiece = null;
                                playerOwnerInt = 0;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
