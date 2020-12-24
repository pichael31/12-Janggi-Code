using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Vector2Int mBoardPosition = Vector2Int.zero;
    public Board mBoard;
    public RectTransform mRectTransform;
    public Piece mCurrentPiece;
    public Image outlineImageObject;
    public int playerEndZone;

    private Color32 outlineColor = new Color32(1, 1, 1, 255);
    private Color32 highlightedOutlineColor = new Color32(217, 224, 29, 255);

    [SerializeField] Sprite outlineImage;
    [SerializeField] Sprite highlightedOutlineImage;

    public void SetupCell(Vector2Int newBoardPosition, Board newBoard, int endZone, Color32 cellColor)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;
        mRectTransform = GetComponent<RectTransform>();
        
        foreach (Image childImage in GetComponentsInChildren<Image>())
        {
            if (childImage.tag == "Outline")
            {
                outlineImageObject = childImage;
            }
        }
        playerEndZone = endZone;
        GetComponent<Image>().color = cellColor;
        Unhighlight();
    }

    public void Highlight()
    {
        outlineImageObject.sprite = highlightedOutlineImage;
        outlineImageObject.color = highlightedOutlineColor;
    }
    public void Unhighlight()
    {
        outlineImageObject.sprite = outlineImage;
        outlineImageObject.color = outlineColor;
    }

    public void RemovePiece()
    {
        if (mCurrentPiece != null)
        {
            mCurrentPiece.Capture();
        }
    }
}
