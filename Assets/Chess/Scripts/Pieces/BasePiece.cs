using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine;
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
    protected Cell mTargetCell = null;
    protected List<Cell> mHighlighedCells = new List<Cell>();

    public bool mIsFirstMove = true;

    public virtual void Setup(Color newTeamColor, Color newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;

        mColor = newTeamColor;
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();
    }

    public void Reset()
    {
        kill_piece();
        Place(mOriginalCell);
    }
    public virtual void kill_piece()
    {
        mCurrentCell.mCurrentPiece = null;
        gameObject.SetActive(false);
    }
    public virtual void Move()
    {
        mTargetCell.RemovePiece();
        mCurrentCell.mCurrentPiece = null;
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;
        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;
    }

    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        int currentX = (int)mCurrentCell.mBoardPosition.x;
        int currentY = (int)mCurrentCell.mBoardPosition.y;

        for (int i = 1; i <= movement; i++)
        {
            currentX += xDirection;
            currentY += yDirection;

            if (currentX < 0 || currentX >= mCurrentCell.mBoard.mAllCells.GetLength(0) ||
                currentY < 0 || currentY >= mCurrentCell.mBoard.mAllCells.GetLength(1))
            {
                break;
            }

            mHighlighedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
        }
    }

    public virtual void CheckPathing()
    {
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);

        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);

        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);

        CreateCellPath(-1, -1, mMovement.z);
        CreateCellPath(1, -1, mMovement.z);
    }
    protected void ShowCells()
    {
        foreach(Cell cell in mHighlighedCells)
            cell.mOutlineImage.enabled = false;
    }

    protected void ClearCells()
    {
        foreach (Cell cell in mHighlighedCells)
            cell.mOutlineImage.enabled = true;

        mHighlighedCells.Clear();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        CheckPathing();
        ShowCells();
    }
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position += (Vector3)eventData.delta;

        foreach (Cell cell in mHighlighedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Mouse.current.position.ReadValue()))
            {
                mTargetCell = cell;
                break;
            }
            mTargetCell = null;
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        ClearCells();

        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }

        Move();
    }

    public void Place(Cell newCell)
    {
        mCurrentCell = newCell;
        mOriginalCell = newCell;

        transform.position = newCell.transform.position;
        gameObject.SetActive(true);
    }
}