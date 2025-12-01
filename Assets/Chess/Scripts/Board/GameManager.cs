using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;

    public PieceManager mPieceManager;
    void Start()
    {
        mBoard.Create();

        mPieceManager.Setup(mBoard);
    }
}
