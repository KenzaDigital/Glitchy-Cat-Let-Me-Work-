using UnityEngine;

public class FichierPiece : MonoBehaviour
{
    public int x, y;
    private DossierBoard board;

    public void Init(int x, int y, DossierBoard board)
    {
        this.x = x;
        this.y = y;
        this.board = board;
    }

    void OnMouseDown()
    {
        board.SelectPiece(this);
    }
}
