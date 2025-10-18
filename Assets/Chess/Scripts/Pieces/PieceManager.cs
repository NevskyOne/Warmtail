using UnityEngine.UI;
using System;
using System.Collections.Generic; 
using UnityEngine;


public class PieceManager : MonoBehaviour
{
    public GameObject mPiecePrefab;
    private List<BasePiece> mWhitePieces = null;
    private List<BasePiece> mBlackPieces = null;

    private string[] mPieceOrder = new string[16]
    {
        "P", "P", "P", "P", "P", "P", "P", "P",
        "R", "KN", "B", "K", "Q", "B", "KN", "R"
    };

    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        { "P", typeof(Pawn)},
        { "R", typeof(Rook)},
        { "KN", typeof(Knight)},
        { "B", typeof(Bishop)},
        { "K", typeof(King)},
        { "Q", typeof(Queen)}
    };

    public void Setup(Board board)
    {
        mWhitePieces = CreatePieces(Color.white, new Color(80, 124, 159, 255), board);
        mBlackPieces = CreatePieces(Color.black, new Color(210, 95, 64, 255), board);
        PlacePieces(1, 0, mWhitePieces, board);
        PlacePieces(6, 7, mBlackPieces, board);
    }

    private List<BasePiece> CreatePieces(Color teamColor, Color spriteColor, Board board)
    {
        List<BasePiece> newPieces = new List<BasePiece>();

        for (int i = 0; i < mPieceOrder.Length; i++)
        {
            GameObject newPieceObject = Instantiate(mPiecePrefab);
            newPieceObject.transform.SetParent(transform);

            newPieceObject.transform.localScale = Vector3.one;
            newPieceObject.transform.localRotation = Quaternion.identity;

            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];

            BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
            newPieces.Add(newPiece);

            newPiece.Setup(teamColor, spriteColor, this);
        }
        return newPieces;
    }

    private void PlacePieces(int pawnRow, int royaltyRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            // Place pawns
            pieces[i].Place(board.mAllCells[i, pawnRow]);
            // Place royalty
            pieces[i + 8].Place(board.mAllCells[i, royaltyRow]);
        }
    }
}