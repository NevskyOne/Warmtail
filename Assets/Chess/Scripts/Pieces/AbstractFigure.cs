using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BasePiece : EventTrigger
{
    [HideInInspector] public Color mColor = Color.clear;
    protected Cell m0riginalCell = null;
    protected Cell mCurrentCell = null;
    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
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
        mCurrentCell.mCurrentPiece = this;

        transform.position = newCell.transform.position;
        game0bject.SetActive(true);
    }
}