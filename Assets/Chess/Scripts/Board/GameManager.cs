using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;

    public PieceManager mPieceManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mBoard.Create();

        mPieceManager.Setup(mBoard);
    }
}
