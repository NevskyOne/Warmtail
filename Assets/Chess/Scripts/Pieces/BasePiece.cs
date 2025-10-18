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

    protected Cell mTargetCell = null;
    protected List<Cell> mHighkighedCells = new List<Cell>();

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

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        transform.position += (Vector3)eventData.delta;
        foreach (Cell cell in mHighkighedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
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