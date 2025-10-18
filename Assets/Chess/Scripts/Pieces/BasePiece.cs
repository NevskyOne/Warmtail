using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector]
    public Color mColor = Color.clear;
    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;
    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;
    
    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();
    public virtual void Setup(Color newTeamColor, Color newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }
    
   

    public void Place(Cell newCell)
    {
        mCurrentCell = newCell;
        mOriginalCell = newCell;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }
    
    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        // Target position
        int currentX = (int)mCurrentCell.mBoardPosition.x;
        int currentY = (int)mCurrentCell.mBoardPosition.y;
        // Check each cell
        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            
            currentY += yDirection;

            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }
    protected virtual void CheckPathing()
    {
        // Horizontal
        CreateCellPath (1, 0, mMovement.x); 
        CreateCellPath(-1, 0, mMovement.x);
     // Vertical
        CreateCellPath(0, 1, mMovement.y); 
        CreateCellPath(0, -1, mMovement.y);
     // Upper diagonal
        CreateCellPath(1, 1, mMovement.z); 
        CreateCellPath(-1, 1, mMovement.z);
     // Lower diagonal
        CreateCellPath(-1, -1, mMovement.z); 
        CreateCellPath(1, -1, mMovement.z);
    }

    protected void ShowCells()
    {
        {
            foreach (Cell cell in mHighlightedCells)
                cell.mOutlineImage.enabled = true;
        }
    }
    protected void ClearCells()
    {
        {
            foreach (Cell cell in mHighlightedCells)
                cell.mOutlineImage.enabled = false;

            mHighlightedCells.Clear();
        }
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        
        base.OnBeginDrag(eventData);
// Test for cells
        CheckPathing();
// Show valid cells
         ShowCells();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        transform.position += (Vector3)eventData.delta;
    }

        public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        ClearCells();
    }

}