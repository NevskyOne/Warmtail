using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject CellPrefab;

    public Cell[,] AllCells = new Cell[8 , 8];

    public void Create()
    {
        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                GameObject newCell = Instantiate(CellPrefab, transform);
                RectTransform rectTrans = newCell.GetComponent<RectTransform>();

                AllCells[x, y] = newCell.GetComponent<Cell>();
                AllCells[x, y].Setup(new Vector2Int(x, y), this);
            }
        }
    }
}
