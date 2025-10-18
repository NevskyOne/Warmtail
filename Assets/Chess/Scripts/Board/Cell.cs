using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image mOutlineImage;

    [HideInInspector]
    public Vector2 mBoardPosition = Vector2Int.zero;
    [HideInInspector]
    public Board mBoard = null;
    [HideInInspector]
    public RectTransform mRectTransform = null;

    [HideInInspector]
    public BasePiece mCurrentPiece = null;

    public void Setup(Vector2Int newBoardPos, Board newBoard)
    {
        mBoardPosition = newBoardPos;    
        mBoard = newBoard;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void RemovePiece()
    {
        if  (mCurrentPiece != null)
        {
            mCurrentPiece.kill();
        }
    }

}
