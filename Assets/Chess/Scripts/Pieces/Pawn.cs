using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{
    private bool mIsFirstMove = true;

    public override void Setup(Color newTeamColor, Color newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        mMovement = mColor == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Pawn");
    }

    public override void Move()
    {
        base.Move();
        mIsFirstMove = false;
    }

    private bool MatchesState(int targetX, int targetY, CellState targetState)
    {
        CellState cellState = CellState.None;
        if (cellState == targetState)
        {
            mHighlighedCells.Add(mCurrentCell.mBoard.mAllCells[targetX, targetY]);
            return true;
        }
        return false;
    }

    protected void CheckPathing()
    {
        int currentX = (int)mCurrentCell.mBoardPosition.x;
        int currentY = (int)mCurrentCell.mBoardPosition.y;

        MatchesState(currentX - mMovement.z, currentY + mMovement.z, CellState.Enemy);

        if (MatchesState(currentX, currentY + mMovement.y, CellState.Free))
        {
            if (mIsFirstMove)
            {
                MatchesState(currentX, currentY + (mMovement.y * 2), CellState.Free);
            }
        }

        MatchesState(currentX + mMovement.z, currentY + mMovement.z, CellState.Enemy);
    }
}