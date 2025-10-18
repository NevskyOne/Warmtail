using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public abstract class BasePiece : MonoBehaviour
{
    [HideInInspector]
    public Color mColor = Color.clear;
    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;
    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

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
}