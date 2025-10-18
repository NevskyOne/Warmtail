using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public Image BackgroudImage;

    public Vector2 BoardPosition = Vector2Int.zero;
    public Board Board = null;
    public RectTransform RectTransform = null;
    
    public void Setup(Vector2Int newBoardPos, Board newBoard)
    {
        BoardPosition = newBoardPos;    
        Board = newBoard;
        RectTransform = GetComponent<RectTransform>();
    }

}
