using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board mBoard;

    [HideInInspector]
    public PieceManager mPieceManager;
    void Start()
    {
        mBoard.Create();

        //mPieceManager.Setup(mBoard);
    }
}
