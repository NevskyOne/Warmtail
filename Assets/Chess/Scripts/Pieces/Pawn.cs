using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class Pawn : BasePiece
{

    public override void Setup(Color newTeamColor, Color newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        mMovement = mColor == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);
        GetComponent<Image>().sprite = Resources.Load<Sprite>("T_Pawn");
    }
   

}
