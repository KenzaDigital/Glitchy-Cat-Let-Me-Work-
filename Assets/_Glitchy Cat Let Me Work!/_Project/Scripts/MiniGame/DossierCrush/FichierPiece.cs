using UnityEngine;

public class FichierPiece : MonoBehaviour
{
    public int x, y;
    private DossierBoard board;

    public void Init(int _x, int _y, DossierBoard _board)
    {
        x = _x;
        y = _y;
        board = _board;
    }

    void OnMouseDown()
    {
        board.SelectPiece(this);
    }
}
