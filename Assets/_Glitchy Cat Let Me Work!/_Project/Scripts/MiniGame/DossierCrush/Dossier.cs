using UnityEngine;

public class Dossier : MonoBehaviour
{
    public int xIndex;
    public int yIndex;
    public bool isMatched;


    public void SetPosition(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }
}
