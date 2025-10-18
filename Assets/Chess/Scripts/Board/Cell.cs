using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image mOutlineImage;

    public Vector2 mBoardPosition = Vector2Int.zero;
    public Board mBoard = null;
    public RectTransform mRectTransform = null;
    
    public void Setup(Vector2Int newBoardPos, Board newBoard)
    {
        mBoardPosition = newBoardPos;    
        mBoard = newBoard;
        mRectTransform = GetComponent<RectTransform>();
    }

}
